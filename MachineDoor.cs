using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineDoor : MonoBehaviour {
    [SerializeField ]bool IsWhite;
    PlayerBrain Playerbrain;
    Collider m_collider;
    // Use this for initialization
	void Start () {
        Playerbrain = GameObject.Find("SUNman").GetComponent<PlayerBrain>();
        m_collider =GetComponent<Collider>();
     
    }
	
	// Update is called once per frame
	void Update () {
        if( Playerbrain.PhotoOrMirk== PlayerBulletManager.bulletstype.Photo ){
            if (IsWhite == true)
            {
                m_collider.isTrigger = true;
            }else
            {
                m_collider.isTrigger = false;
            }
        }
        else if (Playerbrain.PhotoOrMirk==PlayerBulletManager.bulletstype.Mirk) {
            if (IsWhite == true)
            {
                m_collider.isTrigger = false;
            }
            else
            {
                m_collider.isTrigger = true;
            }
        }
        
	}
}
