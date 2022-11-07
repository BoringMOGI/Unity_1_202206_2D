using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // 이벤트 함수 : 어떠한 이벤트가 일어나면서 불리는 함수는 앞에 On을 붙인다.
    public void OnNewGame()
    {
        Debug.Log("New Game");
        SceneManager.LoadScene("Game");
    }
    public void OnContinue()
    {
        Debug.Log("Continue");
    }

    public void OnExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();         // 게임 종료(=끄기)
    }
}
