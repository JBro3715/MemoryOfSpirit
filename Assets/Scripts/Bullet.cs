using UnityEngine;
using UniRx;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public int damage;
    
    public ReactiveCommand returnBulletCommand = new ();

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void MovingBullet()
    {
        rigid.velocity = direction * speed;
        transform.right = direction;
    }

    public void DestroyBullet()
    {
        returnBulletCommand.Execute();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyArea"))
        {
            DestroyBullet();
        }
    }
}
