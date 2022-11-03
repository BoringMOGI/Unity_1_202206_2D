using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] UnityEvent<Transform, int> onDead;

    public void OnDamaged(Transform attacker, int pushForce)
    {
        // �̹� ���� ��ü�� ���� ���� �ʴ´�.
        if (hp <= 0)
            return;

        hp -= 1;
        if (hp <= 0)
            onDead?.Invoke(attacker, pushForce);   // ����Ƽ �̺�Ʈ(=��������Ʈ) ȣ��.
    }
}
