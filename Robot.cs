using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Robot : MonoBehaviour {
    public enum RobotType
    {
        GroundSlow,
        GroundFast,
        FlyingSlow,
        FlyingFast
    }
    public RobotType RT;
    public float AllHP;
    public float HP;
    Transform EnemyTarget;
    Animator m_animator;
    NavMeshAgent m_agent;
   // Rigidbody m_rigid;
    EnemyFramework m_Brain;
    float ObstacleBias=2;

    public float harmful;
   // [SerializeField]public bool Photo = true;

    public int BulletsInGun;//当前子弹数量
    public int BulletsInBox;//一梭子子弹的数量
    float ShootingSpeed;//射速
    public bool Cartridging;//是否正在更换弹夹,这个状态只用于判断当前正在做什么事，用来通知大脑降低fight的影响，不具备其他操作
    [SerializeField]public float Cartridgetime;
    float CartridgeNeedyTime;
    public bool Shooting;//是否正在射击
    private float waitingbullets;//正准备射出去的子弹
    bool Finding;
    bool Fighting;
    [SerializeField]float m_y;
    bool Hiding;
    bool Tracing;
    bool Waring;
    bool haddead;
    [SerializeField] ParticleSystem FireParticle=null;


    // Use this for initialization
    void Start () {
        haddead = false;
        m_Brain = GetComponent<EnemyFramework>();
        EnemyTarget = GameObject.Find("SUNman").GetComponent<Transform>();
        m_agent = GetComponentInParent<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
       // m_rigid = GetComponent<Rigidbody>();
        m_y = transform.position.y;
        if (RT == RobotType.GroundSlow)
        {
            AllHP = 1000;
            ShootingSpeed = 1;
            CartridgeNeedyTime = 3;
            BulletsInBox = 1000;//反正用不到这个，乱设一个方便debug
            harmful = 10;
            m_Brain.AttackRange = 4f;
        }
        else if (RT == RobotType.GroundFast)
        {
            AllHP = 600;
            ShootingSpeed = 10;
            CartridgeNeedyTime = 2;
            BulletsInBox = 30;
            harmful = 1;
            m_Brain.AttackRange = 20f;
        }
        else if (RT == RobotType.FlyingFast)
        {
            AllHP = 500;
            ShootingSpeed = 10;
            CartridgeNeedyTime = 2;
            BulletsInBox = 100;
            harmful = 1;
            m_Brain.AttackRange = 10f;
        }
        else if (RT == RobotType.FlyingSlow)
        {
            AllHP = 700;
            ShootingSpeed = 0.3f;
            CartridgeNeedyTime = 3;
            BulletsInBox = 1000;
            harmful = 10;
            m_Brain.AttackRange = 10f;

        }


        BulletsInGun = BulletsInBox;
        Shooting = false;
        
       
        InitState();
        Waring = false;
        HP = AllHP;
       
        ObstacleBias = 1;
        NotificationCenter.DefaultCenter().AddObserver(this, "HitRobot");
        Cartridging = false;
    }
	void HitRobot(Notification noti)
    {
        PlayerBulletManager.Bullets b = (PlayerBulletManager.Bullets)noti.data;
        
        if (b.Target.name.Equals(gameObject.name))
        {
            m_Brain.BeAttacked();
            HP = HP - b.harmful;
            NotificationCenter.DefaultCenter().PostNotification(this, "HitEnemyP", b.EndPosition);
           // Vector3 direction = transform.position - EnemyTarget.transform.position;
           // m_rigid.velocity = direction.normalized;
            if (HP <= 0&&!haddead)
            {
                m_animator.SetTrigger("Dead");
                m_agent.ResetPath();
                NotificationCenter.DefaultCenter().PostNotification(this,"RobotDead");
                if (FireParticle != null)
                {
                    NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletNotFireP", FireParticle);
                }
                NotificationCenter.DefaultCenter().PostNotification(this,"DeadEnemyP",transform.position);
                GetComponent<BoxCollider>().enabled = false;
                DestroyObject(gameObject,2);
                haddead = true;
              
            }
        }
    }
	// Update is called once per frame
	void Update () {
        //Debug.Log(Shooting);
        
        if (HP > 0)
        {




            if (!Shooting)//如果没有正在射击，就让它换子弹（算了改成子弹射击完再换弹夹
            {
                Cartridging = true;
                //if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Cartridge"))
                //{
                //    m_animator.SetTrigger("Cartridge");
                //}
                Cartridgetime += Time.deltaTime;
                if (Cartridgetime > CartridgeNeedyTime)
                {

                    Cartridging = false;
                    BulletsInGun = BulletsInBox;
                }
            }
            if (Waring)
            {
                if (Finding)
                {
                    m_agent.ResetPath();
                    m_animator.SetTrigger("Idle");
                    if (FireParticle != null)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletNotFireP", FireParticle);
                    }
                    Quaternion tempQ = Quaternion.LookRotation(EnemyTarget.position - transform.position,new Vector3(0,1,0));
                    transform.rotation = Quaternion.Slerp(transform.rotation,tempQ,0.02f);
                }
                if (Tracing)
                {
                    if (transform.position.y != m_y&&!m_Brain.Attacked)
                    {
                        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, m_y, 0.2f), transform.position.z);
                    }
                    m_agent.ResetPath();
                    
                    m_animator.SetTrigger("Idle");
                    if (FireParticle != null)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletNotFireP", FireParticle);
                    }
                    m_agent.SetDestination(new Vector3(EnemyTarget.position.x,m_y,EnemyTarget.position.z));
                    m_agent.stoppingDistance = m_Brain.AttackRange / 5 * 3;
                }
                if (Hiding)
                {
                    if (transform.position.y != m_y && !m_Brain.Attacked)
                    {
                        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, m_y, 0.2f), transform.position.z);
                    }
                    m_agent.ResetPath();
                    m_animator.SetTrigger("Idle");
                    if (FireParticle != null)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletNotFireP", FireParticle);
                    }
                    Vector3 position = m_Brain.Obstacles[m_Brain.NearestObstacle].position;
                    Vector3 direction = (-EnemyTarget.position + transform.position).normalized;
                    Vector3 Destination = position + direction * ObstacleBias;
                    m_agent.SetDestination(Destination);
                    m_agent.stoppingDistance = ObstacleBias / 2;

                }
                if (Fighting)
                {
                    m_agent.ResetPath();

                    //Shooting = true;
                    if (RT == RobotType.FlyingFast )
                    {
                        CalculatelNumberOfBullets();
                    }
                    else
                    {
                        CalculatelSlowBullets();
                    }
                    if (Shooting)
                    {
                        Quaternion tempQ = Quaternion.LookRotation(EnemyTarget.position - transform.position, new Vector3(0, 1, 0));
                        transform.rotation = Quaternion.Slerp(transform.rotation, tempQ, 0.05f);
                    }


                }
            }
        }
        
	}
    public void action_Fighting()//兄弟刚不刚？
    {
        InitState();
        Fighting = true;
        
    }
    public void action_Finding()
    {
        InitState();
        Shooting = false;
        Finding = true;

    }
    public void action_Waring(bool war)
    {
        Waring = war;
    }
    public void action_Tracing()
    {
        InitState();
        Shooting = false;
        Tracing = true;
    }
    public void action_Hiding()
    {
        InitState();
        Shooting = false;
        Hiding = true;
    }
    float AbsoluteValue(float a)
    {
        float b;
        b = a > 0 ? a : -a;
        return b;
    }
    void InitState()
    {
        Finding = false;
        Hiding = false;
        Fighting = false;
        Tracing = false;
      //  Shooting = false;
    }


    void CalculatelNumberOfBullets()
    {
        waitingbullets += Time.deltaTime * ShootingSpeed;
        
        if (waitingbullets >= 1)
        {
            int bullets = (int)waitingbullets;
            waitingbullets -= bullets;
            if (BulletsInGun < bullets)
            {
                bullets = BulletsInGun;
                //弹夹打完了欸
                Shooting = false;
                BulletsInGun = 0;
                Cartridging = true;
            }
            else
            {
                Shooting = true;
                Cartridgetime = 0;
                // Debug.Log(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"));
                if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
                {
                    m_animator.SetTrigger("Shooting");
                    
                }
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
                {
                    if (FireParticle != null)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletFireP", FireParticle);
                    }
                    BulletsInGun -= bullets;
                    //发出bullets发子弹
                    PlayerBulletManager.Bullets tempbullets = new PlayerBulletManager.Bullets();
                    tempbullets.StartPosition = transform.position;
                    tempbullets.localdirection = transform.forward;
                    tempbullets.harmful = harmful;
                    tempbullets.Bulletstype = PlayerBulletManager.bulletstype.EnemyFast;
                    for (int i = 0; i < bullets; i++)
                    {
                        //Debug.Log("EnemyFast");
                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);
                    }
                }
            }
           
        }
    }
    private void OnDestroy()
    {
        FortuneManager.Fortune_base fb = new FortuneManager.Fortune_base();
        fb.S = transform.position;
        if (RT == RobotType.FlyingSlow||RT==RobotType.GroundSlow)
        {
            fb.Rescue = 10;
        }
        else
        {
            fb.AddAttack = 0.05f;
        }
        
        NotificationCenter.DefaultCenter().PostNotification(this, "InstantiateFortune",fb);
    }
    void CalculatelSlowBullets()
    {
        waitingbullets += Time.deltaTime * ShootingSpeed;

        if (waitingbullets >= 1)
        {
            int bullets = (int)waitingbullets;
            waitingbullets -= bullets;
            //如果准备好了子弹，就进入攻击状态，并由动画来判定什么时候离开攻击状态进入换弹夹状态
            Shooting = true;
            Cartridgetime = 0;
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                    m_animator.SetTrigger("Shooting");
            }
        }
    }


}
