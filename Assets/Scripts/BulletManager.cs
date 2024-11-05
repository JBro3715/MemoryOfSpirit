using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UniRx;

public class BulletManager : Singleton<BulletManager>
{
    [Serializable]
    public class DifficultySettings
    {
        public int straightBulletCount;
        public int straightBulletTerm;
        public int straightFenceBulletTerm;
    }

    [Serializable]
    public class BulletSettings
    {
        public DifficultySettings normal;
        public DifficultySettings hard;
    }

    public GameObject meteor;

    private BulletSettings bulletSettings;
    private BulletPool bulletPool;

    private StraightBullet straightBullet;
    private StraightFenceBullet straightFenceBullet;
    private SpreadBullet spreadBullet;

    private readonly WaitForSeconds fireWaitSeconds = new(1f);

    private string settingFilePath;
    private int straightBulletCount;
    private int straightBulletTerm;
    private int straightFenceBulletTerm;

    private void Start()
    {
        GameManager.Instance.level.Subscribe(level =>
        {
            if (level == GameManager.LEVEL.Hard)
            {
                ChangeHardMode();
            }
        });

        GameManager.Instance.isPlayed.Subscribe(isPlayed =>
        {
            if (!isPlayed)
            {
                StopAllCoroutines();
            }
        });

        bulletPool = GetComponent<BulletPool>();

        straightBullet = new StraightBullet(bulletPool, GameManager.Instance.allBounds);
        straightFenceBullet = new StraightFenceBullet(bulletPool, GameManager.Instance.allBounds);
        spreadBullet = new SpreadBullet(bulletPool, GameManager.Instance.playBounds);

        settingFilePath = Path.Combine(Application.streamingAssetsPath, "BulletSettings.json");
        LoadSettings();

        StartCoroutine(nameof(BulletFire));
    }

    private void LoadSettings()
    {
        if(File.Exists(settingFilePath))
        {
            var json = File.ReadAllText(settingFilePath);
            bulletSettings = JsonUtility.FromJson<BulletSettings>(json);

            straightBulletCount = bulletSettings.normal.straightBulletCount;
            straightBulletTerm = bulletSettings.normal.straightBulletTerm;
            straightFenceBulletTerm = bulletSettings.normal.straightFenceBulletTerm;
        }
    }

    private IEnumerator BulletFire()
    {
        int count = 10;
        while(true)
        {
            yield return fireWaitSeconds;

            if (count % straightBulletTerm == 0)
            {
                for(int i = 0; i < straightBulletCount; i++)
                {
                    straightBullet.Fire();
                }
            }

            if (count % straightFenceBulletTerm == 0)
            {
                straightFenceBullet.Fire();
            }

            count++;
        }
    }

    private void ChangeHardMode()
    {
        straightBulletCount = bulletSettings.hard.straightBulletCount;
        straightBulletTerm = bulletSettings.hard.straightBulletTerm;
        straightFenceBulletTerm = bulletSettings.hard.straightFenceBulletTerm;

        spreadBullet.Fire();
    }
}
