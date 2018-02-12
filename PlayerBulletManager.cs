using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletManager : MonoBehaviour {

    // Use this for initialization

   
    [SerializeField] GameObject[] Photobullettype;
 
    [SerializeField] GameObject[] Mirkbullettype;

    [SerializeField] GameObject Enemybulletfasttype;
    [SerializeField] GameObject Enemybulletslowtype;
    // [SerializeField] float BulletAffectRange = 5f;
    public enum bulletstype
    {
        Photo,
        Mirk,
        EnemyFast,
        EnemySlow
    }
    public class  Bullets
    {
        public bulletstype Bulletstype;
        public GameObject bullet;
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public GameObject Target;
        public Vector3 localdirection;
        public float harmful;
        public float LivingTime;
        public bool isAlive=true;
       public  float BulletFlyingSpeed;
        public float BulletsLiveTime;

    };
    LayerMask m_Mask;
    List<Bullets> AllBullets=new List<Bullets>();
	void Start () {
        NotificationCenter.DefaultCenter().AddObserver(this,"PlayerBullet");
       
	}

    // Update is called once per frame
    void Update() {
        foreach(var b in AllBullets)
        {
            b.LivingTime += Time.deltaTime;
            b.bullet.transform.Translate(b.localdirection * b.BulletFlyingSpeed*Time.deltaTime);
            if (b.LivingTime > b.BulletsLiveTime) { b.isAlive = false; }
            if (Vector3.Dot(b.bullet.transform.position-b.EndPosition,b.EndPosition-b.StartPosition)>0)
            {
                b.isAlive = false;
                b.bullet.transform.position = b.EndPosition;
                //通知粒子管理器放火花啦！
                //NotificationCenter.DefaultCenter().PostNotification(this, "FireP", b.EndPosition);
                
                if (b.Target != null&&b.Target.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))//击中敌人了
                {
                    //掉血了！
                //    Debug.Log(b.Target.name);
                    NotificationCenter.DefaultCenter().PostNotification(this,"HitRobot",b);
                }else if(b.Target != null && b.Target.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    //如果是快速的子弹，则通知玩家，由玩家判断是否被攻击
                    //如果是慢速的子弹，也通知玩家，由玩家判断是否被攻击
                    //慢速的子弹到达预定位置
                    if (b.Bulletstype == bulletstype.EnemyFast)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBeAttacked", b);//只通知身体，告诉身体做防御动作，身体通知大脑掉血了
                    }else
                    {
                        //通知粒子播放器
                        NotificationCenter.DefaultCenter().PostNotification(this, "SlowBulletBoomP",b);
                        
                    }
                }else //撞到障碍
                {
                    
                    if (b.Bulletstype == bulletstype.EnemySlow)
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "SlowBulletBoomP", b);
                        
                    }
                    else
                    {
                        NotificationCenter.DefaultCenter().PostNotification(this, "HitObstacleP", b.EndPosition);
                    }
                }
                
            }

        }
        for (int i = AllBullets.Count - 1; i >= 0; i--)
        {
            if (!AllBullets[i].isAlive)
            {
              
                Destroy(AllBullets[i].bullet);
                AllBullets.RemoveAt(i);
            }
        }

    }
    void PlayerBullet(Notification noti)
    {
        Bullets b = (Bullets)noti.data;
        GameObject single_bullet;
        Bullets temp_bullet = new Bullets();
        string TargetLayer;
        if (b.Bulletstype == bulletstype.Photo)
        {
            temp_bullet.BulletFlyingSpeed = 10;
            temp_bullet.BulletsLiveTime = 2;
            TargetLayer = "Enemy";
            m_Mask = ~(1 << LayerMask.NameToLayer("Player")|1<<LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Light"));
            if (b.harmful <= 1)
            {
                single_bullet = Instantiate(Photobullettype[0]) as GameObject;
            } else if (b.harmful <= 1.5f)
            {
                single_bullet = Instantiate(Photobullettype[1]) as GameObject;
            }
            else
            {
                single_bullet = Instantiate(Photobullettype[2]) as GameObject;
            }
        }else if (b.Bulletstype == bulletstype.Mirk)
        {
            temp_bullet.BulletFlyingSpeed = 20;
            temp_bullet.BulletsLiveTime = 2;
            TargetLayer = "Enemy";
            m_Mask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Light"));
            if (b.harmful <= 1)
            {
                single_bullet = Instantiate(Mirkbullettype[0]) as GameObject;
            }
            else if (b.harmful <= 1.5f)
            {
                single_bullet = Instantiate(Mirkbullettype[1]) as GameObject;
            }
            else
            {
                single_bullet = Instantiate(Mirkbullettype[2]) as GameObject;
            }
        }else if(b.Bulletstype==bulletstype.EnemyFast)
        {
            TargetLayer = "Player";
            //Debug.Log("FastBullet");
            temp_bullet.BulletFlyingSpeed = 10;
            temp_bullet.BulletsLiveTime = 2;
            m_Mask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Light"));
            single_bullet = Instantiate(Enemybulletfasttype) as GameObject; 
        }else  
        {
            TargetLayer = "Player";
            temp_bullet.BulletFlyingSpeed = 3;
            temp_bullet.BulletsLiveTime = 10;
            m_Mask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Level") | 1 << LayerMask.NameToLayer("Light"));
            single_bullet = Instantiate(Enemybulletslowtype) as GameObject;
        }

        
        


        Vector3 S = b.StartPosition;
        
        temp_bullet.bullet = single_bullet;
        temp_bullet.StartPosition = S;
        temp_bullet.bullet.transform.position = S;
        temp_bullet.EndPosition = S + b.BulletsLiveTime * b.BulletFlyingSpeed*b.localdirection.normalized;
        temp_bullet.LivingTime =0;
        temp_bullet.localdirection = temp_bullet.bullet.transform.InverseTransformDirection(b.localdirection).normalized;
        temp_bullet.isAlive = true;
        temp_bullet.harmful = b.harmful;
        temp_bullet.Bulletstype = b.Bulletstype;


        Ray shooting=new Ray(b.StartPosition,b.localdirection);
        RaycastHit target;
        if(Physics.Raycast(shooting,out target, temp_bullet.BulletFlyingSpeed * temp_bullet.BulletsLiveTime, m_Mask))
        {
            //Debug.Log(target.collider.gameObject.name);
            if (target.collider.gameObject.layer.Equals(LayerMask.NameToLayer(TargetLayer)))//这里判定是否攻击到了敌人
            {
                //在这里判断这个子弹是否对这种敌人有效
                if (TargetLayer == "Enemy")//如果对方是敌人的话，才需要判定，光否？暗否？
                {
                    //Debug.Log(target.collider.gameObject.GetComponent<EnemyPhotoMirk>().PM);
                    Debug.Log(target.collider.gameObject.GetComponent<EnemyPhotoMirk>().PM);
                    Debug.Log(b.Bulletstype);
                    if (target.collider.gameObject.GetComponent<EnemyPhotoMirk>().PM==EnemyPhotoMirk.PhotoOrMirk.Photo&& b.Bulletstype == bulletstype.Mirk)
                    {
                        
                            temp_bullet.Target = target.collider.gameObject;
                        
                    }
                    else if (target.collider.gameObject.GetComponent<EnemyPhotoMirk>().PM == EnemyPhotoMirk.PhotoOrMirk.Mirk && b.Bulletstype == bulletstype.Photo)
                    {
                        
                        
                            temp_bullet.Target = target.collider.gameObject;

                    }
                    else
                    {
                        temp_bullet.Target = null;
                    }
                }
                else if(TargetLayer=="Player")
                {
                    temp_bullet.Target = target.collider.gameObject;
                }
            }
            else
            {
                temp_bullet.Target = null;
            }
            temp_bullet.EndPosition = target.point;
            //Debug.Log(target.collider.gameObject.name);
           // Debug.Log(target.point);
        }
        AllBullets.Add(temp_bullet);
        temp_bullet.bullet.GetComponent<ParticleSystem>().Play();
    }
   
}
