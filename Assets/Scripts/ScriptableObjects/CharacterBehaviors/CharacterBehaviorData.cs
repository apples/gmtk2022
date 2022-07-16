using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviorData : ScriptableObject
{
    public abstract ICharacterBehavior Create(TurnTaker characterController);
}
