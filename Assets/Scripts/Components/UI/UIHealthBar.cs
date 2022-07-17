using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public GameObjectReference healthHaver;

    public Sprite heartFullSprite;
    public Sprite heartEmptySprite;

    public List<Image> heartImages;

    private Health health;

    void OnEnable()
    {
        healthHaver.onChange += this.healthHaver_onChange;
        if (healthHaver.Current != null)
        {
            health = healthHaver.Current.GetComponent<Health>();
            health.onChange.AddListener(this.health_onChange);
        }
        Refresh();
    }

    void OnDisable()
    {
        healthHaver.onChange -= this.healthHaver_onChange;
        if (health != null)
        {
            health.onChange.RemoveListener(this.health_onChange);
        }
    }

    private void healthHaver_onChange(GameObject obj)
    {
        if (health != null)
        {
            health.onChange.RemoveListener(this.health_onChange);
        }
        if (obj == null)
        {
            health = null;
        }
        else
        {
            health = obj.GetComponent<Health>();
            health.onChange.AddListener(this.health_onChange);
        }
        Refresh();
    }

    private void health_onChange(Health health)
    {
        Refresh();
    }

    void Refresh()
    {
        if (health == null)
        {
            foreach (var img in heartImages)
            {
                img.gameObject.SetActive(false);
            }
            return;
        }

        for (var i = 0; i < heartImages.Count; ++i)
        {
            if (i < health.CurrentHealth)
            {
                heartImages[i].sprite = heartFullSprite;
                heartImages[i].gameObject.SetActive(true);
            }
            else if (i < health.maxHealth)
            {
                heartImages[i].sprite = heartEmptySprite;
                heartImages[i].gameObject.SetActive(true);
            }
            else
            {
                heartImages[i].gameObject.SetActive(false);
            }
        }
    }
}
