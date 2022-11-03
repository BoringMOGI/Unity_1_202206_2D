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
        // 이미 죽은 개체는 공격 받지 않는다.
        if (hp <= 0)
            return;

        hp -= 1;
        if (hp <= 0)
            onDead?.Invoke(attacker, pushForce);   // 유니티 이벤트(=델리게이트) 호출.
    }
}
