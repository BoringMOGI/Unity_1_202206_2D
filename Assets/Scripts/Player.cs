using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Movement2D movement;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // 무적 시간.

    [Header("Attack")]
    [SerializeField] Vector3 attackOffset;
    [SerializeField] LayerMask attackMask;
    [SerializeField] float attackRadius;

    // Rigidbody2D 컴포넌트를 참조하는 변수.
    Rigidbody2D rigid;
    new Collider2D collider2D;

    int score;                  // 나의 점수.
    bool isLockControl;         // 캐릭터 컨트롤 막기.
    bool isPushForce;           // 내가 적에게 맞아서 일정 힘으로 날아가고 있다.
    
    void Start()
    {
        // GetComponent : 오브젝트에게 붙어있는 Rigidbody2D 컴포넌트를 검색한다.
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    // 매 프레임마다 1회씩 불리는 함수.
    void Update()
    {
        // !bool : not연산
        // true는 false, false는 true.
        if (!isLockControl && !isPushForce)
        {
            movement.Move(Input.GetAxis("Horizontal"));  // 이동.
            if (Input.GetKeyDown(KeyCode.Space))         // 점프.
                movement.Jump();
        }

        Attack();
    }
   
    private void Attack()
    {
        // 원형 충돌 영역을 만들고 그곳에 충돌한 모든 충돌체를 가져온다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + attackOffset, attackRadius, attackMask);
        if(colliders.Length > 0)
        {
            // 검색한 충돌체를 foreach문으로 순회.
            foreach(Collider2D collider in colliders)
            {
                // Enemy 컴포넌트를 검색해 OnDamaged함수 호출.
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.OnDamaged();
            }

            // 적을 공격하면 나는 위로 3만큼 뛴다.
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        }
    }


    // 기본 매개변수
    // => 인자를 넘기지 않으면 기본값으로 사용하겠다.
    public void SwitchLockControl(bool isLock, bool isStopVelocity = true)
    {
        isLockControl = isLock;
        if(isLock)
        {
            // 플레이어의 컨트롤 락이 걸린 시점에서
            // 기존 X축 속도를 지울것인가 유지할 것인가?
            float velocityX = isStopVelocity ? 0 : rigid.velocity.x;
            rigid.velocity = new Vector2(velocityX, rigid.velocity.y);
        }
    }

    public void OnDamage()
    {
        SwitchLockControl(true, false);
        collider2D.isTrigger = true;
        GameUI.Instance.SwitchClearPanel(true, false);
    }
    public void OnDamage(bool isLeft, float force)
    {
        Vector2 direction = Vector2.up;
        direction += Vector2.right * (isLeft ? -1f : 1f);

        isPushForce = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(IEGodMode());
    }
    IEnumerator IEGodMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_God");

        float time = godTime;
        float blinkTime = 0.0f;
        bool isLight = true;
        while((time -= Time.deltaTime) > 0.0f)
        {
            if ((blinkTime += Time.deltaTime) >= 0.1f)
            {
                spriteRenderer.color = isLight ? Color.red : Color.white;
                isLight = !isLight;
                blinkTime = 0.0f;
            }
            
            yield return null;      // 잠시 대기하겠다.
        }

        spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void GetScore()
    {
        Debug.Log("점수가 1 증가!");
        score += 1;

        GameUI.Instance.UpdateScoreText(score);
    }

    private void OnDrawGizmosSelected()
    {
        // 씬 뷰에 가상의 아이콘을 그려주겠다. (광선)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackOffset, attackRadius);
    }
}
