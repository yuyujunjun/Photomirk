using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class test : MonoBehaviour {

    // Use this for initialization
  
    void Start () {

        NotificationCenter.DefaultCenter().AddObserver(this, "ABBB");
        
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    void ABBB( Notification notification)
    {
        
        Debug.Log("Received notification from: " + notification.sender);
        if (notification.data == null)
            Debug.Log("And the data object was null!");
        else
        {
            Debug.Log("And it included a data object: " + notification.data);
            Debug.Log(notification.name);
            GameObject b = (GameObject)notification.data;
            Debug.Log(b.transform.position);
            
            
        }
    }
}
