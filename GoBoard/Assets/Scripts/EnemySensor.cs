using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public Vector3 directionToSearch = new Vector3(0f, 0f, 2f);

    Node m_nodeToSearch;
    Board m_board;

    bool m_foundPlayer = false;
    public bool FoundPlayer { get { return m_foundPlayer; } }
    // Start is called before the first frame update
    void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void UpdateSensor()
    {
        Vector3 worldSpacePositionToSearch = transform.TransformVector(directionToSearch) +
            transform.position;
        if (m_board != null)
        {
            m_nodeToSearch = m_board.FindNodeAt(worldSpacePositionToSearch);

            if (m_nodeToSearch == m_board.PlayerNode)
            {
                m_foundPlayer = true;
            }
        }
    }


}
