using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour {

    // Use this for initialization
    public Robot.RobotType RT;
   public  Transform EnemyTarget;
    public float AllHP;
    public float HP;
    Animator m_animator;

    public bool hasTransformation = false;

    bool haddead = false;
    [SerializeField]public float TransformationArea=10;
   
   
    void Start () {
 
        NotificationCenter.DefaultCenter().AddObserver(this, "HitRobot");
        AllHP = 500;
        m_animator = GetComponent<Animator>();
     
        EnemyTarget = GameObject.Find("SUNman").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        Transformation();

    }
    void HitRobot(Notification noti)
    {
        PlayerBulletManager.Bullets b = (PlayerBulletManager.Bullets)noti.data;

        if (b.Target.transform.parent.name.Equals(gameObject.name))
        {
            
            HP = HP - b.harmful;
            NotificationCenter.DefaultCenter().PostNotification(this, "HitEnemyP", b.EndPosition);
            // Vector3 direction = transform.position - EnemyTarget.transform.position;
            // m_rigid.velocity = direction.normalized;
            if (HP <= 0 && !haddead)
            {
               
                NotificationCenter.DefaultCenter().PostNotification(this, "RobotDead");
                NotificationCenter.DefaultCenter().PostNotification(this, "DeadEnemyP", b.Target.transform.position);
                GetComponentInChildren<SphereCollider>().enabled = false;
                DestroyObject(gameObject, 2);
                haddead = true;

            }
        }
    }
    void Transformation()
    {
        if (!hasTransformation && Vector3.Distance(transform.position, EnemyTarget.transform.position) < TransformationArea)
        {
            m_animator.SetBool("Transformation",true);
        }
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hasTransformation = true;
        }
    }
    private void OnDestroy()
    {
        FortuneManager.Fortune_base fb = new FortuneManager.Fortune_base();
        fb.S = transform.position;
        if (RT == Robot.RobotType.FlyingSlow || RT == Robot.RobotType.GroundSlow)
        {
            fb.Rescue = 10;
        }
        else
        {
            fb.AddAttack = 0.05f;
        }

        NotificationCenter.DefaultCenter().PostNotification(this, "InstantiateFortune", fb);
    }
   
}
