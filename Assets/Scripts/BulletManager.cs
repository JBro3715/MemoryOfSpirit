using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject background;

    private BulletPool bulletPool;

    private StraightBullet straightBullet;
    private StraightFenceBullet straightFenceBullet;


    private const int STRAIGHT_BULLET_COUNT = 20;
    private const int STRAIGHT_BULLET_TERM = 10;
    private const int STRAIGHT_FENCE_BULLET_TERM = 25;

    private void Start()
    {
        var bounds = background.GetComponent<Collider2D>().bounds;
        bulletPool = GetComponent<BulletPool>();

        straightBullet = new StraightBullet(bulletPool, bounds);
        straightFenceBullet = new StraightFenceBullet(bulletPool, bounds);

        StartCoroutine(nameof(BulletFire));
    }

    IEnumerator BulletFire()
    {
        int count = 10;
        while(true)
        {
            yield return new WaitForSeconds(1f);

            if (count % STRAIGHT_BULLET_TERM == 0)
            {
                for(int i = 0; i < STRAIGHT_BULLET_COUNT; i++)
                {
                    straightBullet.Fire();
                }
            }

            if (count % STRAIGHT_FENCE_BULLET_TERM == 0)
            {
                straightFenceBullet.Fire();
            }

            count++;
        }
    }
}
