using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
    public TurnTakerList characterControllerList;

    private List<TurnSlot> turnSlots = new List<TurnSlot>();

    void OnEnable()
    {
        turnSlots.AddRange(characterControllerList.List.Select(x => new TurnSlot { characterController = x }));
        characterControllerList.onAdd += this.characterControllerList_onAdd;
        characterControllerList.onRemove += this.characterControllerList_onRemove;
    }

    void OnDisable()
    {
        characterControllerList.onAdd -= this.characterControllerList_onAdd;
        characterControllerList.onRemove -= this.characterControllerList_onRemove;
        turnSlots.Clear();
    }

    void Update()
    {
        if (turnSlots.Count > 0)
        {
            var next = turnSlots[0];

            if (!next.hasBegun)
            {
                next.characterController.BeginTurn();
                next.hasBegun = true;
            }

            var result = next.characterController.PerformTurn();

            switch (result)
            {
                case TurnResult.Wait:
                    break;
                case TurnResult.EndTurn:
                    next.hasBegun = false;
                    turnSlots.RemoveAt(0);
                    turnSlots.Add(next);
                    break;
                default:
                    throw new NotImplementedException($"Invalid result: {result}");
            }
        }
    }

    private void characterControllerList_onAdd(TurnTaker characterController)
    {
        var slot = new TurnSlot { characterController = characterController };
        turnSlots.Add(slot);
    }

    private void characterControllerList_onRemove(TurnTaker characterController)
    {
        turnSlots.RemoveAt(turnSlots.FindIndex(x => x.characterController == characterController));
    }

    private class TurnSlot
    {
        public TurnTaker characterController;
        public bool hasBegun;
    }
}
