using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridPosition))]
public class PlayerController : MonoBehaviour, ICharacterBehavior
{
    public TileGridReference tileGridReference;

    public DieData dieData;

    [SerializeField]
    private List<MeshRenderer> faceMeshRenderers;

    [SerializeField]
    private GameObject trailingFacePrefab;

    private PlayerControls controls;

    private GridPosition gridPosition;
    private SyncGridPosition syncGridPosition;
    private DieAttackSwing dieAttackSwing;

    private Vector2Int? queuedMove;

    private enum Status
    {
        WaitingForInput,
        Moving,
        Attacking,
    }

    private Status status;

    private DieState dieState;

    private List<TrailingFace> trail = new List<TrailingFace>(6);
    private List<Vector2Int> trailMoves = new List<Vector2Int>(6);

    void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
        syncGridPosition = GetComponent<SyncGridPosition>();
        dieAttackSwing = GetComponent<DieAttackSwing>();

        controls = new PlayerControls();
        controls.PlayerDice.MoveUp.performed += this.controls_PlayerDice_MoveUp_performed;
        controls.PlayerDice.MoveDown.performed += this.controls_PlayerDice_MoveDown_performed;
        controls.PlayerDice.MoveLeft.performed += this.controls_PlayerDice_MoveLeft_performed;
        controls.PlayerDice.MoveRight.performed += this.controls_PlayerDice_MoveRight_performed;

        controls.PlayerDice.MoveUp.canceled += this.controls_PlayerDice_MoveUp_canceled;
        controls.PlayerDice.MoveDown.canceled += this.controls_PlayerDice_MoveDown_canceled;
        controls.PlayerDice.MoveLeft.canceled += this.controls_PlayerDice_MoveLeft_canceled;
        controls.PlayerDice.MoveRight.canceled += this.controls_PlayerDice_MoveRight_canceled;

        dieState = new DieState(dieData);

        for (var i = 0; i < 6; ++i)
        {
            switch (dieState.faces[i].effect)
            {
                case DieFaceEffect.Attack:
                    faceMeshRenderers[i].material.mainTexture = dieData.attackTexture;
                    break;
                case DieFaceEffect.Shield:
                    faceMeshRenderers[i].material.mainTexture = dieData.defendTexture;
                    break;
                case DieFaceEffect.Miss:
                    faceMeshRenderers[i].material.mainTexture = dieData.missTexture;
                    break;
            }
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
    }

    private void controls_PlayerDice_MoveLeft_performed(InputAction.CallbackContext obj) => Move(Vector2Int.left);
    private void controls_PlayerDice_MoveRight_performed(InputAction.CallbackContext obj) => Move(Vector2Int.right);
    private void controls_PlayerDice_MoveUp_performed(InputAction.CallbackContext obj) => Move(Vector2Int.up);
    private void controls_PlayerDice_MoveDown_performed(InputAction.CallbackContext obj) => Move(Vector2Int.down);

    private void controls_PlayerDice_MoveLeft_canceled(InputAction.CallbackContext obj) => CancelMove(Vector2Int.left);
    private void controls_PlayerDice_MoveRight_canceled(InputAction.CallbackContext obj) => CancelMove(Vector2Int.right);
    private void controls_PlayerDice_MoveUp_canceled(InputAction.CallbackContext obj) => CancelMove(Vector2Int.up);
    private void controls_PlayerDice_MoveDown_canceled(InputAction.CallbackContext obj) => CancelMove(Vector2Int.down);

    private void Move(Vector2Int dir)
    {
        queuedMove = dir;
    }

    private void CancelMove(Vector2Int dir)
    {
        if (queuedMove == dir)
        {
            queuedMove = null;
        }
    }

    public void BeginTurn()
    {
        status = Status.WaitingForInput;
    }

    public TurnResult PerformTurn()
    {
        switch (status)
        {
            case Status.WaitingForInput:
                return WaitingForInput();
            case Status.Moving:
                return Moving();
            case Status.Attacking:
                return Attacking();
            default:
                throw new InvalidOperationException($"Unknown status {status}");
        }
    }

