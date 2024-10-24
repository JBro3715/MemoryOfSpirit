using UnityEngine;
using UniRx;

public class Wisp : MonoBehaviour
{
    public ReactiveCommand returnWispCommand = new();

    [HideInInspector] public int point = 20;


    private float lifeTime = DEFAULT_LIFE_TIME;
    private float delay = DEFAULT_DELAY;

    private const float DEFAULT_LIFE_TIME = 15f;
    private const float DEFAULT_DELAY = 0.15f;

    private void Update()
    {
        if (lifeTime <= 0)
        {
            DestroyWisp();
            lifeTime = DEFAULT_LIFE_TIME;
        }

        lifeTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        delay -= Time.deltaTime;

        if (delay <= 0)
        {
            transform.Translate((Vector3)Random.insideUnitCircle * 0.2f);
            delay = DEFAULT_DELAY;
        }
    }

    public void DestroyWisp()
    {
        ResetWisp();
        returnWispCommand.Execute();
    }

    private void ResetWisp()
    {
        lifeTime = DEFAULT_LIFE_TIME;
        delay = DEFAULT_DELAY;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(GameManager.Tag.Wall.ToString()))
        {
            DestroyWisp();
        }
    }
}
