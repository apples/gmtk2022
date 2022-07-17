using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    public UnityEvent<EnemyAttack> onDone;

    public void AnimationDone()
    {
        onDone.Invoke(this);
    }
}
