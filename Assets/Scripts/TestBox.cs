using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    [SerializeField] Vector3 movement;      // 이동량.
    [SerializeField] float moveSpeed;       // 속도.

    void Start()
    {
        // 게임이 시작되면 현재 위치에서 movement만큼 움직인 곳에 가라.
        //transform.position += movement;

        // 게임이 시작되면 내 기준 오른쪽 방향으로 2만큼 움직인 곳에 가라.
        // 위치 계산시에는 (방향 * 거리)를 더해준다.
        //transform.position += transform.right * 2f;

        // Vector3.right : 월드 좌표 기준 오른쪽 벡터           <절대 좌표>
        // transform.right : 내(로컬) 좌표 기준 오른쪽 벡터     <상대 좌표>

    }

    // Update is called once per frame
    void Update()
    {
        // 방향 * 거리 * 델타 타임(프레임 차이에 따른 속도 보상)
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
