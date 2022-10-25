using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ���� �տ� �ٴ� [...]�� �ش� �������� �Ӽ��� �ο��ϰڴ�.
    // SerializeField �Ӽ� : Component�� �ʵ�ν� �����Ű�ڴ�.
    // ������Ʈ �ʵ忡 �����ϰڴ� ������ �ؼ��Ѵ�.
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] string playerName;
    [SerializeField] int hp;
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.
    [SerializeField] float jumpPower;       // ������.

    [SerializeField] Transform groundPivot; // ���� üũ Ray�� ������.
    [SerializeField] LayerMask groundMask;  // ���� üũ ���̾� ����ũ.
    [SerializeField] float groundRadius;    // ���� üũ ������.

    // Rigidbody2D ������Ʈ�� �����ϴ� ����.
    Rigidbody2D rigid;
    Animator anim;

    int score;                  // ���� ����.
    int maxCount;               // �ִ� ���� Ƚ��.

    int jumpCount;              // (������)���� Ƚ��.
    bool isGrounded;            // ���� ���� �� �ִ°�?

    bool isLockControl;         // ĳ���� ��Ʈ�� ����.
    
    // �̺�Ʈ �Լ�
    // => ���� ȣ���ϴ°� �ƴϰ� �������� ���ؼ� ȣ��Ǵ� ��.

    // ����Ƽ �̺�Ʈ Start.
    // �Ķ������� �Ǿ������� ���� ���� �� ���ʿ� 1�� �Ҹ��� �Լ�.
    void Start()
    {
        // GetComponent : ������Ʈ���� �پ��ִ� Rigidbody2D ������Ʈ�� �˻��Ѵ�.
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();

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
        if (!isLockControl)
        {
            Movement();
            Jump();
        }

        // �׻� animator�� �Ķ���͸� �ֽ�ȭ ��Ų��.
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
        isGrounded = Physics2D.OverlapCircle(groundPivot.position, groundRadius, groundMask);
        if (isGrounded && rigid.velocity.y <= 0f)
            jumpCount = maxCount;
    }
    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //transform.position += transform.right * x * moveSpeed * Time.deltaTime;

        // ���� �ӵ��� x���� �Է¿� ����
        // ���� �ӵ��� y���� ���� �״�� �����Ѵ�.
        rigid.velocity = new Vector2(moveSpeed * x, rigid.velocity.y);
        anim.SetBool("isMove", x != 0);

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

    public void OnGoal()
    {
        Debug.Log("�� ���Դ�!!");
        isLockControl = true;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        anim.SetBool("isMove", false);
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
        if(groundPivot != null)
        {
            Gizmos.DrawWireSphere(groundPivot.position, groundRadius);
        }
    }
}
