using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    struct AiCheckData
    {
        public float offset;            // 기준점 오차.
        public float radius;            // 원의 반지름.
        public float rayDistance;       // 광선의 길이.
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
        // 나로부터 특정 방향으로 n만큼 떨어진 위치.
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

        // nameof(Method) : 해당 메서드의 이름을 string으로 반환한다.
        // Invoke : 해당 이름의 함수를 n초뒤에 호출하세요.
        Invoke(nameof(OnDeleteEnemy), 2.0f);  
    }
    private void OnDeleteEnemy()
    {
        Destroy(gameObject);    // 나의 gameObject를 삭제하라.
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 point = transform.position + transform.right * checkData.offset * (isRight ? 1f : -1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point, checkData.radius);
        Gizmos.DrawRay(point, Vector2.down * checkData.rayDistance);
    }
}
