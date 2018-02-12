using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBulletFly : SlowBullet {

    // Use this for initialization
    Robot parent_body;
    void Start()
    {
        parent_body = GetComponentInParent<Robot>();
    }
    // Update is called once per frame
    void Update () {
		
	}
    override public void Shooting()
    {

        parent_body.Shooting = false;
        parent_body.Cartridgetime = 0;

        PlayerBulletManager.Bullets tempbullets = new PlayerBulletManager.Bullets();
        tempbullets.StartPosition = transform.position;
        tempbullets.localdirection = transform.forward;
        tempbullets.harmful = parent_body.harmful;
        tempbullets.Bulletstype = PlayerBulletManager.bulletstype.EnemySlow;


        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);

    }
}
