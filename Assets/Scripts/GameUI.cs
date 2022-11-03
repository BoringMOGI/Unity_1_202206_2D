using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // 싱글톤 패턴.
    // = 오로지 하나만 존재하는 클래스에 사용한다.
    //   게임 초기화 단계에서 나를 static변수에 참조시켜
    //   어디서든 쉽게 접근하도록 하는 방법.
    static GameUI instance;
    public static GameUI Instance => instance;

    [SerializeField] Text scoreText;
    [SerializeField] Image[] hpImages;
    [SerializeField] Sprite hpOnSprite;
    [SerializeField] Sprite hpOffSprite;

    [Header("클리어 관련")]
    [SerializeField] GameObject clearPanel;     // 게임 종료시 출력되는 패널.
    [SerializeField] Text clearText;            // 게임 종료시 출력되는 텍스트.
    [SerializeField] Color clearColor;
    [SerializeField] Color gameoverColor;

    // Awake는 Start보다 먼저 불리는 초기화 함수.
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        clearPanel.SetActive(false);        // 클리어 패널을 끈다.
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
