using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    public Sprite heartFullSprite;
    public Sprite heartEmptySprite;

    public GameObject heartPrefab;

    public float heartSpacing;
    public Transform heartOrigin;

    private Health health;

    private List<SpriteRenderer> hearts = new List<SpriteRenderer>(5);

    void Awake()
    {
        health = GetComponent<Health>();
    }

    void Start()
    {
        for (int i = 0; i < health.maxHealth; ++i)
        {
            var heartObj = Instantiate(heartPrefab, heartOrigin);
            heartObj.transform.localPosition = new Vector3(-heartSpacing * (health.maxHealth - 1) / 2f + heartSpacing * i, 0f, 0f);

            var spriteRenderer = heartObj.GetComponent<SpriteRenderer>();
            hearts.Add(spriteRenderer);
        }

        Refresh();
    }

    public void Refresh()
    {
        for (var i = 0; i < hearts.Count; ++i)
        {
            hearts[i].transform.localPosition = new Vector3(-heartSpacing * (health.maxHealth - 1) / 2f + heartSpacing * i, 0f, 0f);
            hearts[i].sprite = health.CurrentHealth > i ? heartFullSprite : heartEmptySprite;
        }
    }
}
