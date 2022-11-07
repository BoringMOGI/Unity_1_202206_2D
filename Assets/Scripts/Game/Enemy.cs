using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Movement2D movement;       // �̵� ���� ������Ʈ.
    [SerializeField] Vector3 pivotOffset;       // ������ ����.
    [SerializeField] float radius;              // ���� ������.
    [SerializeField] float rayDistance;         // ������ ����.
    [SerializeField] LayerMask groundMask;      // ���� ����ũ.

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

        // �����ϴ� ������ ���ϴ� ���.
        Vector2 rayPoint = transform.position + pivotOffset * (isRight ? -1.0f : 1.0f);
        bool isCheckWall = CheckWall(rayPoint);
        bool isCheckCliff = CheckCliff(rayPoint) && movement.isGrounded;
        if (isCheckWall || isCheckCliff)
            isRight = !isRight;

        // ������ ���� �� Movement2D�� ���� �̵�.
        float x = isRight ? 1 : -1;
        movement.Move(x);
        movement.FlipX(isRight);
    }

    private bool CheckWall(Vector2 point)
    {
        // ���κ��� Ư�� �������� n��ŭ ������ ��ġ.
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
        Vector2 rayPoint = transform.position + pivotOffset * (isRight ? -1.0f : 1.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rayPoint, radius);
        Gizmos.DrawRay(rayPoint, Vector2.down * rayDistance);
    }
}
