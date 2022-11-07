using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // 싱글톤
    // => 오직 하나만 존재하고 외부에서 빈번히 접근해야할 때 효과적으로 사용 가능한 디자인 패턴.
    private static FollowCamera instance;
    public static FollowCamera Instance => instance;    // 프로퍼티.

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        instance = this;
    }

    // 모든 update가 다 불린 이후에 호출되는 이벤트 함수.
    private void LateUpdate()
    {
        if (target == null)
            return;

        // 타겟의 위치 + Offset => 나의 위치.
        transform.position = target.position + offset;
    }

    public void ResetTarget()
    {
        target = null;
    }
}
