using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : Singleton<GameManager>
{
    public enum Tag { Player, Wall, Bullet, DestroyArea, HealthPotion, Wisp };
    public enum LEVEL { Normal = 1, Hard = 2 };

    public ReactiveProperty<LEVEL> level;
    public ReactiveProperty<bool> isPlayed;

    [HideInInspector] public Bounds allBounds;
    [HideInInspector] public Bounds playBounds;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject playground;

    private Dictionary<LEVEL, Action<int>> scoreMap;
    private WaitForSeconds timerSeconds;
    private DateTime endTime;
    private int score;
    private int playerCount;

    private int normalScore;
    private int hardScore;
    private int timeBonus;

    private const int PLAY_TIME = 2;
    private const int LEVEL_CHANGE_TIME = 1;
    private const int POINT_TIME = 10;
    private const int TIME_BONUS = 30;
        
    protected override void Awake()
    {
        base.Awake();

        allBounds = background.GetComponent<Collider2D>().bounds;
        playBounds = playground.GetComponent<Collider2D>().bounds;
        level.Value = LEVEL.Normal;
        isPlayed.Value = true;

        scoreMap = new Dictionary<LEVEL, Action<int>>
        {
            { LEVEL.Normal, score => normalScore += score },
            { LEVEL.Hard, score => hardScore += score }
        };

        timerSeconds = new(1f);
        score = 0;
        playerCount = 0;

        normalScore = 0;
        hardScore = 0;
        timeBonus = 0;

        AddScore(0);
        AddPlayer();
        StartCountdown(PLAY_TIME);
    }

    #region Public Field
    public void AddScore(int score, bool isTimeBonus = false)
    {
        var levelScore = score * (int)level.Value;
        this.score += levelScore;
        UIManager.Instance.ApplyScoreUI(this.score);

        if(scoreMap.ContainsKey(level.Value) && !isTimeBonus)
        {
            scoreMap[level.Value](levelScore);
        }
    }

    public void AddPlayer()
    {
        playerCount++;
        UIManager.Instance.ApplyPlayerCountUI(playerCount);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!");
        isPlayed.Value = false;
        StopAllCoroutines();

        UIManager.Instance.FinalScoreUI(normalScore, hardScore, timeBonus, false);
    }
    #endregion

    #region Private Field
    private void Clear()
    {
        Debug.Log("Clear!!");
        isPlayed.Value = false;
        StopAllCoroutines();

        UIManager.Instance.FinalScoreUI(normalScore, hardScore, timeBonus, true);
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
            
            // 남은 시간이 0초 이하인 경우
            if(playTime <= TimeSpan.Zero)
            {
                playTime = TimeSpan.Zero;
                Clear();
            }
            
            // 남은 시간을 체크해서 Hard 난이도로 변경
            if(playTime <= TimeSpan.FromMinutes(LEVEL_CHANGE_TIME) && level.Value != LEVEL.Hard)
            {
                level.Value = LEVEL.Hard;
            }

            // 특정 플레이 타임마다 포인트 획득
            if(playTime.Seconds % POINT_TIME == 0)
            {
                AddScore(TIME_BONUS, true);
                timeBonus += TIME_BONUS * (int)level.Value;
            }

            UIManager.Instance.ApplyTimeUI(playTime);

            yield return timerSeconds;
        }
    }
    #endregion
}