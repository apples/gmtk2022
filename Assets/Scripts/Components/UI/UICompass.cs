using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompass : MonoBehaviour
{
    public GameObjectReference targetReference;
    public GameObjectReference playerReference;
    
    [SerializeField]
    private Image image;

    void Start()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (targetReference.Current == null || playerReference.Current == null)
        {
            return;
        }

        var dir = (targetReference.Current.transform.position - playerReference.Current.transform.position).normalized;

        var angle = Vector3.SignedAngle(Vector3.forward, dir, Vector3.up);

        image.rectTransform.localEulerAngles = new Vector3(0, 0, -angle);
    }
}
