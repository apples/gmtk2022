using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenericEnemyCharacterBehavior : CharacterBehavior
{
    private enum Status
    {
        Start,
        Moving,
    }

    public GenericEnemyCharacterBehaviorData Data { get; set; }

    private Status status;
    private int turnSequenceIndex = -1;

    public override void BeginTurn()
    {
        ++turnSequenceIndex;
        if (turnSequenceIndex >= Data.turnSequence.Count)
        {
            turnSequenceIndex = 0;
        }

        status = Status.Start;
    }

    public override TurnResult PerformTurn()
    {
        switch (status)
        {
            case Status.Start:
                return Start();
            case Status.Moving:
                return Moving();
            default:
                throw new InvalidOperationException($"Unknown status: {status}");
        }
    }

    private TurnResult Start()
    {
        return Data.turnSequence[turnSequenceIndex] switch
        {
            TurnStrategy.Idle => Start_Idle(),
            TurnStrategy.Wander => Start_Wander(),
            _ => throw new NotImplementedException(),
        };
    }

    private TurnResult Start_Idle()
    {
        return TurnResult.EndTurn;
    }

    private TurnResult Start_Wander()
    {
        Span<Vector2Int> possibleDirs = stackalloc Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        var e = 4;

        // remove_if
        for (var i = 0; i != e;)
        {
            var dir = possibleDirs[i];
            if (Controller.TileGrid.IsTileEmpty(Controller.Position + dir))
            {
                ++i;
            }
            else
            {
                possibleDirs[i] = possibleDirs[e - 1];
                --e;
            }
        }

        // no possible dirs
        if (e == 0)
        {
            return TurnResult.EndTurn;
        }

        // move

        Controller.Position += possibleDirs[Random.Range(0, e)];
        status = Status.Moving;

        return TurnResult.Wait;
    }

    private TurnResult Moving()
    {
        if (Controller.IsMoving)
        {
            return TurnResult.Wait;
        }

        return TurnResult.EndTurn;
    }
}
