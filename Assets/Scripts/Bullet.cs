using UnityEngine;
using UniRx;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public int damage;
    
    public ReactiveCommand returnBulletCommand = new ();

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
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
