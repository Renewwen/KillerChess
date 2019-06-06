using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : TurnManager
{
    public PlayerMover playerMover;
    public PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();

        // cache references to PlayerMover and PlayerInput
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();

        // make sure the input is enabled when we begin
        playerInput.InputEnabled = true;
    }

    void Update()
    {
        if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player) 
        {
            return;
        }

        playerInput.GetKeyInput();

        if (playerInput.V == 0) 
        { 
            if (playerInput.H < 0) 
            {
                playerMover.MoveLeft();
            } 
            else if (playerInput.H > 0)
            {
                playerMover.MoveRight();
            }
        } 
        else if (playerInput.H == 0)
        { 
            if (playerInput.V > 0) 
            {
                playerMover.MoveForward();
            }
            else if (playerInput.V < 0)
            {
                playerMover.MoveBackward();
            }
        }
    }
}