    private TurnResult WaitingForInput()
    {
        if (queuedMove is Vector2Int qm)
        {
            if (trail.Count != 0)
            {
                trailMoves.Add(qm);
            }
            queuedMove = null;

            var newPos = gridPosition.Position + qm;

            if (tileGridReference.Current.IsTileEmpty(newPos, out var occupant))
            {
                gridPosition.Position = newPos;
                status = Status.Moving;

                Dir? dir =
                    qm == Vector2Int.up ? Dir.Up :
                    qm == Vector2Int.right ? Dir.Right :
                    qm == Vector2Int.down ? Dir.Down :
                    qm == Vector2Int.left ? Dir.Left :
                    null;

                if (dir is Dir d)
                {
                    dieState.Spin(d);
                }
            }

            if (occupant != null && occupant.GetComponent<Health>() is Health occupantHealth)
            {
                if (ConsumeTrail(out var stats))
                {
                    if (stats.attack > 0)
                    {
                        occupantHealth.CurrentHealth -= stats.attack;
                    }
                }

                dieAttackSwing.Play(qm);

                status = Status.Attacking;
            }
        }

        return TurnResult.Wait;
    }

    private bool ConsumeTrail(out TrailStats trailStats)
    {
        trailStats = default;

        if (trail.Count == 0) return false;

        foreach (var trailingFace in trail)
        {
            switch (trailingFace.FaceEffect)
            {
                case DieFaceEffect.Attack:
                    ++trailStats.attack;
                    break;
                case DieFaceEffect.Shield:
                    ++trailStats.shields;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        DestroyTrail();

        return true;
    }

    private void DestroyTrail()
    {
        foreach (var trailingFace in trail)
        {
            Destroy(trailingFace.gameObject);
        }
        trail.Clear();
        trailMoves.Clear();
    }

    private TurnResult Moving()
    {
        if (syncGridPosition == null || syncGridPosition.Done)
        {
            UpdateTrail();
            return TurnResult.EndTurn;
        }

        return TurnResult.Wait;
    }

    private TurnResult Attacking()
    {
        if (dieAttackSwing == null || dieAttackSwing.Done)
        {
            return TurnResult.EndTurn;
        }

        return TurnResult.Wait;
    }

    private void UpdateTrail()
    {
        AddTrail();

        // validate

        var isValid = dieData.chargeMode switch
        {
            DieChargeMode.Linear => ValidateLinearTrail(),
            _ => throw new NotImplementedException(),
        };

        if (!isValid)
        {
            for (var i = 0; i < trail.Count - 1; ++i)
            {
                Destroy(trail[i].gameObject);
            }
            trail.RemoveRange(0, trail.Count - 1);
            trailMoves.Clear();
        }

        // miss

        if (trail[trail.Count - 1].FaceEffect == DieFaceEffect.Miss)
        {
            DestroyTrail();
        }
    }

    private void AddTrail()
    {
        var overlap = trail.Find(x => x.Position == gridPosition.Position);

        var previous = trail.FindIndex(x => x.FaceIndex == dieState.TopFaceIndex);

        if (overlap == null)
        {
            if (previous >= 0)
            {
                Destroy(trail[previous].gameObject);
                trail.RemoveAt(previous);
            }

            var trailingFaceObj = Instantiate(trailingFacePrefab);
            trailingFaceObj.transform.position = this.transform.position;

            var trailingFace = trailingFaceObj.GetComponent<TrailingFace>();

            trailingFace.dieData = dieData;
            trailingFace.FaceIndex = dieState.TopFaceIndex;
            trailingFace.Position = gridPosition.Position;

            trail.Add(trailingFace);
        }
        else
        {
            if (previous < 0 || overlap != trail[previous])
            {
                if (previous >= 0)
                {
                    Destroy(trail[previous].gameObject);
                    trail.RemoveAt(previous);
                }

                overlap.FaceIndex = dieState.TopFaceIndex;
                overlap.Position = gridPosition.Position;

                trail.Remove(overlap);
                trail.Add(overlap);
            }
        }
    }

    private bool ValidateLinearTrail()
    {
        if (trailMoves.Count < 2) return true;

        var step = trailMoves[0];

        var zeroComponent = step[0] == 0 ? 0 : 1;

        for (var i = 1; i < trailMoves.Count; ++i)
        {
            var nextStep = trailMoves[i];

            if (nextStep[zeroComponent] != 0)
            {
                return false;
            }
        }

        return true;
    }

    private struct TrailStats
    {
        public int attack;
        public int shields;
    }
}
