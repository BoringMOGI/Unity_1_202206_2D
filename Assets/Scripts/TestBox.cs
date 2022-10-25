using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    [SerializeField] Vector3 movement;      // �̵���.
    [SerializeField] float moveSpeed;       // �ӵ�.

    void Start()
    {
        // ������ ���۵Ǹ� ���� ��ġ���� movement��ŭ ������ ���� ����.
        //transform.position += movement;

        // ������ ���۵Ǹ� �� ���� ������ �������� 2��ŭ ������ ���� ����.
        // ��ġ ���ÿ��� (���� * �Ÿ�)�� �����ش�.
        //transform.position += transform.right * 2f;

        // Vector3.right : ���� ��ǥ ���� ������ ����           <���� ��ǥ>
        // transform.right : ��(����) ��ǥ ���� ������ ����     <��� ��ǥ>

    }

    // Update is called once per frame
    void Update()
    {
        // ���� * �Ÿ� * ��Ÿ Ÿ��(������ ���̿� ���� �ӵ� ����)
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
