using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    // ���� ��� �浹ü�� �浹�� ���� ȣ��Ǵ� �Լ�. (1ȸ)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�浹�ߴ�!!");
    }
    // ���� ��� �浹ü�� �浹�ϰ� �ִ� ���� ��� ȣ��.
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("�浹 ���̴�");
    }
    // ���� �浹�� ��ü�� ���������� ȣ�� �Ǵ� �Լ�. (1ȸ)
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("��������.");
    }
}
