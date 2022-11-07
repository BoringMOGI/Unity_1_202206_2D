using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Attackable weapon;
    [SerializeField] Movement2D movement;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // 무적 시간.

    // Rigidbody2D 컴포넌트를 참조하는 변수.
    Rigidbody2D rigid;
    new Collider2D collider2D;

    bool isAlive => hp > 0;     // 플레이어의 생존 여부.
    int score;                  // 나의 점수.
    bool isLockControl;         // 캐릭터 컨트롤 막기.
    bool isPushForce;           // 내가 적에게 맞아서 일정 힘으로 날아가고 있다.
    
    void Start()
    {
        // GetComponent : 오브젝트에게 붙어있는 Rigidbody2D 컴포넌트를 검색한다.
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        GameUI.Instance.UpdateHpImage(hp);  // HP 이미지 업데이트.
        weapon.Switch(true);                // 무기를 켠다.
    }

    // 매 프레임마다 1회씩 불리는 함수.
    void Update()
    {
        // !bool : not연산
        // true는 false, false는 true.
        if (isLockControl || isPushForce || !isAlive)
            return;

        // GetAxisRaw : -1 or 0 or 1
        // GetAxis    : -1.0f ~ 1.0f
        float x = Input.GetAxis("Horizontal");
        movement.Move(x);                           // 이동.
        if (x < 0)
            movement.FlipX(true);
        else if (x > 0)
            movement.FlipX(false);

        if (Input.GetKeyDown(KeyCode.Space))        // 점프.
        {
            bool isJump = movement.Jump();
            if (isJump)
                AudioManager.Instance.PlaySE("Jump");
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

    public void OnDead()
    {
        hp = 0;
        GameUI.Instance.UpdateHpImage(hp);
        AudioManager.Instance.StopBGM();

        SwitchLockControl(true, false);                     // 컨트롤 막기.
        collider2D.isTrigger = true;                        // 트리거로 변경.
        GameUI.Instance.SwitchClearPanel(true, false);      // 클리어 패널 활성화.
        FollowCamera.Instance.ResetTarget();                // 카메라 끊기.
        weapon.Switch(false);                               // 무기 비활성화.
    }
    public void OnDamage(bool isLeft, float force)
    {
        if (!isAlive)
            return;

        Vector2 direction = Vector2.up;
        direction += Vector2.right * (isLeft ? -1f : 1f);

        isPushForce = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(direction * force, ForceMode2D.Impulse);

        hp -= 1;
        GameUI.Instance.UpdateHpImage(hp);
        AudioManager.Instance.PlaySE("Hit");

        if (isAlive)
        {
            StartCoroutine(IEGodMode());
            StartCoroutine(IEForceLock());
        }
        else
        {
            OnDead();
        }
    }

    IEnumerator IEGodMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_God");
        weapon.Switch(false);                               // 무기를 끈다.

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
        weapon.Switch(true);                                // 무기를 켠다.
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    IEnumerator IEForceLock()
    {
        while(true)
        {
            if (rigid.velocity.y <= 0f && movement.isGrounded)
                break;

            yield return null;
        }

        isPushForce = false;
    }
    public void GetScore()
    {
        Debug.Log("점수가 1 증가!");
        score += 1;

        GameUI.Instance.UpdateScoreText(score);
    }
}
