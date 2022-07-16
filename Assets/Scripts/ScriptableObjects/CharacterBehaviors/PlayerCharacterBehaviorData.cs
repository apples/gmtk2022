using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "GMTKJAM/Character Behaviors/Player")]
public class PlayerCharacterBehaviorData : CharacterBehaviorData
{
    public override ICharacterBehavior Create(TurnTaker characterController) => characterController.GetComponent<PlayerController>();
}
