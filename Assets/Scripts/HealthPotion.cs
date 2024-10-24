using UnityEngine;
using UniRx;

public class HealthPotion : MonoBehaviour
{
    public ReactiveCommand returnHealthPotionCommand = new();

    [HideInInspector] public int heal = 50;

    private float lifeTime = DEFAULT_LIFE_TIME;

    private const float DEFAULT_LIFE_TIME = 15f;

    private void Update()
    {
        if(lifeTime <= 0)
        {
            DestroyPotion();
            lifeTime = DEFAULT_LIFE_TIME;
        }

        lifeTime -= Time.deltaTime;
    }

    public void DestroyPotion()
    {
        returnHealthPotionCommand.Execute();
    }
}
