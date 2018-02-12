using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class RushingManager : MonoBehaviour
    {

        // Use this for initialization
        [SerializeField] float rushingTime;
        [SerializeField] float m_RushingSpeed;
        [SerializeField] float m_rushingCold;
        ThirdPersonCharacter m_Character;
        float statTime;
        private bool Rushing;
        Rigidbody m_Rigidbody;
    [SerializeField]MeshRenderer[] head;
    float radius;
    float height;
        void Start()
        {
            Rushing = false;
        radius = GetComponent<CapsuleCollider>().radius;
        height = GetComponent<CapsuleCollider>().height;
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            m_rushingCold -= Time.deltaTime;
        }
        public void m_Rushing()
        {
            if (Rushing == false)
            {
                if (m_rushingCold <= 0)
                {
                    Rushing = true;
                    statTime = 0;
                //GetComponent<Collider>().isTrigger = true;
                GetComponent<CapsuleCollider>().radius = 0.1f;
                GetComponent<CapsuleCollider>().height = 0.3f;
                    GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                foreach(var h in head)
                {
                    h.enabled = false;
                }
                     NotificationCenter.DefaultCenter().PostNotification(this, "RushingP");
                     
                     m_Rigidbody.useGravity = false;
                    m_rushingCold = rushingTime * 2f;
                }
            }
            else
            {
            
            
                m_Rigidbody.velocity = transform.forward * m_RushingSpeed;
                statTime += Time.deltaTime;
           


            }
            if (statTime >= rushingTime)
            {
                Rushing = false;
            //GetComponent<Collider>().isTrigger = false;
            GetComponent<CapsuleCollider>().radius = radius;
            GetComponent<CapsuleCollider>().height = height;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            foreach (var h in head)
            {
                h.enabled = true;
            }
            m_Character.m_rushing = false;
                m_Rigidbody.useGravity = true;
                
            }
        }
    }
