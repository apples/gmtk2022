using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(SyncGridPosition))]
public class TurnTaker : MonoBehaviour
{
    public TurnTakerList characterControllerList;
    public CharacterBehaviorData characterBehaviorData;
    public GameObject enemySprite;

    public GameObjectReference playerReference;

    [SerializeField]
    private TileGridReference tileGridReference;

    private GridPosition gridPosition;
    private SyncGridPosition syncGridPosition;
    private ICharacterBehavior behavior;

    public TileGrid TileGrid => tileGridReference.Current;
    public Vector2Int Position
    {
        get => gridPosition.Position;
        set => gridPosition.Position = value;
    }
    public bool IsMoving => !syncGridPosition.Done;

    void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
        syncGridPosition = GetComponent<SyncGridPosition>();
        behavior = characterBehaviorData.Create(this);
    }

    void Start()
    {
        characterControllerList.Add(this);
    }

    void OnDestroy()
    {
        characterControllerList.Remove(this);
    }

    public void BeginTurn() => behavior.BeginTurn();

    public TurnResult PerformTurn()
    {
        if (behavior is GenericEnemyCharacterBehavior geb)
        {
            if (playerReference == null || playerReference.Current == null)
            {
                return behavior.PerformTurn();
            }

            var playerCoord = playerReference.Current.GetComponent<GridPosition>().Position;
            var myCoord = gridPosition.Position;

            var dist = Vector2Int.Distance(playerCoord, myCoord);

            if (dist > geb.Data.playerDetectionRange)
            {
                return TurnResult.EndTurn;
            }
        }

        return behavior.PerformTurn();
    }

    public void AttackAnimationDone(EnemyAttack attack)
    {
        if (behavior is GenericEnemyCharacterBehavior geb)
        {
            geb.SetAttackAnimationDone();
        }
    }
}
