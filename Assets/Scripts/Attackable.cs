using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask mask;
    [SerializeField] float radius;
    [SerializeField] float jumpPower;

    Rigidbody2D rigid;
    bool isOn;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 플레이어의 공격은 아래로 떨어지는 중에만 한다.
        if (rigid.velocity.y >= -0.1f || !isOn)
            return;

        bool isAttack = false;

        Collider2D[] contacts = Physics2D.OverlapCircleAll(transform.position + offset, radius, mask);
        foreach (Collider2D contact in contacts)
        {
            Enemy enemy = contact.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnDamaged();
                isAttack = true;
            }
        }

        if(isAttack)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            AudioManager.Instance.PlaySE("Attack");
        }
    }

    public void Switch(bool isOn)
    {
        this.isOn = isOn;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
