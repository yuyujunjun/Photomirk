using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBullet : MonoBehaviour {

    // Use this for initialization
   
    [SerializeField] ParticleSystem SlowAttack;
    

    // Update is called once per frame
    void Update() {

    }
    virtual public void Shooting()
    {

    }
    public void Attack()
    {
//        Debug.Log(SlowAttack.isPlaying);
        SlowAttack.Play();
    }
}
