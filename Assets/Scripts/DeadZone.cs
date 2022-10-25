using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Camera.main.transform.SetParent(null);  // ���� ī�޶��� �θ� ������Ʈ�� ����.
            collision.gameObject.SetActive(false);
        }
    }
}
