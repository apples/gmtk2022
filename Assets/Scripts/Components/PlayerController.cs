using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridPosition))]
public class PlayerController : MonoBehaviour, ICharacterBehavior
{
    public TileGridReference tileGridReference;

    private PlayerControls controls;

    private GridPosition gridPosition;
    private SyncGridPosition syncGridPosition;

    private Vector2Int? queuedMove;

    private enum Status
    {
        WaitingForInput,
        Moving,
    }

    private Status status;

    void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
        syncGridPosition = GetComponent<SyncGridPosition>();
        controls = new PlayerControls();
        controls.PlayerDice.MoveUp.performed += this.controls_PlayerDice_MoveUp_performed;
        controls.PlayerDice.MoveDown.performed += this.controls_PlayerDice_MoveDown_performed;
        controls.PlayerDice.MoveLeft.performed += this.controls_PlayerDice_MoveLeft_performed;
        controls.PlayerDice.MoveRight.performed += this.controls_PlayerDice_MoveRight_performed;

        controls.PlayerDice.MoveUp.canceled += this.controls_PlayerDice_MoveUp_canceled;
        controls.PlayerDice.MoveDown.canceled += this.controls_PlayerDice_MoveDown_canceled;
        controls.PlayerDice.MoveLeft.canceled += this.controls_PlayerDice_MoveLeft_canceled;
        controls.PlayerDice.MoveRight.canceled += this.controls_PlayerDice_MoveRight_canceled;
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
            default:
                throw new InvalidOperationException($"Unknown status {status}");
        }
    }

    private TurnResult WaitingForInput()
    {
        if (queuedMove is Vector2Int qm)
        {
            var newPos = gridPosition.Position + qm;
            queuedMove = null;

            if (tileGridReference.Current.IsTileEmpty(newPos))
            {
                gridPosition.Position = newPos;
                status = Status.Moving;
            }
        }

        return TurnResult.Wait;
    }

    private TurnResult Moving()
    {
        if (syncGridPosition == null || syncGridPosition.Done)
        {
            return TurnResult.EndTurn;
        }

        return TurnResult.Wait;
    }
}
