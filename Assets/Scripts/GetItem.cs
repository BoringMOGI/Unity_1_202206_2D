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

    // Ʈ���Ű� ��� ��ü�� �浹������ �Ҹ��� �̺�Ʈ �Լ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �浹�� �浹ü�� Player ������Ʈ�� ��� ���� �ʴٸ�..
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        circleCollider2D.enabled = false;       // ���� �ݶ��̴��� ��������.
        player.GetScore();                      // �÷��̾�� ������ �ش�.
        anim.SetTrigger("onGet");               // �ִϸ������� onGet Ʈ���Ÿ� ������.
    }


    private void OnEndGetAnimation()
    {
        Destroy(gameObject);                  // ���� ���ӿ�����Ʈ�� �����ϰڴ�.
    }
}
