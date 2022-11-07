using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // �̺�Ʈ �Լ� : ��� �̺�Ʈ�� �Ͼ�鼭 �Ҹ��� �Լ��� �տ� On�� ���δ�.
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
        Application.Quit();         // ���� ����(=����)
    }
}
