using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private ItemPool itemPool;
    private Bounds bounds;
    private Vector2 spawnPosition;

    private const int MAX_HEALPOTION_COUNT = 2;

    private void Start()
    {
        itemPool = GetComponent<ItemPool>();
        bounds = GameManager.Instance.bounds;

        StartCoroutine(SpawnHealPotion());
    }

    private IEnumerator SpawnHealPotion()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);

            for(int i = 0; i < MAX_HEALPOTION_COUNT; i++)
            {
                var healthPotion = itemPool.GetHealthPotion();
                SetPosition(healthPotion.transform);
            }
        }
    }

    private void SetPosition(Transform item)
    {
        var extents = bounds.extents;
        spawnPosition.x = Random.Range(0, extents.x) * GetRandomSign();
        spawnPosition.y = Random.Range(0, extents.y) * GetRandomSign();

        item.position = spawnPosition;
    }

    private int GetRandomSign()
    {
        return Random.value < 0.5f ? -1 : 1;
    }
}
