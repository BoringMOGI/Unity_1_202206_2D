using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Contactable : MonoBehaviour
{
    [SerializeField] float pushPower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            OnAttack(player);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            OnAttack(player);
    }

    private void OnAttack(Player player)
    {
        // ���Ͱ� ��� �浹ü�� �浹�ߴµ� �� ����� Player ������Ʈ�� ��� �ִٸ�...
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
