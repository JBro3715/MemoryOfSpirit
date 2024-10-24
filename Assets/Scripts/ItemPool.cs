using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject wispPrefab;

    private Queue<HealthPotion> healthPotionPool = new();
    private Queue<Wisp> wispPool = new();

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

    public Wisp GetWisp()
    {
        if(wispPool.Count > 0)
        {
            var wisp = wispPool.Dequeue();
            wisp.gameObject.SetActive(true);

            return wisp;
        }
        else
        {
            var wisp = Instantiate(wispPrefab, itemParent).GetComponent<Wisp>();

            wisp.returnWispCommand.Subscribe(_ =>
            {
                ReturnWisp(wisp);
            });

            return wisp;
        }
    }

    private void ReturnHealthPotion(HealthPotion healthPotion)
    {
        healthPotion.gameObject.SetActive(false);
        healthPotionPool.Enqueue(healthPotion);
    }

    private void ReturnWisp(Wisp wisp)
    {
        wisp.gameObject.SetActive(false);
        wispPool.Enqueue(wisp);
    }
}
