using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private GameObject finalInfoPanel;
    [SerializeField] private Image finalImage;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private TextMeshProUGUI finalNormalScoreText;
    [SerializeField] private TextMeshProUGUI finalHardScoreText;
    [SerializeField] private TextMeshProUGUI finalTimeBonusText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [SerializeField] private Sprite gameoverSprite;
    [SerializeField] private Sprite clearSprite;

    private const string NORMAL_HEX_COLOR = "#53FFFA";
    private const string HARD_HEX_COLOR = "#FF1317";

    private void Start()
    {
        levelText.text = $"NORMAL";
        ColorUtility.TryParseHtmlString(NORMAL_HEX_COLOR, out Color color);
        levelText.color = color;

        GameManager.Instance.level.Subscribe(level =>
        {
            if(level == GameManager.LEVEL.Hard)
            {
                levelText.text = $"HARD";
                ColorUtility.TryParseHtmlString(HARD_HEX_COLOR, out Color color);
                levelText.color = color;
            }
        });
    }

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
