using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiePivot : MonoBehaviour
{
    public UnityEvent<DiePivot> onAnimationDone;

    public void AnimationDone()
    {
        onAnimationDone.Invoke(this);
    }
}
