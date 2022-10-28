using System.Collections;
using UnityEngine;

class Test : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Download());
            Debug.Log("다운로드 함수 끝.");
        }    
    }

    IEnumerator Download()
    {
        Debug.Log("다운로드 시작!!");
        int data = 1000;
        while (true)
        {
            data -= 1;
            if (data <= 0)
                break;

            yield return null;
        }
        Debug.Log("다운로드 완료!!");
    }
}