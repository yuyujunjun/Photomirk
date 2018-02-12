using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBulletGround : SlowBullet {

    // Use this for initialization
    [SerializeField] float harmful;
    [SerializeField] Transform EnemyTarget;
	void Start () {
        EnemyTarget = GameObject.Find("SUNman").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    override public void Shooting()
    {

        

        PlayerBulletManager.Bullets tempbullets = new PlayerBulletManager.Bullets();
        tempbullets.StartPosition = transform.position;
        tempbullets.localdirection = EnemyTarget.position - transform.position;
        tempbullets.harmful =harmful;
        tempbullets.Bulletstype = PlayerBulletManager.bulletstype.EnemySlow;


        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);

    }
}
