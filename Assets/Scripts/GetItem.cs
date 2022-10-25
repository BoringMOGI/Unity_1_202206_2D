using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    Animator anim;
    CircleCollider2D circleCollider2D;

    private void Start()
    {
        anim = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // 트리거가 어떠한 물체와 충돌했을때 불리는 이벤트 함수.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나와 충돌한 충돌체가 Player 컴포넌트를 들고 있지 않다면..
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        circleCollider2D.enabled = false;       // 나의 콜라이더를 꺼버린다.
        player.GetScore();                      // 플레이어에게 점수를 준다.
        anim.SetTrigger("onGet");               // 애니메이터의 onGet 트리거를 누른다.
    }


    private void OnEndGetAnimation()
    {
        Destroy(gameObject);                  // 나의 게임오브젝트를 제거하겠다.
    }
}
