using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poof : MonoBehaviour
{
    public void AnimationDone()
    {
        Destroy(this.gameObject);
    }
}
