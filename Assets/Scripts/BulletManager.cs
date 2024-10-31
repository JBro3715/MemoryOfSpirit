using System;
using System.IO;
using System.Collections;
using UnityEngine;

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

    private BulletSettings bulletSettings;
    private BulletPool bulletPool;

    private StraightBullet straightBullet;
    private StraightFenceBullet straightFenceBullet;

    private readonly WaitForSeconds fireWaitSeconds = new(1f);

    private string settingFilePath;
    private int straightBulletCount;
    private int straightBulletTerm;
    private int straightFenceBulletTerm;

    private void Start()
    {
        var bounds = GameManager.Instance.allBounds;
        bulletPool = GetComponent<BulletPool>();

        straightBullet = new StraightBullet(bulletPool, bounds);
        straightFenceBullet = new StraightFenceBullet(bulletPool, bounds);

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

    public void ChangeHardMode()
    {
        straightBulletCount = bulletSettings.hard.straightBulletCount;
        straightBulletTerm = bulletSettings.hard.straightBulletTerm;
        straightFenceBulletTerm = bulletSettings.hard.straightFenceBulletTerm;
    }
}
