using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    Transform m_player = null;
    bool m_IsPlayerInRange = false;

    private void Start()
    {
        m_player = GameObject.Find("Main Camera").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == m_player)
        {            
            m_IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == m_player)
        {            
            m_IsPlayerInRange = false;
        }
    }

    public bool GetPlayerInRange()
    {
        return m_IsPlayerInRange;
    }

}
