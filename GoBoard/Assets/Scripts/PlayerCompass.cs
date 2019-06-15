using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    Board m_board;

    public GameObject arrowPrefab;

    List<GameObject> m_arrows = new List<GameObject>();

    public float scale = 1f;
    public float startDistance = 0.25f;
    public float endDistance = 0.5f;

    public float moveTime = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public float delay = 0f;

    void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        SetupArrows();
        MoveArrows();
    }

    void SetupArrows()
    { 
        if (arrowPrefab == null) 
        {
            Debug.LogWarning("PLAYERCOMPASS Setup ERROR: missing arrow prefab!");    
        }

        foreach (Vector2 dir in Board.directions) 
        {
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            GameObject arrowInstance = Instantiate(arrowPrefab, transform.position + dirVector * startDistance, rotation);
            arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
            arrowInstance.transform.parent = transform;

            m_arrows.Add(arrowInstance);
        }
    }

    // use iTween to animate a single arrow
    void MoveArrow(GameObject arrowInstance)
    {
        // animate the arrow in a cycle from startDistance to endDistance in local z
        iTween.MoveBy(arrowInstance, iTween.Hash(
            "z", endDistance - startDistance,
            "looptype", iTween.LoopType.loop,
            "time", moveTime,
            "easetype", easeType
            ));
    }

    // animate all of the arrows
    void MoveArrows()
    { 
        foreach (GameObject arrow in m_arrows)
        {
            MoveArrow(arrow);
        }
    }

    public void ShowArrows(bool state)
    { 
        if (m_board == null)
        {
            Debug.LogWarning("PLAYERCOMPASS ShowArrows ERROR: on Board found!");
            return;
        }

        if (m_arrows == null || m_arrows.Count != Board.directions.Length)
        {
            Debug.LogWarning("PLAYERCOMPASS ShowArrows ERROR: on arrows found!");
            return;
        }

        if (m_board.PlayerNode != null)
        { 
            for (int i = 0; i < Board.directions.Length; i++)
            {
                Node neighbor = m_board.PlayerNode.FindNeighborAt(Board.directions[i]);

                if (neighbor == null)
                {
                    m_arrows[i].SetActive(false);
                }
                else
                {
                    bool activeState = m_board.PlayerNode.LinkedNodes.Contains(neighbor);
                    m_arrows[i].SetActive(activeState);
                }
            }
        }
        ResetArrows();
        MoveArrows();
    }

    void ResetArrows()
    {
        for (int i = 0; i < Board.directions.Length; i++)
        {
            iTween.Stop(m_arrows[i]);
            Vector3 dirVector = new Vector3(Board.directions[i].normalized.x, 0f,
                                            Board.directions[i].normalized.y);
            m_arrows[i].transform.position = transform.position + dirVector * startDistance;
        }
    }

}
