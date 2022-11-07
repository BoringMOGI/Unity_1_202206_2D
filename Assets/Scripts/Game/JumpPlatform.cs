using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] float force;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            anim.SetTrigger("onActive");
            AudioManager.Instance.PlaySE("JumpPlatform");
        }

        Movement2D movement2D = collision.gameObject.GetComponent<Movement2D>();
        if(movement2D != null)
        {
            movement2D.OnJumpCountZero();
        }
    }
}
