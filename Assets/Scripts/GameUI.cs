using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // �̱��� ����.
    // = ������ �ϳ��� �����ϴ� Ŭ������ ����Ѵ�.
    //   ���� �ʱ�ȭ �ܰ迡�� ���� static������ ��������
    //   ��𼭵� ���� �����ϵ��� �ϴ� ���.
    static GameUI instance;
    public static GameUI Instance => instance;

    [SerializeField] GameObject clearPanel;
    [SerializeField] Text scoreText;

    // Awake�� Start���� ���� �Ҹ��� �ʱ�ȭ �Լ�.
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        clearPanel.SetActive(false);        // Ŭ���� �г��� ����.
        UpdateScoreText(0);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SwitchClearPanel(bool isOn)
    {
        clearPanel.SetActive(isOn);
    }
    
}
