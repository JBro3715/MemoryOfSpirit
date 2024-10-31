using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    [SerializeField] private GameObject finalInfoPanel;
    [SerializeField] private Image finalImage;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI finalNormalScoreText;
    [SerializeField] private TextMeshProUGUI finalHardScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeBonusText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [SerializeField] private Sprite gameoverSprite;
    [SerializeField] private Sprite clearSprite;

    public void ApplyTimeUI(TimeSpan value)
    {
        timeText.text = string.Format("{0:00}:{1:00}", value.Minutes, value.Seconds);
        finalTimeText.text = string.Format("{0:00}:{1:00}", value.Minutes, value.Seconds);
    }

    public void ApplyScoreUI(int value)
    {
        scoreText.text = $"{value}";
        finalScoreText.text = $"{value}";
    }

    public void ApplyPlayerCountUI(int value)
    {
        playerCountText.text = $"{value}";
    }

    public void FinalScoreUI(int normal, int hard, int timeBonus, bool isCleared)
    {
        finalInfoPanel.SetActive(true);

        finalImage.sprite = isCleared ? clearSprite : gameoverSprite;

        finalNormalScoreText.text = $"{normal}";
        finalHardScoreText.text = $"{hard}";
        finalTimeBonusText.text = $"{timeBonus}";
    }
}
