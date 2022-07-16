using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat", menuName = "GMTKJAM/Character Behaviors/Bat")]
public class BatCharacterBehaviorData : CharacterBehaviorData
{
    public override ICharacterBehavior Create(TurnTaker characterController) => new BatCharacterBehavior { Controller = characterController, Data = this };
}
