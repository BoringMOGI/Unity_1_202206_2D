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

    [SerializeField] Text scoreText;
    [SerializeField] Image[] hpImages;
    [SerializeField] Sprite hpOnSprite;
    [SerializeField] Sprite hpOffSprite;

    [Header("Ŭ���� ����")]
    [SerializeField] GameObject clearPanel;     // ���� ����� ��µǴ� �г�.
    [SerializeField] Text clearText;            // ���� ����� ��µǴ� �ؽ�Ʈ.
    [SerializeField] Color clearColor;
    [SerializeField] Color gameoverColor;

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

    public void SwitchClearPanel(bool isOn, bool isGameClear)
    {
        clearPanel.SetActive(isOn);
        clearText.text = isGameClear ? "GAME CLEAR!!" : "GAME OVER..";
        clearText.color = isGameClear ? clearColor : gameoverColor;
    }
    
    public void UpdateHpImage(int hp)
    {
        for(int i = 0; i< hpImages.Length; i++)
        {
            hpImages[i].sprite = (i < hp) ? hpOnSprite : hpOffSprite;
        }
    }

}
