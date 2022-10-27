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

    private void OnDrawGizmosSelected()
    {
        Vector2 point = transform.position + transform.right * checkData.offset * (isRight ? 1f : -1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point, checkData.radius);
        Gizmos.DrawRay(point, Vector2.down * checkData.rayDistance);
    }
}
