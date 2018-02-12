using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneManager : MonoBehaviour {
    public class Fortune_base
    {
        public Vector3 S;
        public Vector3 Direction;
        public GameObject fortune;    
        public float Rescue;
        public float AddAttack;
        public bool isDead=false;
        public float Speed=30;
    }
    // Use this for initialization
    List<Fortune_base> fortune = new List<Fortune_base>();

    GameObject Player;
    [SerializeField] GameObject RedBuff;
    [SerializeField] GameObject BlueBuff;
	void Start () {
        Player = GameObject.Find("SUNman");
        NotificationCenter.DefaultCenter().AddObserver(this, "InstantiateFortune");
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var f in fortune)
        {
            Vector3 localDirection = f.fortune.transform.InverseTransformDirection(f.Direction).normalized;
            f.fortune.transform.Translate(localDirection * f.Speed*Time.deltaTime);
            f.Direction = Player.transform.position - f.fortune.transform.position;
            f.Speed = Vector3.Distance(f.fortune.transform.position, Player.transform.position);
            f.Speed = f.Speed * f.Speed;
            if (Vector3.Distance(f.fortune.transform.position, Player.transform.position) < 0.5f || (Vector3.Dot(f.fortune.transform.position - Player.transform.position, f.S - Player.transform.position) < 0))
            {
                f.isDead = true;
                NotificationCenter.DefaultCenter().PostNotification(this, "GetFortune", f);
            }
        }
        for (int i = fortune.Count - 1; i >= 0; i--)
        {
            if (fortune[i].isDead)
            {
                Destroy(fortune[i].fortune);
                fortune.RemoveAt(i);
            }
        }
	}
    void InstantiateFortune(Notification noti)
    {
        Fortune_base fb = (Fortune_base)noti.data;
        fb.Direction = Player.transform.position - fb.S;
        if (fb.Rescue > 0)
        {
            Debug.Log("RedBuff");
            fb.fortune = Instantiate(RedBuff);
            
        }
        else
        {
            Debug.Log("BlueBuff");
            fb.fortune = Instantiate(BlueBuff);
        }
        
        fb.fortune.transform.position = fb.S;
        fb.Direction = Player.transform.position - fb.S;
        fortune.Add(fb);
    }
}
