using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject playground;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    public enum Tag { Player, Wall, Bullet, DestroyArea, HealthPotion, Wisp };
    [HideInInspector] public Bounds allBounds;
    [HideInInspector] public Bounds playBounds;

    private WaitForSeconds timerSeconds;
    private DateTime endTime;
    private int score;
    private int playerCount;

    private const int PLAY_TIME = 2;
        
    protected override void Awake()
    {
        base.Awake();

        allBounds = background.GetComponent<Collider2D>().bounds;
        playBounds = playground.GetComponent<Collider2D>().bounds;

        timerSeconds = new(1f);
        score = 0;
        playerCount = 0;

        AddScore(0);
        AddPlayer();
        StartCountdown(PLAY_TIME);
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = $"{this.score}";
    }

    public void AddPlayer()
    {
        playerCount++;
        playerCountText.text = playerCount.ToString();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!");
    }

    private void StartCountdown(int minutes)
    {
        endTime = DateTime.Now.AddMinutes(minutes);
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(true)
        {
            TimeSpan playTime = endTime - DateTime.Now;

            if(playTime <= TimeSpan.Zero)
            {
                playTime = TimeSpan.Zero;
                GameOver();
            }

            timeText.text = string.Format("{0:00}:{1:00}", playTime.Minutes, playTime.Seconds);

            yield return timerSeconds;
        }
    }
}