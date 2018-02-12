using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFramework : MonoBehaviour {

    // Use this for initialization
    //表示AI的意愿强烈程度
    [SerializeField] bool isWar;//是否在战斗中（包括攻击躲避等一系列状态
    [SerializeField] float isFight;//是否需要刚
    [SerializeField] float isHide;//是否需要怂
  //  [SerializeField] float isCartridge;//是否需要更换弹夹
    [SerializeField] float isFind;//是否需要去寻找现在打我的人在哪？
    [SerializeField] float isTrace;//是否需要追敌人
    //表示AI知道的一些事实
    float InsightRange=10;//观察范围
    [SerializeField]public float AttackRange;//我的子弹射程，我应该知道的吧？应该没那么蠢吧？
    [SerializeField]float EnemyRange;//敌人距离我的距离，如果我看不见敌人，那么应该是无限大
   [SerializeField] List<float> ObstacleRange=new List<float>();//最近的我能达到的掩体离我的距离，我的智商找得到掩体的吧？
    public int NearestObstacle;//最近的障碍物的编号
    //表示当前正在发生的事情（AI应该知道当前发生了什么。如果AI特别蠢呢？反正最后写出来肯定还是特别蠢的）
    [SerializeField]public bool Waring;//是否正在战斗
    [SerializeField]float Waringtime;
    [SerializeField]public bool isWatching;//此时是否能看见敌人
    [SerializeField]bool inShootingAngleRange;//在攻击的角度之内

    [SerializeField]public bool Attacked;//是否正在被攻击
    [SerializeField] float Attackedtime;
    //在当前正在发生的事情中，有一些事情是AI底层的行为
    

    [SerializeField]float ReactionDelay=2;//忘记时间，所有状态至少应该持续一会儿


    //表示AI当前的一些状态
    float allHP;//总体血量
    float HP;//当前血量

    


    Transform EnemyTarget;
    [SerializeField]public  List<Transform> Obstacles=new List<Transform>();
    //影响因子
    float HP_affect_Fight=0.9f;//血量百分比对攻击状态的影响
    float HP_affect_Hide=1;
    float Range_affect=0.3f;//距离影响因子
    float Desired_affect=0.3f;//权重影响因子
    Robot m_Body;
	void Start () {
        EnemyTarget = GameObject.Find("SUNman").GetComponent<Transform>();
        m_Body = GetComponent<Robot>();
        isFight = 0;

        isHide = 0;
        Waring = false;
        isWatching = false;

    
        Attacked = false;
        inShootingAngleRange = false;
        EnemyRange = Mathf.Infinity;
        foreach(var a in Obstacles)
        {
            ObstacleRange.Add(0);
        }
        
	}
	public void BeAttacked()
    {
        isWar = true;
        Attacked = true;
        Attackedtime = 0;


    }
    // Update is called once per frame
    void Update() {
        if (Attacked)
        {
            Attackedtime += Time.deltaTime;
            if (Attackedtime > 0.5)
            {
                Attacked = false;
            }
        }
        //每时每刻需要更新的量有血量，和是否看的见玩家
        allHP = GetComponent<Robot>().AllHP;
        HP = GetComponent<Robot>().HP;
        //是否看得见玩家
        isWatching = IsTargetInsight();
        //计算与玩家的距离
        CalculateTargetDistance();
        if (isWatching)
        {
            isWar = true;
        }
        if (isWar)
        {
            Waring = true;
            m_Body.action_Waring(Waring);
            Waringtime = 0;
            isWar = false;
        }
        if (Waring)
        {
            NearestObstacle = CalculateObstacleDistance();

            Waringtime += Time.deltaTime;
            if (Waringtime > ReactionDelay*100)
            {
                Waring = false;
                m_Body.action_Waring(Waring);
            }
            //决定做哪一个动作
            float tempaction=Mathf.Max(isFight,isTrace,isHide,isFind);
            if (isFight == tempaction)
            {
                m_Body.action_Fighting();
            }else if (isFind == tempaction)
            {
                m_Body.action_Finding();
            }else if (isHide == tempaction)
            {
                m_Body.action_Hiding();
            }else if (isTrace == tempaction)
            {
                m_Body.action_Tracing();
            }
            
        }
        
        
        //进行移动的判定
        
        //进行攻击的判定


        isFight = FightDesired();
        isTrace = TraceDesired();
        isHide = HideDesired();
        isFind = FindDesired();

	}
    //玩家是否在视线范围以内
    bool IsTargetInsight()
    {
        bool Insight = true;
        CalculateTargetDistance();
        //判断距离
        if (EnemyRange >= InsightRange)
        {
            Insight = false;
        }
        //判断角度
        if (Insight)
        {
            float angle = Vector3.Angle(transform.forward, EnemyTarget.transform.position - transform.position);
            if (angle < 80)
            {
               

                isWatching = true;
                if (angle < 8)
                {
                    inShootingAngleRange = true;
                }
                else
                {
                    inShootingAngleRange = false;
                }

            }
            else
            {
                Insight = false;
            }
        }
        if (Insight)
        {
            //判断是否有障碍物
            LayerMask m_Mask = 1 << LayerMask.NameToLayer("Obstacle");
            Ray ray = new Ray(transform.position, EnemyTarget.transform.position - transform.position);
            
            if (Physics.Raycast(ray,m_Mask))
            {
                Insight = false;
            }
        }
        return Insight;
    }
    
    void CalculateTargetDistance()
    {
        EnemyRange = Vector3.Distance(transform.position,EnemyTarget.transform.position);

    }
    int CalculateObstacleDistance()
    {
        int nearest = 0;
        float min = Mathf.Infinity;
        for(int i = 0; i < Obstacles.Count; i++)
        {
            ObstacleRange[i] = Vector3.Distance(transform.position,Obstacles[i].position);
            
            //询问当前的掩体是否可以容纳我
            if (min > ObstacleRange[i])
            {
                min = ObstacleRange[i];
                nearest = i;
            }
        }
        return nearest;
        
    }
    float FightDesired()
    {
        float fight = HP / allHP * HP_affect_Fight;
        if (isWatching)
        {
            if (EnemyRange > AttackRange)
            {
                
                fight+= 1/EnemyRange*Range_affect;
            }
            else
            {
                fight += 1/AttackRange* Range_affect;
            }
        }
        else
        {
            fight = 0;
        }
        if (inShootingAngleRange)
        {
            fight += Range_affect;
        }
        if (m_Body.Cartridging)
        {
            fight -= Desired_affect;
        }
     ///   Debug.Log("FightDesired: "+fight);
        return fight;
    }
    //float CartridgeDesired()
    //{
    //    float Cartridge = 0;
    //    Cartridge = 1-(float)m_Body.BulletsInGun/(float)m_Body.BulletsInBox;
    //    Debug.Log("CartridgeDesired: "+Cartridge);
    //    return Cartridge;

    //}
    float HideDesired()
    {
        float hide =1- HP / allHP * HP_affect_Hide;
        if (Attacked)
        {
            hide += 3*Desired_affect;
            if (isWatching)
            {
                hide -= EnemyRange * Range_affect;
            }
            else
            {
                hide +=Desired_affect;
            }
           
            if (hide <= 0)
            {
                hide = 0;
            }
            hide += 1/ObstacleRange[NearestObstacle] * Range_affect;
        }
        if (m_Body.Cartridging)
        {
            hide +=   Desired_affect;
        }

        //   Debug.Log("HideDesired: "+hide);
        return hide;
    }
    float FindDesired()
    {
        float find=0;
        if (Waring)
        {
            find = 0.4f;
        }
        if (isWatching&&!inShootingAngleRange)
        {
            find = 0.6f;
        }
        //Debug.Log(m_Body.Shooting);
        if (m_Body.Shooting&& !inShootingAngleRange)
        {
            
            find = isFight + 0.1f;
        }
        if (m_Body.Cartridging)
        {
            find += Desired_affect;
        }
        //  Debug.Log("FindDesired: " + find);
        return find;
    }
    float TraceDesired()
    {
        float Trace = 0;

        if (EnemyRange>=AttackRange*4/5&&EnemyRange<=AttackRange*2)
        {
            Trace += Desired_affect * 2;
        }
      //  Debug.Log("TraceDesired: " + Trace);
        return Trace;
    }
}
