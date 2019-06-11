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

    // check if the Player has moved into our snesor
    public void UpdateSensor(Node enemyNode)
    {
        // convert the local directionToSearch into a world space 3d position
        Vector3 worldSpacePositionToSearch = transform.TransformVector(directionToSearch) +
            transform.position;

        if (m_board != null)
        {
            // find the node
            m_nodeToSearch = m_board.FindNodeAt(worldSpacePositionToSearch);

            // check if enemy has the way to go to that node?
            if (!enemyNode.LinkedNodes.Contains(m_nodeToSearch))
            {
                m_foundPlayer = false;
                return;
            }

            // check if the node is player?
            if (m_nodeToSearch == m_board.PlayerNode)
            {
                m_foundPlayer = true;
            }
        }
    }


}
