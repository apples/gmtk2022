using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnTakerList", menuName = "GMTKJAM/Runtime/TurnTaker List")]
public class TurnTakerList : ScriptableObject
{
    private List<TurnTaker> list = new List<TurnTaker>();
    public IReadOnlyList<TurnTaker> List => list;

    public event Action<TurnTaker> onAdd;
    public event Action<TurnTaker> onRemove;

    public void Add(TurnTaker characterController)
    {
        list.Add(characterController);
        if (onAdd != null) onAdd(characterController);
    }

    public void Remove(TurnTaker characterController)
    {
        list.Remove(characterController);
        if (onRemove != null) onRemove(characterController);
    }
}
