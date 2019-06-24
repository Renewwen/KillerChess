
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public enum Turn
{ 
    Player,
    Eemey
}

public class GameManager : MonoBehaviour
{
    // reference to the GameBoard
    Board m_board;
    // reference to the PlayerManager
    PlayerManager m_player;

    List<EnemyManager> m_enemies;

    Turn m_currentTurn = Turn.Player;

    public Turn CurrentTurn { get { return m_currentTurn; } }

    bool m_hasLevelStarted = false;
    bool m_isGamePlaying = false;
    bool m_isGameOver = false;
    bool m_hasLevelFinished = false;

    public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }
    public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }
    public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }
    public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

    public float delay = 1f;

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;

    void Awake()
    {
        m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
        m_player = GameObject.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();

        EnemyManager[] enemies = GameObject.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        m_enemies = enemies.ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_player != null && m_board != null)
        {
            StartCoroutine("RunGameLoop");
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    // start the level
    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if (setupEvent != null) 
        {
            setupEvent.Invoke();
        }
        Debug.Log("Start Level");
        m_player.playerInput.InputEnabled = false;
        while (!m_hasLevelStarted)
        {
            // show start screen
            // user presses button to start
            // HasLevelStarted = true
            yield return null;
        }

        if (startLevelEvent != null) 
        {
            startLevelEvent.Invoke();
        }
    }

    // gameplay stage
    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("play Level");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_player.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();    
        }

        while (!m_isGameOver)
        {
            yield return null;
            // check for game over condition

            // win
            // reach the end of the level
            m_isGameOver = IsWinner();


            // lose
            // player dies

            // m_isGameOver = true;
        }
        Debug.Log("Win!!!!!!!!");
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        m_isGameOver = true;

        yield return new WaitForSeconds(1.5f);
        // invoke loseLovelEvent
        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }

        yield return new WaitForSeconds(2f);

        Debug.Log("LOSE!");

        RestartLevel();
    }

    // end stage after gameplay is completed
    IEnumerator EndLevelRoutine()
    {
        Debug.Log("End Level");
        m_player.playerInput.InputEnabled = false;

        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        // show end screen
        while (!m_hasLevelFinished)
        {
            // user presses button to contine

            // HasLevelFinished = true;
            yield return null;
        }

        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    bool IsWinner()
    { 
        if (m_board.PlayerNode != null)
        {
            return (m_board.PlayerNode == m_board.GoalNode);
        }
        return false;
    }

    bool AreEnemiesAllDead() 
    { 
        foreach (EnemyManager enemy in m_enemies)
        { 
            if (!enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }

    // switch between Player and Enemy Turns
    public void UpdateTurn()
    {
        if (m_currentTurn == Turn.Player && m_player != null)
        { 
            if (m_player.IsTurnComplete && !AreEnemiesAllDead())
            {
                // switch to EnemyTurn and play enemies
                PlayEnemyTurn();
            }
        }
        else if (m_currentTurn == Turn.Eemey)
        {
            // if enemy turn is complete, play player turn
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }

    void PlayPlayerTurn()
    {
        m_currentTurn = Turn.Player;
        m_player.IsTurnComplete = false;

        // allow Player to move
    }

    void PlayEnemyTurn()
    {
        m_currentTurn = Turn.Eemey;

        foreach (EnemyManager enemy in m_enemies)
        { 
            if (enemy != null && !enemy.IsDead)
            {
                enemy.IsTurnComplete = false;

                enemy.PlayTurn();
            }
        }
    }

    // have all of the enemies completed their turns?
    bool IsEnemyTurnComplete()
    { 
        foreach (EnemyManager enemy in m_enemies)
        { 
            if (enemy.IsDead)
            {
                continue;
            }
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }

    public void changeToNextlevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}


