using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : MonoBehaviour {

    // Use this for initialization
    public Transform Player;
    
    
    
    
    
    public GroundEnemy m_self;
    
    public Animator m_animator;
    
	void Start () {
        m_self = GetComponentInParent<GroundEnemy>();
        m_animator = GetComponentInParent<Animator>();
        Player = GameObject.Find("SUNman").transform;		
	}
	virtual public void Init()
    {

    }
	// Update is called once per frame
	void Update () {
        if (m_self.hasTransformation)
        {
            Fight();
        }
	}
    virtual public void Fight()
    {
       
    }
}
