using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // �̱���
    // => ���� �ϳ��� �����ϰ� �ܺο��� ����� �����ؾ��� �� ȿ�������� ��� ������ ������ ����.
    private static FollowCamera instance;
    public static FollowCamera Instance => instance;    // ������Ƽ.

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        instance = this;
    }

    // ��� update�� �� �Ҹ� ���Ŀ� ȣ��Ǵ� �̺�Ʈ �Լ�.
    private void LateUpdate()
    {
        if (target == null)
            return;

        // Ÿ���� ��ġ + Offset => ���� ��ġ.
        transform.position = target.position + offset;
    }

    public void ResetTarget()
    {
        target = null;
    }
}
