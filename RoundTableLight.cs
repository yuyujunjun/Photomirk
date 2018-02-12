using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTableLight : MonoBehaviour {

    // Use this for initialization
    [SerializeField]public bool IsOn;
	void Start () {
        gameObject.layer = LayerMask.NameToLayer("Light");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeLightState()
    {
        if (IsOn)
        {
            IsOn = false;
        }else
        {
            IsOn = true;
        }
    }
}
