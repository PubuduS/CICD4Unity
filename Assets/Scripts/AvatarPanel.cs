using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPanel : MonoBehaviour
{

    [SerializeField]
    private ScrollingObjectCollection scrollview;

    [SerializeField]
    private GameObject parent;

    private int m_CellIndex = -1;
    private const int m_NumberOfAvatars = 2;

    void Update()
    {
        SetVisibleCellIndex();
    }

    private void SetVisibleCellIndex()
    {
        m_CellIndex = scrollview.FirstVisibleCellIndex; 
            
            // Prevent accessing out of bound cell number
            if( m_CellIndex >= m_NumberOfAvatars)
            {
                m_CellIndex -= 1;
            }        
    }

    public int GetAvatarIndex()
    {
        return m_CellIndex;
    }

    // Toggle GUI visibility
    public void SetVisibility() 
    {
        bool isObjectActive = parent.activeSelf;
        isObjectActive = !isObjectActive;
        parent.SetActive(isObjectActive);

    }
  

}
