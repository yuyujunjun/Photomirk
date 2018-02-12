using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlowAttack : GroundAttack {

	// Use this for initialization
	override
        public void Fight()
    {
        if (Vector3.Distance(Player.position, transform.position) < m_self.TransformationArea)
        {
            m_animator.SetTrigger("Attack");

          
        }
        else
        {
         
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                m_animator.SetTrigger("Idle");
            }
        
        }
    }
}
