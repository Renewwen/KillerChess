using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // store horizontal input
    private float m_h;
    public float H
    {
        // read only
        get { return m_h; }
    }

    // store the vertical value
    private float m_v;
    public float V 
    { 
        // read only
        get { return m_v; }
    }

    // check whether users could input or not
    private bool m_inputEnable = false;
    // Using properties 
    public bool InputEnabled
    { 
        get { return m_inputEnable; }
        set { m_inputEnable = value; }
    }

    public void GetKeyInput()
    {
        if (m_inputEnable)
        {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
        else 
        {
            m_h = 0f;
            m_v = 0f;
        }
    }

}
