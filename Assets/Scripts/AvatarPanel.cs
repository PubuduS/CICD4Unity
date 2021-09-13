using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPanel : MonoBehaviour
{

    [SerializeField]
    private ScrollingObjectCollection scrollview;

    [SerializeField]
    private GameObject scrollParent;

    private int m_CellIndex = -1;
    private const int m_NumberOfAvatars = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
  

}
