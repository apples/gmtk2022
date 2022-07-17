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

    [SerializeField]
    private GameObject diePivot;

    private PlayerControls controls;

    private GridPosition gridPosition;
    private SyncGridPosition syncGridPosition;
    private DieAttackSwing dieAttackSwing;

    private Dir? queuedMove;

    private enum Status
    {
        WaitingForInput,
        Moving,
        Attacking,
        WaitingForSpin,
    }

    private Status status;

    private DieState dieState;

    private List<TrailingFace> trail = new List<TrailingFace>(6);
    private List<Vector2Int> trailMoves = new List<Vector2Int>(6);

    private bool isSpinAnimationDone = false;

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

        controls.PlayerDice.SpinCW.performed += this.controls_PlayerDice_SpinCW_performed;
        controls.PlayerDice.SpinCCW.performed += this.controls_PlayerDice_SpinCCW_performed;

        controls.PlayerDice.MoveUp.canceled += this.controls_PlayerDice_MoveUp_canceled;
        controls.PlayerDice.MoveDown.canceled += this.controls_PlayerDice_MoveDown_canceled;
        controls.PlayerDice.MoveLeft.canceled += this.controls_PlayerDice_MoveLeft_canceled;
        controls.PlayerDice.MoveRight.canceled += this.controls_PlayerDice_MoveRight_canceled;

        controls.PlayerDice.SpinCW.canceled += this.controls_PlayerDice_SpinCW_canceled;
        controls.PlayerDice.SpinCCW.canceled += this.controls_PlayerDice_SpinCCW_canceled;

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

    private void controls_PlayerDice_MoveLeft_performed(InputAction.CallbackContext obj)
    {
        if (controls.PlayerDice.SpinModifier.IsPressed())
        {
            Move(Dir.SpinCW);
        }
        else
        {
            Move(Dir.Left);
        }
    }

    private void controls_PlayerDice_MoveRight_performed(InputAction.CallbackContext obj)
    {
        if (controls.PlayerDice.SpinModifier.IsPressed())
        {
            Move(Dir.SpinCCW);
        }
        else
        {
            Move(Dir.Right);
        }
    }

    private void controls_PlayerDice_MoveUp_performed(InputAction.CallbackContext obj) => Move(Dir.Up);
    private void controls_PlayerDice_MoveDown_performed(InputAction.CallbackContext obj) => Move(Dir.Down);

    private void controls_PlayerDice_SpinCW_performed(InputAction.CallbackContext obj) => Move(Dir.SpinCW);
    private void controls_PlayerDice_SpinCCW_performed(InputAction.CallbackContext obj) => Move(Dir.SpinCCW);

    private void controls_PlayerDice_MoveLeft_canceled(InputAction.CallbackContext obj)
    {
        CancelMove(Dir.Left);
        CancelMove(Dir.SpinCW);
    }

    private void controls_PlayerDice_MoveRight_canceled(InputAction.CallbackContext obj)
    {
        CancelMove(Dir.Right);
        CancelMove(Dir.SpinCCW);
    }

    private void controls_PlayerDice_MoveUp_canceled(InputAction.CallbackContext obj) => CancelMove(Dir.Up);
    private void controls_PlayerDice_MoveDown_canceled(InputAction.CallbackContext obj) => CancelMove(Dir.Down);

    private void controls_PlayerDice_SpinCW_canceled(InputAction.CallbackContext obj) => CancelMove(Dir.SpinCW);
    private void controls_PlayerDice_SpinCCW_canceled(InputAction.CallbackContext obj) => CancelMove(Dir.SpinCCW);

    private void Move(Dir dir)
    {
        queuedMove = dir;
    }

    private void CancelMove(Dir dir)
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
        return status switch
        {
            Status.WaitingForInput => WaitingForInput(),
            Status.Moving => Moving(),
            Status.Attacking => Attacking(),
            Status.WaitingForSpin => WaitingForSpin(),
            _ => throw new NotImplementedException($"Unknown status {status}"),
        };
    }

    private TurnResult WaitingForSpin()
    {
        if (isSpinAnimationDone)
        {
            foreach (var fmr in faceMeshRenderers)
            {
                fmr.transform.SetParent(this.transform, true);
            }

            diePivot.GetComponent<Animator>().enabled = false;
            diePivot.transform.eulerAngles = Vector3.zero;

            return TurnResult.EndTurn;
        }

        return TurnResult.Wait;
    }

    public void SetSpinAnimationDone()
    {
        isSpinAnimationDone = true;
    }

    private TurnResult WaitingForInput()
    {
        if (queuedMove is Dir qd)
        {
            return qd switch
            {
                Dir.Left => WaitingForInput_Move(qd, Vector2Int.left),
                Dir.Right => WaitingForInput_Move(qd, Vector2Int.right),
                Dir.Up => WaitingForInput_Move(qd, Vector2Int.up),
                Dir.Down => WaitingForInput_Move(qd, Vector2Int.down),
                Dir.SpinCW => WaitingForInput_Spin(qd),
                Dir.SpinCCW => WaitingForInput_Spin(qd),
                _ => throw new NotImplementedException(),
            };
        }

        return TurnResult.Wait;
    }

    private TurnResult WaitingForInput_Spin(Dir dir)
    {
        var animName = dir == Dir.SpinCW ? "Base Layer.PlayerDieSpinCW" : "Base Layer.PlayerDieSpinCCW";

        diePivot.transform.localPosition = Vector3.zero;
        diePivot.transform.localEulerAngles = Vector3.zero;

        foreach (var fmr in faceMeshRenderers)
        {
            fmr.transform.SetParent(diePivot.transform, true);
        }

        var animator = diePivot.GetComponent<Animator>();
        animator.enabled = true;
        animator.Play(animName);

        dieState.Spin(dir);

        isSpinAnimationDone = false;
        status = Status.WaitingForSpin;

        return TurnResult.Wait;
    }

    private TurnResult WaitingForInput_Move(Dir dir, Vector2Int qm)
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

            dieState.Spin(dir);
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
