using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour {

    // Use this for initialization
   [SerializeField] float HP;
    [SerializeField] Transform ParallelLight;
    public PlayerBulletManager.bulletstype PhotoOrMirk;
    [SerializeField]bool Inlight;
    [SerializeField] bool InRoundLight;
    float HPLimited;
    float AddAttack;
    [SerializeField] List<Material> T = new List<Material>();
    private float stateQuan = 0;
    [SerializeField] float transformSpeed = 10;
    void Start () {
        Inlight = false;
        HP = 100;
        HPLimited = 120;
        AddAttack = 0;
        NotificationCenter.DefaultCenter().AddObserver(this,"GetFortune");
        NotificationCenter.DefaultCenter().AddObserver(this, "BrainBeAttacked");
        PhotoOrMirk = PlayerBulletManager.bulletstype.Mirk;
        for (int i = 0; i < T.Count; i++)
        {
            T[i].SetFloat("_bw",stateQuan);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!InRoundLight)//如果没有受到圆台光照射，就判断平行光
        {
            Inlight = IsLight();
        }
        else
        {
            Inlight = true;
        }
        //Debug.Log(Inlight);
        if (!Inlight)
        {
            //增光
            stateQuan += transformSpeed * Time.deltaTime;
            if (stateQuan >= 1)
            {
                stateQuan = 1;
                PhotoOrMirk = PlayerBulletManager.bulletstype.Photo;
            }
            
                for (int i = 0; i < T.Count; i++)
                {
                    T[i].SetFloat("_bw", stateQuan);
                }
            

        }
        else
        {
            //减光
            stateQuan -= transformSpeed * Time.deltaTime;
            if (stateQuan <=0)
            {
                stateQuan = 0;
                PhotoOrMirk = PlayerBulletManager.bulletstype.Mirk;
            }
           
                for (int i = 0; i < T.Count; i++)
                {
                    T[i].SetFloat("_bw", stateQuan);
                }
            
        }
        
	}
    void GetFortune(Notification noti)
    {
        FortuneManager.Fortune_base fb = (FortuneManager.Fortune_base)noti.data;
        HP += fb.Rescue;
        if (HP >= HPLimited)
        {
            HP = HPLimited;
        }
        AddAttack += fb.AddAttack;
        if (AddAttack >= 0.5)
        {
            AddAttack = 0;
            NotificationCenter.DefaultCenter().PostNotification(this,"PlayerFightingAddAttack");
        }
    }
    void BrainBeAttacked(Notification noti)
    {
        PlayerBulletManager.Bullets b = (PlayerBulletManager.Bullets)noti.data;
        float harmful=0;
        if (b.Bulletstype == PlayerBulletManager.bulletstype.EnemyFast)
        {
            harmful= b.harmful;
        }
        else
        {
            harmful = (1- Vector3.Distance(transform.position, b.EndPosition)/5) * b.harmful;
            if (harmful <= 0) harmful = 0f;
        }
        HP -= harmful;
    }
    bool IsLight()
    {
        bool Photo = false;
        LayerMask lm = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Level")|1<<LayerMask.NameToLayer("Light"));
        Ray light=new Ray(transform.position,-ParallelLight.transform.forward);
        Debug.DrawRay(transform.position, -ParallelLight.transform.forward,Color.blue,100);
        RaycastHit target;
        if (Physics.Raycast(light, out target,Mathf.Infinity , lm))
        {
           // Debug.Log("isawef");
        }
        else
        {
            Photo = true;
        }
        return Photo;
    }
    private void OnTriggerEnter(Collider other)
    {
        InRoundLight = false;
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Light")))
        {
            InRoundLight=other.GetComponent<RoundTableLight>().IsOn;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        InRoundLight = false;
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Light")))
        {
            InRoundLight = other.GetComponent<RoundTableLight>().IsOn;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Light")))
        {
            InRoundLight = false;
        }
    }
}
