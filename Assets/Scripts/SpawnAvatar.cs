using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAvatar : MonoBehaviour
{
    [SerializeField]
    AvatarPanel avatarPanelObject;

    private void Start()
    {
        //avatarPanelObject = GetComponent<AvatarPanel>();
    }
    
    public void SpawnCharacter()
    {
        GameObject prefab = null;
        int avatarIndex = avatarPanelObject.GetAvatarIndex();        
                
        if(avatarIndex == 0)
        {
            DestroyPreviousGameObject("Jenny(Clone)");
            GameObject avatarObj = GameObject.Find("Drew(Clone)");
            if(avatarObj == null)
            {
                prefab = (GameObject)Resources.Load("Drew", typeof(GameObject));
            }   
        }
        else if(avatarIndex == 1)
        {
            DestroyPreviousGameObject("Drew(Clone)");
            GameObject avatarObj = GameObject.Find("Jenny(Clone)");
            if (avatarObj == null)
            {
                prefab = (GameObject)Resources.Load("Jenny", typeof(GameObject));
            }
            
        }

        if(prefab != null)
        {
            Vector3 forwardDirection = Camera.main.transform.forward * 3 + new Vector3(0, -0.6f, 0);
            GameObject avatar = Instantiate(prefab, Camera.main.transform.position + forwardDirection, Quaternion.LookRotation(Camera.main.transform.forward)) as GameObject;
        }

        
    }

    private void DestroyPreviousGameObject(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
        {
            Destroy(obj);
        }
    }    

}
