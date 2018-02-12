using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpop : MonoBehaviour {

    // Use this for initialization
    [SerializeField]GameObject a;
	void Start () {
        NotificationCenter.DefaultCenter().PostNotification(this, "ABBB",a);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
