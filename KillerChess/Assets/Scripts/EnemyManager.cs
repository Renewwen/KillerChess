using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : TurnManager
{
    EnemyMover m_enemyMover;
    EnemySensor m_enemySensor;
    EnemyAttack m_enemyAttack;
    Board m_board;

    bool m_isDead = false;
    public bool IsDead { get { return m_isDead; } }

    public UnityEvent deathEvent;

    // setup member variables
    protected override void Awake()
    {
        base.Awake();

        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        m_enemyMover = GetComponent<EnemyMover>();
        m_enemySensor = GetComponent<EnemySensor>();
        m_enemyAttack = GetComponent<EnemyAttack>();
    }

    public void PlayTurn()
    {
        if (m_isDead)
        {
            FinishTurn();
            return;
        }
        StartCoroutine(PlayTurnRoutine());
    }

    // main enemy routine: detect/attack Player!!!!
    IEnumerator PlayTurnRoutine()
    {
        if (m_gameManager != null && !m_gameManager.IsGameOver)
        {
            // detect player
            m_enemySensor.UpdateSensor(m_enemyMover.CurrentNode);

            // wait
            yield return new WaitForSeconds(0f);

            if (m_enemySensor.FoundPlayer)
            {
                // notify the GameManager to lose the level
                m_gameManager.LoseLevel();

                Vector3 playerPosition = new Vector3(m_board.PlayerNode.Coordinate.x, 0f,
                                                     m_board.PlayerNode.Coordinate.y);
                m_enemyMover.Move(playerPosition, 0f);

                while (m_enemyMover.isMoving) 
                {
                    yield return null;
                }
                // attack player
                m_enemyAttack.Attack();
            }
            else
            {
                // movement
                m_enemyMover.MoveOneTurn();

            }
        }
    }

    public void Die()
    { 
        if (m_isDead)
        {
            return;
        }
        m_isDead = true;

        if (deathEvent != null)
        {
            deathEvent.Invoke();
        }
    }
}
