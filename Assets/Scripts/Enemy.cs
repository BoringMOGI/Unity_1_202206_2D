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
    [SerializeField] float pushPower;
    [SerializeField] float moveSpeed;
    [SerializeField] AiCheckData checkData;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    bool isRight;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isRight = false;
    }

    private void Update()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���Ͱ� ��� �浹ü�� �浹�ߴµ� �� ����� Player ������Ʈ�� ��� �ִٸ�...
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            // ����    direction (����ȭ�� ����)
            // �̵���  movement (���� * �Ÿ�)
            // ��ġ    position.

            // normalized : ����ȭ. �̵������� ���� ���ͷ� ġȯ�Ѵ�.
            // magnitude  : �Ÿ�, �̵������� �Ÿ��� float�� ��ȯ�Ѵ�.

            Vector3 direction = (player.transform.position - transform.position).normalized;
            bool isLeft = direction.x < 0f;
            player.OnDamage(isLeft, pushPower);
        }
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

    private void OnDrawGizmosSelected()
    {
        Vector2 point = transform.position + transform.right * checkData.offset * (isRight ? 1f : -1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point, checkData.radius);
        Gizmos.DrawRay(point, Vector2.down * checkData.rayDistance);
    }
}
