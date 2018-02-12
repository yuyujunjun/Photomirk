using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour {

    // Use this for initialization

    [SerializeField] ParticleSystem RushingParticles;
    [SerializeField] ParticleSystem Jump1Particles;
    [SerializeField] ParticleSystem Jump2Particles;
    [SerializeField] ParticleSystem PlayerShootingParticles;
    [SerializeField] ParticleSystem HotReactionParticles;//枪械过热
    [SerializeField] ParticleSystem WhenBWChangeParticles;
    [SerializeField] GameObject HitParticles;
    [SerializeField] GameObject DeadParticles;
    [SerializeField] GameObject HitObstacleParticles;
    [SerializeField] GameObject SlowBulletBoomParticles;

    
    void Start () {
        
        NotificationCenter.DefaultCenter().AddObserver(this,"RushingP");
        NotificationCenter.DefaultCenter().AddObserver(this, "Jump1P");
        NotificationCenter.DefaultCenter().AddObserver(this, "Jump2P");
        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerShootingP");
        NotificationCenter.DefaultCenter().AddObserver(this, "HotReactionP");
        NotificationCenter.DefaultCenter().AddObserver(this, "HitEnemyP");
        NotificationCenter.DefaultCenter().AddObserver(this, "DeadEnemyP");
        NotificationCenter.DefaultCenter().AddObserver(this, "HitObstacleP");
        NotificationCenter.DefaultCenter().AddObserver(this, "SlowBulletBoomP");
        NotificationCenter.DefaultCenter().AddObserver(this, "FastBulletFireP");
        NotificationCenter.DefaultCenter().AddObserver(this, "FastBulletNotFireP");
        NotificationCenter.DefaultCenter().AddObserver(this, "whenBWChange");
    }
	
	// Update is called once per frame
	void Update () {
       
	}
   
    void RushingP(Notification notification)
    {
        RushingParticles.Play();
        //Debug.Log("Received notification from: " + notification.sender);
        //if (notification.data == null)
        //    Debug.Log("And the data object was null!");
        //else
        //{
        //    Debug.Log("And it included a data object: " + notification.data);
        //    Debug.Log(notification.name);
        //    GameObject b = (GameObject)notification.data;
        //    Debug.Log(b.transform.position);


        //}

    }
    void Jump1P(Notification notification)
    {
        
        Jump1Particles.Play();
        

    }
    void Jump2P(Notification notification)
    {
        Jump2Particles.Play();
        //Debug.Log("Received notification from: " + notification.sender);
        //if (notification.data == null)
        //    Debug.Log("And the data object was null!");
        //else
        //{
        //    Debug.Log("And it included a data object: " + notification.data);
        //    Debug.Log(notification.name);
        //    GameObject b = (GameObject)notification.data;
        //    Debug.Log(b.transform.position);


        //}

    }
    void PlayerShootingP()
    {
        PlayerShootingParticles.Play();
    }
    void HotReactionP()
    {
        HotReactionParticles.Play();
    }
    void HitEnemyP(Notification noti)
    {
        Vector3 position = (Vector3)noti.data;
        GameObject tmp=Instantiate(HitParticles);
        //tmp.AddComponent<HitDestory>();
        tmp.transform.position = position;
    }
    void DeadEnemyP(Notification noti)
    {
        Vector3 position = (Vector3)noti.data;
        GameObject tmp = Instantiate(DeadParticles);
        tmp.transform.position = position;
    }
    void HitObstacleP(Notification noti)
    {
        Vector3 position = (Vector3)noti.data;
        GameObject tmp = Instantiate(HitObstacleParticles);
        //tmp.AddComponent<HitDestory>();
        tmp.transform.position = position;
    }
    void SlowBulletBoomP(Notification noti)
    {
        PlayerBulletManager.Bullets b = (PlayerBulletManager.Bullets)noti.data;
        GameObject tmp = Instantiate(SlowBulletBoomParticles);
        tmp.transform.position = b.EndPosition;
    }
    void FastBulletFireP(Notification  noti)
    {
        ParticleSystem part = (ParticleSystem)noti.data;
        if (!part.isPlaying)
        {
            part.Play();
        }
    }
    void FastBulletNotFireP(Notification noti)
    {
        ParticleSystem part = (ParticleSystem)noti.data;
        if (!part.isStopped)
        {
            part.Stop();
        }
    }
    void whenBWChange()
    {
        WhenBWChangeParticles.Play();
    }
}
