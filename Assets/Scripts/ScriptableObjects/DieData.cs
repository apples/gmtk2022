using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieData", menuName = "GMTKJAM/Die Data")]
public class DieData : ScriptableObject
{
    public Texture attackTexture;
    public Texture defendTexture;
    public Texture missTexture;
    public Texture attackTrailingTexture;
    public Texture defendTrailingTexture;
    public Texture missTrailingTexture;
    public DieChargeMode chargeMode;
    public List<Face> faces;

    [System.Serializable]
    public class Face
    {
        public DieFaceEffect effect;
    }
}
