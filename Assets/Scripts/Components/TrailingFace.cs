using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingFace : MonoBehaviour
{
    public DieData dieData;

    [SerializeField]
    private int faceIndex;

    [SerializeField]
    private MeshRenderer meshRenderer;

    public Vector2Int Position { get; set; }
    public int FaceIndex
    {
        get => faceIndex;
        set
        {
            faceIndex = value;
            meshRenderer.material.mainTexture = dieData.faces[faceIndex].effect switch
            {
                DieFaceEffect.Attack => dieData.attackTrailingTexture,
                DieFaceEffect.Shield => dieData.defendTrailingTexture,
                DieFaceEffect.Miss => dieData.missTrailingTexture,
                _ => throw new NotImplementedException(),
            };
        }
    }

    public DieFaceEffect FaceEffect => dieData.faces[FaceIndex].effect;
}
