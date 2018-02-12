using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine.Utility;
using Cinemachine.Timeline;
using Cinemachine.PostFX;
//这个脚本在接到攻击指令后，进行攻击，根据武器当前状况处理是否能攻击，以及控制攻击频率
public class PlayerFighting : MonoBehaviour {

    // Use this for initialization
    private bool m_Fighting = false;
    private bool m_Hotting = false;
    private float Current_Gun = 0;
    [SerializeField]private float ColdingSpeed;
    [SerializeField] private float HotSpeed;
    [SerializeField] private float ShootingSpeed=10;
    [SerializeField] private float ReShootingTime;
    [SerializeField] GameObject m_MainCamera;
    [SerializeField] GameObject m_SecondCamera;
    private float Hotting = 0;
    private float BulletInGun;
    PlayableDirector m_PDirector;
    [SerializeField] PlayableAsset zoomin;
    [SerializeField] PlayableAsset zoomout;
    [SerializeField] Material magazine;
    [SerializeField] float harmful=1f;
    GameObject m_Player;
    Transform m_camera;
    PlayerBulletManager.Bullets tempbullets;
    void Start () {
        m_Player = GameObject.Find("SUNman");
        tempbullets = new PlayerBulletManager.Bullets();
        NotificationCenter.DefaultCenter().AddObserver(this, "Fighting");
        m_PDirector = GetComponent<PlayableDirector>();
        m_camera = Camera.main.transform;
        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerFightingAddAttack");
    }
	
	// Update is called once per frame
	void Update () {//枪口冷却是随时随地都在冷却的，除了，过热的时候，过热的时候我们为了惩罚玩家不爱惜自己的武器，在一个小的时间范围内不让枪进行冷却
        if (m_Hotting)
        {
            Hotting += Time.deltaTime;
            //在这里写入shader 1
            magazine.SetFloat("_hot", 1);
            if (Hotting >= ReShootingTime)
            {
                m_Hotting = false;
                Current_Gun = 0.3f;
                Hotting = 0;
            }
        }
       

        Current_Gun -= Time.deltaTime * ColdingSpeed;
        if (Current_Gun < 0) Current_Gun = 0;
        
    }
    void Fighting(Notification noti)
    {
        bool isFight = (bool)noti.data;
        if (isFight&&m_Fighting==false)//第一次进入攻击状态
        {
            
            //初始化攻击参数（Current_Gun应该是全局的)播放timeline，通知粒子去加载特定的粒子
            m_Fighting = true;
            BulletInGun = 0;
            m_PDirector.playableAsset = zoomin;
            m_PDirector.Play();

        }
        else if (!isFight && m_Fighting == true)//退出攻击状态
        {
            //关闭攻击参数
            m_Fighting = false;
            m_PDirector.playableAsset = zoomout;
            m_PDirector.Play();
            
        }
        if (m_Fighting)//处于攻击状态
        {
            //此处填写攻击参数

            if (!m_Hotting)//判断是否可以发射子弹
            {

                //给shader写入current_gun的数值
                magazine.SetFloat("_hot",Current_Gun);
                if (Input.GetMouseButton(0))
                {
                    BulletInGun += Time.deltaTime / 1 * ShootingSpeed;
                    if (BulletInGun >= 1)
                    {
                        int Bullets = (int)BulletInGun;
                        BulletInGun -= Bullets;
                        //发射Bullets颗子弹
                       // Debug.Log(Bullets);
                        
                        NotificationCenter.DefaultCenter().PostNotification(this,"PlayerShootingP");
                        tempbullets.StartPosition = transform.position;
                        tempbullets.localdirection = m_camera.forward;
                        tempbullets.harmful = harmful;
                        tempbullets.Bulletstype = m_Player.GetComponent<PlayerBrain>().PhotoOrMirk;
                       
                        for (int i = 0; i < Bullets; i++)
                        {
                            NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet",tempbullets);
                        }

                        Current_Gun += Bullets * HotSpeed;
                        
                    }
                }
                //你丫枪口过热了
                if (Current_Gun >= 1)
                {
                    NotificationCenter.DefaultCenter().PostNotification(this, "HotReactionP");
                    m_Hotting = true;
                }

            }
        }
        else
        {
            magazine.SetFloat("_hot", 0);
        }
        
    }
    void PlayerFightingAddAttack()
    {
        harmful += 0.5f;
        if (harmful >= 2)
        {
            harmful = 2;
        }
    }
}
