using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Camera.main.transform.SetParent(null);  // 메인 카메라의 부모 오브젝트는 없다.
            collision.gameObject.SetActive(false);
        }
    }
}
