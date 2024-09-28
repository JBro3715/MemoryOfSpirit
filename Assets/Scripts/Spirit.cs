using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private Color[] basicColors = {Color.grey, Color.yellow, Color.magenta, Color.cyan, Color.red, Color.blue, Color.green };
    private float moveSpeed = 3f;
    private int HP = 100;

    // Start is called before the first frame update
    void Start()
    {
        var randomIndex = Random.Range(0, basicColors.Length);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = basicColors[randomIndex];
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;

        transform.Translate(move);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();

            HP -= bullet.damage;
            bullet.DestroyBullet();

            if (HP <= 0)
            {
                Debug.Log("GameOver!");
            }
        }

        if (collision.transform.tag == "Wall")
        {
            HP -= 100;
            Debug.Log("GameOver!");
        }
    }
}
