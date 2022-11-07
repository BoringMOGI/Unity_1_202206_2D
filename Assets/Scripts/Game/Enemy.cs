using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Movement2D movement;       // 이동 관련 컴포턴트.
    [SerializeField] Vector3 pivotOffset;       // 기준점 오차.
    [SerializeField] float radius;              // 원의 반지름.
    [SerializeField] float rayDistance;         // 광선의 길이.
    [SerializeField] LayerMask groundMask;      // 지면 마스크.

    Animator anim;
    Rigidbody2D rigid;
    new Collider2D collider2D;
    bool isAlive;
    bool isRight;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        isAlive = true;
        isRight = false;
    }
    private void Update()
    {
        if (!isAlive)
            return;

        // 가야하는 방향을 정하는 요소.
        Vector2 rayPoint = transform.position + pivotOffset * (isRight ? -1.0f : 1.0f);
        bool isCheckWall = CheckWall(rayPoint);
        bool isCheckCliff = CheckCliff(rayPoint) && movement.isGrounded;
        if (isCheckWall || isCheckCliff)
            isRight = !isRight;

        // 방향을 정한 수 Movement2D를 통해 이동.
        float x = isRight ? 1 : -1;
        movement.Move(x);
        movement.FlipX(isRight);
    }

    private bool CheckWall(Vector2 point)
    {
        // 나로부터 특정 방향으로 n만큼 떨어진 위치.
        return Physics2D.OverlapCircle(point, radius, groundMask) != null;
    }
    private bool CheckCliff(Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, rayDistance, groundMask);
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
        Vector2 rayPoint = transform.position + pivotOffset * (isRight ? -1.0f : 1.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rayPoint, radius);
        Gizmos.DrawRay(rayPoint, Vector2.down * rayDistance);
    }
}
