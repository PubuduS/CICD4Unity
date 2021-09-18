using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovements : MonoBehaviour
{

    Observer m_ObserverScript = null;
    Animator m_Animator = null;

    GameObject m_Player = null;
    public Transform m_Avatar = null;

    [SerializeField]
    float m_AvatarWalkSpeed = 0.007f;
    const float adjustToPlayerEyeLevel = -0.6f;


    // Start is called before the first frame update
    void Start()
    {
        m_ObserverScript = GetComponentInChildren<Observer>();
        m_Animator = GetComponent<Animator>();
        m_Player = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        AvatarWalkForward();
    }

    void AvatarWalkForward()
    {

        if (m_ObserverScript.GetPlayerInRange() == true)
        {
            Vector3 direction = m_Avatar.position - m_Player.transform.position;
            direction.y = 0;
            direction = Vector3.Normalize(direction);            
            m_Avatar.rotation = Quaternion.Euler(direction);
            SetWalking(true);
            m_Avatar.Translate(transform.forward * m_AvatarWalkSpeed * Time.deltaTime);
        }
        else if (m_ObserverScript.GetPlayerInRange() == false)
        {
            SetWalking(false);
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (m_Animator.GetBool("IsWalking") == false)
        {
            Vector3 lookAt = m_Player.transform.position;
            transform.LookAt(lookAt + new Vector3(0, -1f, 0));
        }
    }

    void SetWalking(bool walkFlag = false)
    {
        m_Animator.SetBool("IsWalking", walkFlag);
    }

    void DisableCollusions()
    {
        //Debug.Log("Disabled");
        //m_CapsuleCollider.enabled = false;
        //m_PointOfView.enabled = false;
        //m_Rigidbody.isKinematic = true;
        //m_Rigidbody.detectCollisions = false;
    }

    void EnableCollusions()
    {
        // m_CapsuleCollider.enabled = true;
        //m_PointOfView.enabled = true;
        //m_Rigidbody.isKinematic = false;
        //m_Rigidbody.detectCollisions = true;
    }

}// end of the class
