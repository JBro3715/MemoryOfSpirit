using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class BulletPattern
{
    readonly protected BulletPool bulletPool;

    readonly protected Bounds bulletBounds;
    protected Vector2 startPosition;
    protected Vector2 direction;
    protected float bulletSpeed;
    protected int damage;

    public BulletPattern(BulletPool bulletPool, Bounds bounds)
    {
        this.bulletPool = bulletPool;
        bulletBounds = bounds;

        startPosition = Vector2.zero;
        direction = Vector2.zero;
        bulletSpeed = 1f;
        damage = 40;
    }

    public abstract void Fire();

    protected Bullet SetBullet()
    {
        var bullet = bulletPool.GetNormalBullet();

        bullet.transform.position = startPosition;
        bullet.speed = bulletSpeed;
        bullet.direction = direction;
        bullet.damage = damage;

        return bullet;
    }
}

public class StraightBullet : BulletPattern
{
    public StraightBullet(BulletPool bulletPool, Bounds bounds) : base(bulletPool, bounds)
    {
    }

    public override void Fire()
    {
        SetStartPosition();
        SetDirection();

        SetBullet();
    }

    private void SetStartPosition()
    {
        var extents = bulletBounds.extents;
        float startX = Random.Range(0, extents.x) * GetRandomSign();
        float startY = Random.Range(0, extents.y) * GetRandomSign();

        startPosition.x = startX;
        startPosition.y = startY;
    }

    private void SetDirection()
    {
        float min;
        float max;

        if (startPosition.x < 0 && startPosition.y > 0) // ÁÂ »ó´Ü
        {
            min = 0f;
            max = 90f;
        }
        else if (startPosition.x > 0 && startPosition.y > 0) // ¿ì »ó´Ü
        {
            min = 180f;
            max = 270f;
        }
        else if (startPosition.x < 0 && startPosition.y < 0) // ÁÂ ÇÏ´Ü
        {
            min = 90f;
            max = 180f;
        }
        else if (startPosition.x > 0 && startPosition.y < 0) // ¿ì ÇÏ´Ü
        {
            min = 270f;
            max = 360f;
        }
        else
        {
            min = 0f;
            max = 360f;
        }

        float randomAngle = Random.Range(min, max);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        direction.x = Mathf.Cos(angleInRadians);
        direction.y = Mathf.Sin(angleInRadians);
    }

    private int GetRandomSign()
    {
        return Random.value < 0.5f ? -1 : 1;
    }
}

public class StraightFenceBullet : BulletPattern
{
    readonly private int interval;
    private float startX;
    private float startY;

    public StraightFenceBullet(BulletPool bulletPool, Bounds bounds) : base(bulletPool, bounds)
    {
        interval = 2;
    }

    public override void Fire()
    {
        SetTopFenceBullets();
        SetBottomFenceBullets();
        SetLeftFenceBullets();
        SetRightFenceBullets();
    }

    private void SetTopFenceBullets()
    {
        startX = -bulletBounds.extents.x;
        startPosition.y = bulletBounds.extents.y;
        direction = Vector2.down;
        bulletSpeed = Random.Range(1f, 3f);

        for (float i = startX + interval; i < -startX; i += interval)
        {
            startPosition.x = i;

            SetBullet();
        }
    }

    private void SetBottomFenceBullets()
    {
        startX = -bulletBounds.extents.x;
        startPosition.y = -bulletBounds.extents.y;
        direction = Vector2.up;
        bulletSpeed = Random.Range(1f, 3f);

        for (float i = startX + interval * 0.5f; i < -startX; i += interval)
        {
            startPosition.x = i;

            SetBullet();
        }
    }

    private void SetLeftFenceBullets()
    {
        startY = -bulletBounds.extents.y;
        startPosition.x = -bulletBounds.extents.x;
        direction = Vector2.right;
        bulletSpeed = Random.Range(1f, 3f);

        for (float i = startY + interval; i < -startY; i += interval)
        {
            startPosition.y = i;

            SetBullet();
        }
    }

    private void SetRightFenceBullets()
    {
        startY = -bulletBounds.extents.y;
        startPosition.x = bulletBounds.extents.x;
        direction = Vector2.left;
        bulletSpeed = Random.Range(1f, 3f);

        for (float i = startY + interval * 0.5f; i < -startY; i += interval)
        {
            startPosition.y = i;

            SetBullet();
        }
    }
}

public class SpreadBullet : BulletPattern
{
    public SpreadBullet(BulletPool bulletPool, Bounds bounds) : base(bulletPool, bounds)
    {
    }

    public override void Fire()
    {
        Debug.Log("È®»ê Åº¸·");
    }

}