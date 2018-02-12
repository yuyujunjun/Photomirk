using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyManager : MonoBehaviour {

    // Use this for initialization
    float StartTime;
    float EndTime = 10;
    int numberOFenemy=0;
    [SerializeField] private string CurrentSceneName;
	void Start () {
        numberOFenemy = transform.childCount;
        NotificationCenter.DefaultCenter().AddObserver(this, "RobotDead");
	}
	
	// Update is called once per frame
	void Update () {
        if (numberOFenemy <= 0)
        {
            StartTime += Time.deltaTime;
            if (StartTime > EndTime)
            {
                
                NotificationCenter.DefaultCenter().PostNotification(this,"UnloadEnemy",CurrentSceneName);
            }
        }
        
	}
    void RobotDead(Notification noti)
    {
        numberOFenemy--;
        if (numberOFenemy <= 0)
        {
            StartTime = 0;
        }
    }
}
