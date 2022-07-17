using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BatCharacterBehavior : CharacterBehavior
{
    private enum Status
    {
        Start,
        Moving,
    }

    public BatCharacterBehaviorData Data { get; set; }

    private Status status;

    public override void BeginTurn()
    {
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
        Span<Vector2Int> possibleDirs = stackalloc Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        var dir = possibleDirs[Random.Range(0, 4)];

        if (Controller.TileGrid.IsTileEmpty(Controller.Position + dir, out var occupant))
        {
            Controller.Position += dir;
            status = Status.Moving;
            return TurnResult.Wait;
        }

        return TurnResult.EndTurn;
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
