using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spirit : MonoBehaviour
{
    [SerializeField] private Image hpBar;

    readonly private Color[] basicColors = {Color.grey, Color.yellow, Color.magenta, Color.cyan, Color.red, Color.blue, Color.green };
    private Vector2 direction;
    private Vector3 offset;
    private float moveSpeed = 3f;
    private int HP = 100;
    private int maxHP;

    // Start is called before the first frame update
    void Start()
    {
        if(hpBar == null)
        {
            Debug.LogException(new System.Exception("HP Bar 이미지가 연결되어 있지 않습니다."));
        }

        var randomIndex = Random.Range(0, basicColors.Length);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = basicColors[randomIndex];
        maxHP = HP;

        offset.x = 0.05f;
        offset.y = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        Vector2 move = moveSpeed * Time.deltaTime * direction;

        transform.Translate(move);
    }

    private void LateUpdate()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position + offset);
        Debug.Log(screenPosition);
        hpBar.transform.position = screenPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();

            HP -= bullet.damage;
            hpBar.fillAmount = HP / (float)maxHP;
            bullet.DestroyBullet();

            if (HP <= 0)
            {
                Debug.Log("GameOver!");
            }
        }

        if (collision.transform.CompareTag("Wall"))
        {
            HP -= 100;
            hpBar.fillAmount = 0;
            Debug.Log("GameOver!");
        }
    }
}
