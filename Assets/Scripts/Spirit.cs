using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spirit : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;

    readonly private Color[] basicColors = {Color.grey, Color.yellow, Color.magenta, Color.cyan, Color.red, Color.blue, Color.green };
    private Vector2 direction;
    private Vector3 offset;
    private float moveSpeed = 3f;
    private int HP = DEFAULT_HP;
    private int maxHP = DEFAULT_HP;

    private const int DEFAULT_HP = 100;
    private const int LIMIT_HP = 250;


    private void Start()
    {
        if(hpBar == null)
        {
            Debug.LogException(new System.Exception("HP Bar 이미지가 연결되어 있지 않습니다."));
        }

        var randomIndex = Random.Range(0, basicColors.Length);
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = basicColors[randomIndex];

        offset.x = 0.05f;
        offset.y = 0.5f;

        UpdateHP(0);
    }

    private void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        Vector2 move = moveSpeed * Time.deltaTime * direction;

        transform.Translate(move);
    }

    private void LateUpdate()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position + offset);
        hpBar.transform.position = screenPosition;
    }

    private void UpdateHP(int hp)
    {
        HP += hp;

        if(HP > maxHP)
        {
            maxHP = HP;
        }
        
        if(HP >= LIMIT_HP)
        {
            HP = LIMIT_HP;
            maxHP = LIMIT_HP;
        }

        hpBar.fillAmount = HP / (float)maxHP;
        hpText.text = $"{HP} / {maxHP}";

        if(HP <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.Tag.Bullet.ToString()))
        {
            var bullet = collision.GetComponent<Bullet>();

            UpdateHP(-bullet.damage);
            bullet.DestroyBullet();
        }

        if (collision.CompareTag(GameManager.Tag.Wall.ToString()))
        {
            UpdateHP(-LIMIT_HP);
        }

        if(collision.CompareTag(GameManager.Tag.HealthPotion.ToString()))
        {
            var healthPotion = collision.GetComponent<HealthPotion>();

            UpdateHP(healthPotion.heal);
            healthPotion.DestroyPotion();
        }
    }
}
