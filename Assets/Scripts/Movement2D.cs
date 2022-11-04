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

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;

    public bool isGrounded { get; private set; }
    private int jumpCount;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = 1;
    }
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position + pivotOffset, groundCheckRadius, groundCheckMask);
        if (isGrounded && rigid.velocity.y <= -0.1f)
            jumpCount = 1;

        // 항상 animator의 파라미터를 최신화 시킨다.
        if (anim != null)
        {
            anim.SetBool("isMove", rigid.velocity.magnitude != 0);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("velocityY", rigid.velocity.y);
        }
    }

    public void Move(float x)
    {
        rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);
    }
    public void FlipX(bool isFlipX)
    {
        spriteRenderer.flipX = isFlipX;
    }
    public bool Jump()
    {
        if (jumpCount <= 0)
            return false;

        jumpCount--;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        return true;
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
