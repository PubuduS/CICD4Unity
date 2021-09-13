using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMessage : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Load;
    private GameObject m_StartMessage;
    private GameObject m_ThankMessage;
    // Start is called before the first frame update
    void Start()
    {
        m_StartMessage = GameObject.Find("Message");
        m_ThankMessage = GameObject.Find("ThankYou");
        m_ThankMessage.SetActive(false);
        m_StartMessage.SetActive(true);
        StartCoroutine(ShowStartingMessage()); 
    }

    private IEnumerator ShowStartingMessage()
    {
        yield return new WaitForSeconds(5);
        m_StartMessage.SetActive(false);
        m_ThankMessage.SetActive(true);
        Destroy(m_StartMessage);

        yield return new WaitForSeconds(3);
        m_ThankMessage.SetActive(false);
        Destroy(m_ThankMessage);
        Destroy(m_Load);
    }


}
