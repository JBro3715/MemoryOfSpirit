using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject healthPotionPrefab;

    private Queue<HealthPotion> healthPotionPool = new Queue<HealthPotion>();

    public HealthPotion GetHealthPotion()
    {
        if(healthPotionPool.Count > 0)
        {
            var healthPotion = healthPotionPool.Dequeue();
            healthPotion.gameObject.SetActive(true);

            return healthPotion;
        }
        else
        {
            var healthPotion = Instantiate(healthPotionPrefab, itemParent).GetComponent<HealthPotion>();

            healthPotion.returnHealthPotionCommand.Subscribe(_ =>
            {
                ReturnHealthPotion(healthPotion);
            });

            return healthPotion;
        }
    }

    public void ReturnHealthPotion(HealthPotion healthPotion)
    {
        healthPotion.gameObject.SetActive(false);
        healthPotionPool.Enqueue(healthPotion);
    }
}
