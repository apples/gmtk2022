using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenericEnemyCharacterBehavior : CharacterBehavior
{
    private enum Status
    {
        Start,
        Moving,
        Attacking,
    }

    public GenericEnemyCharacterBehaviorData Data { get; set; }

    private Status status;
    private int turnSequenceIndex = -1;

    private GameObject attackTarget;
    private bool isAttackAnimationDone;

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
            case Status.Attacking:
                return Attacking();
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
            if (Controller.TileGrid.IsTileEmpty(Controller.Position + dir, out var occupant) ||
                (occupant != null && occupant.GetComponent<HitTarget>().type == HitTarget.Type.Player))
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

        {
            var dir = possibleDirs[Random.Range(0, e)];

            var occupant = Controller.TileGrid.GetOccupant(Controller.Position + dir);
            
            if (occupant != null)
            {
                Debug.Assert(occupant.GetComponent<HitTarget>().type == HitTarget.Type.Player);
                attackTarget = occupant.gameObject;
                isAttackAnimationDone = false;

                var enemyAttack = Controller.GetComponentInChildren<EnemyAttack>();
                enemyAttack.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y), Vector3.up);
                Controller.enemySprite.transform.SetParent(enemyAttack.transform, true);
                var animator = enemyAttack.GetComponent<Animator>();
                animator.Play("Base Layer.EnemyAttack", -1, 0f);

                status = Status.Attacking;
            }
            else
            {
                Controller.Position += dir;
                status = Status.Moving;
            }
        }

        return TurnResult.Wait;
    }

    public void SetAttackAnimationDone()
    {
        isAttackAnimationDone = true;
    }

    private TurnResult Moving()
    {
        if (Controller.IsMoving)
        {
            return TurnResult.Wait;
        }

        return TurnResult.EndTurn;
    }

    private TurnResult Attacking()
    {
        if (!isAttackAnimationDone)
        {
            return TurnResult.Wait;
        }

        Controller.enemySprite.transform.SetParent(Controller.transform, true);

        if (attackTarget.GetComponent<PlayerController>() is PlayerController pc)
        {
            if (!pc.ConsumeShield())
            {
                attackTarget.GetComponent<Health>().CurrentHealth -= 1;
                Controller.hitSound.Play();
            }
            else
            {
                Controller.bounceSound.Play();
            }
        }
        else
        {
            attackTarget.GetComponent<Health>().CurrentHealth -= 1;
            Controller.hitSound.Play();
        }

        return TurnResult.EndTurn;
    }
}
