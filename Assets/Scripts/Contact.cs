using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    // 내가 어떠한 충돌체와 충돌한 순간 호출되는 함수. (1회)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌했다!!");
    }
    // 내가 어떠한 충돌체와 충돌하고 있는 동안 계속 호출.
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("충돌 중이다");
    }
    // 내가 충돌한 물체와 떨어졌을때 호출 되는 함수. (1회)
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("떨어졌다.");
    }
}
