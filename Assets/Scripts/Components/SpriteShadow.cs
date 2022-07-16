using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteShadow : MonoBehaviour
{
    public UnityEngine.Rendering.ShadowCastingMode mode;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        spriteRenderer.shadowCastingMode = mode;
    }

    void OnDisable()
    {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    void OnValidate()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.shadowCastingMode = mode;
        }
    }
}
