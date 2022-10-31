using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    struct AiCheckData
    {
        public float offset;            // ������ ����.
        public float radius;            // ���� ������.
        public float rayDistance;       // ������ ����.
        public LayerMask mask;
    }

    [SerializeField] float moveSpeed;
    [SerializeField] AiCheckData checkData;

    Rigidbody2D rigid;
    new Collider2D collider2D;
    SpriteRenderer spriteRenderer;
    Animator anim;

    bool isAlive;
    bool isRight;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isAlive = true;
        isRight = false;
    }
    private void Update()
    {
        if (!isAlive)
            return;

        Vector2 point = transform.position + transform.right * checkData.offset * (isRight ? 1f : -1f);
        if (CheckWall(point) || CheckCliff(point))
        {
            isRight = !isRight;
            spriteRenderer.flipX = isRight;
        }

        Vector2 velocity = transform.right * moveSpeed * (isRight ? 1f : -1f);
        velocity.y = rigid.velocity.y;

        rigid.velocity = velocity;
        anim.SetBool("isMove", rigid.velocity.magnitude != 0.0f);
    }

    private bool CheckWall(Vector2 point)
    {
        // ���κ��� Ư�� �������� n��ŭ ������ ��ġ.
        return Physics2D.OverlapCircle(point, checkData.radius, checkData.mask) != null;
    }
    private bool CheckCliff(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, checkData.rayDistance, checkData.mask);
        return hit.collider == null;
    }

    public void OnDamaged()
    {
        anim.SetTrigger("onHit");
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
        collider2D.enabled = false;

        isAlive = false;

        // nameof(Method) : �ش� �޼����� �̸��� string���� ��ȯ�Ѵ�.
        // Invoke : �ش� �̸��� �Լ��� n�ʵڿ� ȣ���ϼ���.
        Invoke(nameof(OnDeleteEnemy), 2.0f);  
    }
    private void OnDeleteEnemy()
    {
        Destroy(gameObject);    // ���� gameObject�� �����϶�.
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 point = transform.position + transform.right * checkData.offset * (isRight ? 1f : -1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point, checkData.radius);
        Gizmos.DrawRay(point, Vector2.down * checkData.rayDistance);
    }
}
