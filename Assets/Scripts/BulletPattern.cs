using UnityEngine;
using System.Threading.Tasks;

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
        bulletSpeed = Random.Range(1f, 3f);
        damage = 40;
    }

    public abstract void Fire();

    protected Bullet SetNormalBullet()
    {
        var bullet = bulletPool.GetNormalBullet();

        bullet.transform.position = startPosition;
        bullet.speed = bulletSpeed * (int)GameManager.Instance.level.Value;
        bullet.direction = direction;
        bullet.damage = damage * (int)GameManager.Instance.level.Value;
        bullet.MovingBullet();

        return bullet;
    }

    protected Bullet SetSpecialBullet()
    {
        var bullet = bulletPool.GetSpecialBullet();

        bullet.transform.position = startPosition;
        bullet.speed = bulletSpeed * (int)GameManager.Instance.level.Value;
        bullet.direction = direction;
        bullet.damage = damage * (int)GameManager.Instance.level.Value;
        bullet.MovingBullet();

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

        SetNormalBullet();
    }

    private void SetStartPosition()
    {
        var extents = bulletBounds.extents;
        startPosition.x = Random.Range(0, extents.x) * GetRandomSign();
        startPosition.y = Random.Range(0, extents.y) * GetRandomSign();
    }

    private void SetDirection()
    {
        float min;
        float max;

        if (startPosition.x < 0 && startPosition.y > 0) // 좌 상단
        {
            min = 0f;
            max = 90f;
        }
        else if (startPosition.x > 0 && startPosition.y > 0) // 우 상단
        {
            min = 180f;
            max = 270f;
        }
        else if (startPosition.x < 0 && startPosition.y < 0) // 좌 하단
        {
            min = 90f;
            max = 180f;
        }
        else if (startPosition.x > 0 && startPosition.y < 0) // 우 하단
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

            SetNormalBullet();
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

            SetNormalBullet();
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

            SetNormalBullet();
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

            SetNormalBullet();
        }
    }
}

public class SpreadBullet : BulletPattern
{
    private GameObject meteor;
    private const int SHOOT_COUNT = 5;
    private Vector2 velocity = Vector2.zero;

    public SpreadBullet(BulletPool bulletPool, Bounds bounds) : base(bulletPool, bounds)
    {
        meteor = BulletManager.Instance.meteor;
        meteor.transform.position = bulletBounds.min;
        startPosition = Random.insideUnitCircle * bulletBounds.extents.y * 0.5f;
        bulletSpeed = Random.Range(2f, 3f);
    }

    public async override void Fire()
    {
        meteor.SetActive(true);
        while(Vector2.Distance(meteor.transform.position, startPosition) > 0.05f)
        {
            await Task.Delay(10);
            meteor.transform.position = Vector2.SmoothDamp(meteor.transform.position, startPosition, ref velocity, 0.15f);
        }
        meteor.transform.position = startPosition;

        await SetBullets(SHOOT_COUNT);
        meteor.SetActive(false);
    }

    private async Task SetBullets(int count)
    {
        count--;
        int bulletCount = 24;

        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for(int i = 0; i < bulletCount; i++)
        {
            direction.x = Mathf.Cos(angle * Mathf.Deg2Rad);
            direction.y = Mathf.Sin(angle * Mathf.Deg2Rad);
            direction.Normalize();

            SetSpecialBullet();
            
            angle += angleStep;
        }

        if(count > 0)
        {
            await Task.Delay(500);
            await SetBullets(count);
        }
    }
}