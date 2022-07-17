using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncGameObjectReference : MonoBehaviour
{
    public GameObjectReference gameObjectReference;

    void OnEnable()
    {
        Debug.Assert(gameObjectReference.Current == null);
        gameObjectReference.Current = this.gameObject;
    }

    void OnDisable()
    {
        Debug.Assert(gameObjectReference.Current == this.gameObject);
        gameObjectReference.Current = null;
    }
}
