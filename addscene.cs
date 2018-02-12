using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class addscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync("3",LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(SceneManager.GetActiveScene().name);
	}
}
