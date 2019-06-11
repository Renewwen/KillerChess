using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{
    //PlayerCompass m_playerCompass;

    protected override void Awake()
    {
        base.Awake();
        //m_playerCompass = Object.FindObjectOfType<PlayCompass>().
    }

    protected override void Start()
    {
        base.Start();
        UpdateBoard();
    }

    void UpdateBoard()
    {
        if (m_board != null)
        {
            m_board.UpdatePlayerNode();
        }
    }

    protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        // player compass

        // run the parent class MoveRoutine
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        UpdateBoard();

        base.finishMovementEvent.Invoke();
    }
}