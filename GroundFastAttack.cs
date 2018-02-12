using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFastAttack : GroundAttack {

    // Use this for initialization
    float waitingBullet;
    [SerializeField] float Shootingmaxtime = 3;
    [SerializeField] float Speed = 13;
    [SerializeField] const float harmful = 1;
    [SerializeField] const float Catridge = 2;
    [SerializeField] ParticleSystem FireParticle;
    bool Shooting;
    public float Shootingtime;
    public float Catridgetime;
    override
        public void Init()
    {

        Shootingtime = 0;
        Shooting = true;
     
    }
    override
        public void Fight()
    {
        if (Vector3.Distance(Player.position, transform.position) < m_self.TransformationArea && Shooting)
        {
            Shootingtime += Time.deltaTime;

            if (Shootingtime <= Shootingmaxtime)
            {
                if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    m_animator.SetTrigger("Attack");
                    NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletFireP", FireParticle);
                }
                
                waitingBullet += Time.deltaTime * Speed;
                if (waitingBullet >= 1)
                {
                    int bullets = (int)waitingBullet;
                    waitingBullet -= bullets;
                    //发出bullets发子弹
                    PlayerBulletManager.Bullets tempbullets = new PlayerBulletManager.Bullets();
                    tempbullets.StartPosition = transform.position;
                    tempbullets.localdirection = transform.forward;
                    tempbullets.harmful = harmful;
                    tempbullets.Bulletstype = PlayerBulletManager.bulletstype.EnemyFast;
                    for (int i = 0; i < bullets; i++)
                    {
                        //Debug.Log("EnemyFast");

                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);
                        tempbullets.localdirection = transform.right;
                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);
                        tempbullets.localdirection = -transform.right;
                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);
                        tempbullets.localdirection = -transform.forward;
                        NotificationCenter.DefaultCenter().PostNotification(this, "PlayerBullet", tempbullets);

                    }
                }

            }
            else
            {
                Shooting = false;
                Shootingtime = 0;
            }
        }
        else
        {
            Catridgetime += Time.deltaTime;
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                m_animator.SetTrigger("Idle");
                NotificationCenter.DefaultCenter().PostNotification(this, "FastBulletNotFireP", FireParticle);
            }
            if (Catridgetime >= Catridge)
            {
                Shooting = true;
                Catridgetime = 0;
            }
        }
    }
}
