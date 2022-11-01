using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] Vector3 pivotOffset;
    [SerializeField] LayerMask groundCheckMask;
    [SerializeField] float groundCheckRadius;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rigid;

    public bool isGrounded { get; private set; }
    private int jumpCount;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position + pivotOffset, groundCheckRadius, groundCheckMask);
        if (isGrounded && rigid.velocity.y <= -0.1f)
            jumpCount = 1;

        // 항상 animator의 파라미터를 최신화 시킨다.
        anim.SetBool("isMove", rigid.velocity.magnitude != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rigid.velocity.y);
    }

    public void Move(float x)
    {
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);
        if (x < 0)
            spriteRenderer.flipX = true;
        else if (x > 0)
            spriteRenderer.flipX = false;
    }
    public void Jump()
    {
        if (jumpCount <= 0)
            return;

        jumpCount--;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    public void OnJumpCountZero()
    {
        jumpCount = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + pivotOffset, groundCheckRadius);
    }

}
