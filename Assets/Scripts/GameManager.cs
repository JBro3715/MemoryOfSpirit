using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject playground;

    public enum Tag { Player, Wall, Bullet, DestroyArea, HealthPotion, Wisp };
    [HideInInspector] public Bounds allBounds;
    [HideInInspector] public Bounds playBounds;

    private int score;

    protected override void Awake()
    {
        base.Awake();

        allBounds = background.GetComponent<Collider2D>().bounds;
        playBounds = playground.GetComponent<Collider2D>().bounds;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!");
    }
}