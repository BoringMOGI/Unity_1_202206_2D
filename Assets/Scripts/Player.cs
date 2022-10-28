using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 변수 앞에 붙는 [...]는 해당 변수에게 속성을 부여하겠다.
    // SerializeField 속성 : Component에 필드로써 노출시키겠다.
    // 컴포넌트 필드에 연결하겠다 정도로 해석한다.
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] string playerName;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // 무적 시간.

    [SerializeField] float moveSpeed;       // 이동 속도.
    [SerializeField] float jumpPower;       // 점프력.

    [SerializeField] Transform groundPivot; // 지면 체크 Ray의 기준점.
    [SerializeField] LayerMask groundMask;  // 지면 체크 레이어 마스크.
    [SerializeField] float groundRadius;    // 지면 체크 반지름.

    // Rigidbody2D 컴포넌트를 참조하는 변수.
    Rigidbody2D rigid;
    Animator anim;
    new Collider2D collider2D;

    int score;                  // 나의 점수.
    int maxCount;               // 최대 점프 횟수.

    int jumpCount;              // (가능한)점프 횟수.
    bool isGrounded;            // 내가 땅에 서 있는가?
    bool isLockControl;         // 캐릭터 컨트롤 막기.
    bool isPushForce;           // 내가 적에게 맞아서 일정 힘으로 날아가고 있다.
    
    // 이벤트 함수
    // => 내가 호출하는게 아니고 누군가에 의해서 호출되는 것.

    // 유니티 이벤트 Start.
    // 파란색으로 되어있으며 게임 시작 시 최초에 1번 불리는 함수.
    void Start()
    {
        // GetComponent : 오브젝트에게 붙어있는 Rigidbody2D 컴포넌트를 검색한다.
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();

        maxCount = 1;
        jumpCount = maxCount;

        Debug.Log("플레이어가 시작이 되었다");
        // Time.timeScale = 0.5f;
    }

    // 매 프레임마다 1회씩 불리는 함수.
    void Update()
    {
        CheckGround();

        // !bool : not연산
        // true는 false, false는 true.
        if (!isLockControl && !isPushForce)
        {
            Movement();
            Jump();
        }

        // 항상 animator의 파라미터를 최신화 시킨다.
        anim.SetBool("isMove", rigid.velocity.magnitude != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    private void CheckGround()
    {
        // groundPivot기준 아래 방향으로 Ray(광선)을 발사해 Hit정보를 받아온다.
        // 만약 hit된 콜라이더가 존재한다면 isGrounded를 true로 만든다.
        // 하지만 광선을 이용하면 여러 지형에서 문제점이 발생한다.
        // RaycastHit2D hit = Physics2D.Raycast(groundPivot.position, -transform.up, groundLength, groundMask);
        // isGrounded = hit.collider != null;

        // 콜라이더 비슷한 원형 충돌 영역을 만들고 그 곳에 물체가 충돌하면(=들어오면)
        // 해당 물체의 Collider를 반환해준다.
        isGrounded = Physics2D.OverlapCircle(groundPivot.position, groundRadius, groundMask);
        if (isGrounded && rigid.velocity.y <= 0f)
        {
            jumpCount = maxCount;
            isPushForce = false;
        }
    }
    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //transform.position += transform.right * x * moveSpeed * Time.deltaTime;

        // 현재 속도의 x축은 입력에 따라서
        // 현재 속도의 y축은 원래 그대로 변경한다.
        rigid.velocity = new Vector2(moveSpeed * x, rigid.velocity.y);

        if (x == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (x == 1)
        {
            spriteRenderer.flipX = false;
        }
    }
    private void Jump()
    {
        // Input.Getkey : 해당 키를 누르고 있는 동안 계속 true.
        // Input.GetKeyDown : 해당 키를 누르는 그 순간 1번.
        // Input.GetKeyUp : 해당 키를 때는 그 순간 1번.

        // 내가 땅에 서 있고 Space키를 눌렀다면.
        if (jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            jumpCount--;

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
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


    public void OnDamage()
    {
        SwitchLockControl(true, false);
        collider2D.isTrigger = true;
        GameUI.Instance.SwitchClearPanel(true, false);
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
        Gizmos.color = Color.green;
        if(groundPivot != null)
        {
            Gizmos.DrawWireSphere(groundPivot.position, groundRadius);
        }
    }
}
