using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTransform : MonoBehaviour {

    // Use this for initialization
    [SerializeField] Transform Player;
    Vector3 CompareDirection;

	void Start () {
        CompareDirection = transform.position - Player.position;
     //   Debug.Log(Player.position);
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = new Vector3(Camera.main.transform.forward.x,Player.forward.y, Camera.main.transform.forward.z);
        //transform.forward = Player.forward;
        transform.position = Player.position + CompareDirection;
	}
}
