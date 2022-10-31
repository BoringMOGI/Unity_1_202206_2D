using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    struct OverlapCircle
    {
        public Transform pivot;
        public LayerMask mask;
        public float radius;
    }

    // ���� �տ� �ٴ� [...]�� �ش� �������� �Ӽ��� �ο��ϰڴ�.
    // SerializeField �Ӽ� : Component�� �ʵ�ν� �����Ű�ڴ�.
    // ������Ʈ �ʵ忡 �����ϰڴ� ������ �ؼ��Ѵ�.
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] string playerName;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // ���� �ð�.

    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float jumpPower;       // ������.

    [SerializeField] OverlapCircle overlapGround;   // ���� üũ ���� ����ü.
    [SerializeField] OverlapCircle overlapAttack;   // ���� üũ ���� ����ü.

    // Rigidbody2D ������Ʈ�� �����ϴ� ����.
    Rigidbody2D rigid;
    Animator anim;
    new Collider2D collider2D;

    int score;                  // ���� ����.
    int maxCount;               // �ִ� ���� Ƚ��.

    int jumpCount;              // (������)���� Ƚ��.
    bool isGrounded;            // ���� ���� �� �ִ°�?
    bool isLockControl;         // ĳ���� ��Ʈ�� ����.
    bool isPushForce;           // ���� ������ �¾Ƽ� ���� ������ ���ư��� �ִ�.
    
    // �̺�Ʈ �Լ�
    // => ���� ȣ���ϴ°� �ƴϰ� �������� ���ؼ� ȣ��Ǵ� ��.

    // ����Ƽ �̺�Ʈ Start.
    // �Ķ������� �Ǿ������� ���� ���� �� ���ʿ� 1�� �Ҹ��� �Լ�.
    void Start()
    {
        // GetComponent : ������Ʈ���� �پ��ִ� Rigidbody2D ������Ʈ�� �˻��Ѵ�.
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();

        maxCount = 1;
        jumpCount = maxCount;

        Debug.Log("�÷��̾ ������ �Ǿ���");
        // Time.timeScale = 0.5f;
    }

    // �� �����Ӹ��� 1ȸ�� �Ҹ��� �Լ�.
    void Update()
    {
        CheckGround();

        // !bool : not����
        // true�� false, false�� true.
        if (!isLockControl && !isPushForce)
        {
            Movement();
            Jump();
        }

        Attack();

        // �׻� animator�� �Ķ���͸� �ֽ�ȭ ��Ų��.
        anim.SetBool("isMove", rigid.velocity.magnitude != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    private void CheckGround()
    {
        // groundPivot���� �Ʒ� �������� Ray(����)�� �߻��� Hit������ �޾ƿ´�.
        // ���� hit�� �ݶ��̴��� �����Ѵٸ� isGrounded�� true�� �����.
        // ������ ������ �̿��ϸ� ���� �������� �������� �߻��Ѵ�.
        // RaycastHit2D hit = Physics2D.Raycast(groundPivot.position, -transform.up, groundLength, groundMask);
        // isGrounded = hit.collider != null;

        // �ݶ��̴� ����� ���� �浹 ������ ����� �� ���� ��ü�� �浹�ϸ�(=������)
        // �ش� ��ü�� Collider�� ��ȯ���ش�.
        isGrounded = Physics2D.OverlapCircle(overlapGround.pivot.position, overlapGround.radius, overlapGround.mask);
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

        // ���� �ӵ��� x���� �Է¿� ����
        // ���� �ӵ��� y���� ���� �״�� �����Ѵ�.
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
        // Input.Getkey : �ش� Ű�� ������ �ִ� ���� ��� true.
        // Input.GetKeyDown : �ش� Ű�� ������ �� ���� 1��.
        // Input.GetKeyUp : �ش� Ű�� ���� �� ���� 1��.

        // ���� ���� �� �ְ� SpaceŰ�� �����ٸ�.
        if (jumpCount > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            jumpCount--;

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    private void Attack()
    {
        // ���� �浹 ������ ����� �װ��� �浹�� ��� �浹ü�� �����´�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(overlapAttack.pivot.position, overlapAttack.radius, overlapAttack.mask);
        if(colliders.Length > 0)
        {
            // �˻��� �浹ü�� foreach������ ��ȸ.
            foreach(Collider2D collider in colliders)
            {
                // Enemy ������Ʈ�� �˻��� OnDamaged�Լ� ȣ��.
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                enemy.OnDamaged();
            }

            // ���� �����ϸ� ���� ���� 3��ŭ �ڴ�.
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        }
    }

    // �⺻ �Ű�����
    // => ���ڸ� �ѱ��� ������ �⺻������ ����ϰڴ�.
    public void SwitchLockControl(bool isLock, bool isStopVelocity = true)
    {
        isLockControl = isLock;
        if(isLock)
        {
            // �÷��̾��� ��Ʈ�� ���� �ɸ� ��������
            // ���� X�� �ӵ��� ������ΰ� ������ ���ΰ�?
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
            
            yield return null;      // ��� ����ϰڴ�.
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
        Debug.Log("������ 1 ����!");
        score += 1;

        GameUI.Instance.UpdateScoreText(score);
    }

    private void OnDrawGizmosSelected()
    {
        // �� �信 ������ �������� �׷��ְڴ�. (����)
        Gizmos.color = Color.green;
        if(overlapGround.pivot != null)
        {
            Gizmos.DrawWireSphere(overlapGround.pivot.position, overlapGround.radius);
        }

        Gizmos.color = Color.red;
        if (overlapAttack.pivot != null)
        {
            Gizmos.DrawWireSphere(overlapAttack.pivot.position, overlapAttack.radius);
        }
    }
}
