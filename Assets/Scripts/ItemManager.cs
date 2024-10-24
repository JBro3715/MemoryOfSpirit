using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private readonly WaitForSeconds waitSeconds = new(1f);

    private ItemPool itemPool;
    private Bounds bounds;
    private Vector2 spawnPosition;

    private const float BOUNDS_SCALE = 0.95f;

    private const int HEAL_POTION_SPAWN_TIME = 30;
    private const int HEAL_POTION_SPAWN_COUNT = 2;

    private const int WISP_SPAWN_TIME = 10;
    private const int WISP_SPAWN_COUNT = 1;

    private void Start()
    {
        itemPool = GetComponent<ItemPool>();
        bounds = GameManager.Instance.playBounds;

        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        int count = 1;
        while(true)
        {
            if(count % HEAL_POTION_SPAWN_TIME == 0)
            {
                for (int i = 0; i < HEAL_POTION_SPAWN_COUNT; i++)
                {
                    var healthPotion = itemPool.GetHealthPotion();
                    SetPosition(healthPotion.transform);
                }
            }

            if(count % WISP_SPAWN_TIME == 0)
            {
                for(int i = 0; i < WISP_SPAWN_COUNT; i++)
                {
                    var wisp = itemPool.GetWisp();
                    SetPosition(wisp.transform);
                }
            }

            count++;
            yield return waitSeconds;
        }
    }

    private void SetPosition(Transform item)
    {
        var extents = bounds.extents * BOUNDS_SCALE;
        spawnPosition.x = Random.Range(0, extents.x) * GetRandomSign();
        spawnPosition.y = Random.Range(0, extents.y) * GetRandomSign();

        item.position = spawnPosition;
    }

    private int GetRandomSign()
    {
        return Random.value < 0.5f ? -1 : 1;
    }
}
