using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject specialBulletPrefab;
    [SerializeField] private Transform bulletParent;

    private Queue<Bullet> normalBulletPool = new Queue<Bullet>();
    private Queue<Bullet> specialBulletPool = new Queue<Bullet>();

    public Bullet GetNormalBullet()
    {
        if(normalBulletPool.Count > 0)
        {
            var bullet = normalBulletPool.Dequeue();
            bullet.gameObject.SetActive(true);

            return bullet;
        }
        else
        {
            var bullet = Instantiate(normalBulletPrefab, bulletParent).GetComponent<Bullet>();
            
            bullet.returnBulletCommand.Subscribe(_ =>
            {
                ReturnNormalBullet(bullet);
            });

            return bullet;
        }
    }

    public void ReturnNormalBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        normalBulletPool.Enqueue(bullet);
    }

    public Bullet GetSpecialBullet()
    {
        if (specialBulletPool.Count > 0)
        {
            var bullet = specialBulletPool.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            return Instantiate(specialBulletPrefab, bulletParent).GetComponent<Bullet>();
        }
    }

    public void ReturnSpecialBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        specialBulletPool.Enqueue(bullet);
    }
}
