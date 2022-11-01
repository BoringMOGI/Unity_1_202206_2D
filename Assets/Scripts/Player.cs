using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Movement2D movement;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // ���� �ð�.

    [Header("Attack")]
    [SerializeField] Vector3 attackOffset;
    [SerializeField] LayerMask attackMask;
    [SerializeField] float attackRadius;

    // Rigidbody2D ������Ʈ�� �����ϴ� ����.
    Rigidbody2D rigid;
    new Collider2D collider2D;

    int score;                  // ���� ����.
    bool isLockControl;         // ĳ���� ��Ʈ�� ����.
    bool isPushForce;           // ���� ������ �¾Ƽ� ���� ������ ���ư��� �ִ�.
    
    void Start()
    {
        // GetComponent : ������Ʈ���� �پ��ִ� Rigidbody2D ������Ʈ�� �˻��Ѵ�.
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    // �� �����Ӹ��� 1ȸ�� �Ҹ��� �Լ�.
    void Update()
    {
        // !bool : not����
        // true�� false, false�� true.
        if (!isLockControl && !isPushForce)
        {
            movement.Move(Input.GetAxis("Horizontal"));  // �̵�.
            if (Input.GetKeyDown(KeyCode.Space))         // ����.
                movement.Jump();
        }

        Attack();
    }
   
    private void Attack()
    {
        // ���� �浹 ������ ����� �װ��� �浹�� ��� �浹ü�� �����´�.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + attackOffset, attackRadius, attackMask);
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
            
            yield return null;      // ��� ����ϰڴ�.
        }

        spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackOffset, attackRadius);
    }
}
