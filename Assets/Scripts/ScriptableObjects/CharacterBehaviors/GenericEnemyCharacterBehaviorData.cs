using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericEnemy", menuName = "GMTKJAM/Character Behaviors/Generic Enemy")]
public class GenericEnemyCharacterBehaviorData : CharacterBehaviorData
{
    public float playerDetectionRange;
    public GameObjectReference playerReference;
    public List<TurnStrategy> turnSequence;

    public override ICharacterBehavior Create(TurnTaker characterController) => new GenericEnemyCharacterBehavior { Controller = characterController, Data = this };
}
