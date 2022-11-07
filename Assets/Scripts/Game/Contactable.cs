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
        // 몬스터가 어떠한 충돌체와 충돌했는데 그 대상이 Player 컴포넌트를 들고 있다면...
        // 방향    direction (정규화된 방향)
        // 이동량  movement (방향 * 거리)
        // 위치    position.

        // normalized : 정규화. 이동량에서 방향 벡터로 치환한다.
        // magnitude  : 거리, 이동량에서 거리를 float로 반환한다.        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        bool isLeft = direction.x < 0f;
        player.OnDamage(isLeft, pushPower);
    }
}
