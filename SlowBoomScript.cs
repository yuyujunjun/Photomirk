using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBoomScript : MonoBehaviour {

    
  
    float St;
    bool hasSpawnSphere;
    float Ss;
	void Start () {
        St = 0;
        hasSpawnSphere = false;
        DestroyObject(gameObject, 3.1f);	
        
	}
	
	// Update is called once per frame
	void Update () {
        St += Time.deltaTime;
        if (St >= 1&&!hasSpawnSphere)
        {
            hasSpawnSphere = true;
            gameObject.AddComponent<SphereCollider>();
            Ss = 0;
            GetComponent<SphereCollider>().radius = 3;
            PlayerBulletManager.Bullets tempb = new PlayerBulletManager.Bullets();
            tempb.EndPosition = transform.position;
            tempb.Bulletstype = PlayerBulletManager.bulletstype.EnemySlow;
            tempb.harmful = 30;
            NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBeAttacked", tempb);

        }
        if (hasSpawnSphere)
        {
            Ss += Time.deltaTime;
            if (Ss >= 0.1)
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
	}
    
}
