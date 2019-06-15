using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{
    PlayerCompass m_playerCompass;

    protected override void Awake()
    {
        base.Awake();
        m_playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
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
        if (m_playerCompass != null)
        {
            m_playerCompass.ShowArrows(false);
        }
        // run the parent class MoveRoutine
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        UpdateBoard();

        if (m_playerCompass != null)
        {
            m_playerCompass.ShowArrows(true);
        }


        base.finishMovementEvent.Invoke();
    }
}