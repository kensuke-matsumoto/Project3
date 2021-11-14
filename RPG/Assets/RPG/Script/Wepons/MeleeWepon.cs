using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{

    public class MeleeWepon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform rootTransform;
        }

       public LayerMask targetLayers;
       public int damage;
       public AttackPoint[] attackPoints = new AttackPoint[0];

       private bool m_IsAttack = false;
       private Vector3[] m_OriginAttackPos;
       private RaycastHit[] m_rayCastHitCash = new RaycastHit[32];
       private GameObject m_Owner;

       private void FixedUpdate()
       {
           if(m_IsAttack)
           {
               for (int i = 0; i < attackPoints.Length; i++)
               {
                   AttackPoint ap = attackPoints[i];
                   Vector3 WorldPos = ap.rootTransform.position + ap.rootTransform.TransformVector(ap.offset);//need to change TransformDirection?

                   //Debug.Log(WorldPos);

                   Vector3 attackVector = (WorldPos - m_OriginAttackPos[i]).normalized *0.1f;

                   Ray ray = new Ray(WorldPos, attackVector);
                   Debug.DrawRay(WorldPos, attackVector, Color.red, 4.0f);

                   int contacts = Physics.SphereCastNonAlloc(
                       ray,
                       ap.radius,
                       m_rayCastHitCash,
                       attackVector.magnitude,
                       ~0,
                       QueryTriggerInteraction.Ignore);
                    
                   
                    for (int k = 0; k < contacts; k++)
                    {
                        Collider collider = m_rayCastHitCash[k].collider;
                        if(collider != null)
                        {
                           CheckDamage(collider, ap); 
                        }
                    }

                   m_OriginAttackPos[0] = WorldPos;
               }
           }
       }
       private void CheckDamage(Collider other, AttackPoint ap)
       {
           Damageable damageable = other.GetComponent<Damageable>();

           if((targetLayers.value & (1 << other.gameObject.layer)) == 0)
           {
               return;
           }
           //Debug.Log("We are hitting correct layer");
           //Debug.Log(other.gameObject.layer);


           if(damageable != null)
           {
               Damageable.DamageMessage data;
               data.amount = damage;
               data.damager = this;
               data.damageSource = m_Owner.transform.position;
               damageable.ApplyDamage(data);
           }

       }
       public void SetOwner(GameObject owner)
       {
           m_Owner = owner ;
       }

       public void BeginAttack()
        {
            m_IsAttack = true;  
            m_OriginAttackPos = new Vector3[attackPoints.Length];
            for(int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint ap = attackPoints[i];
                m_OriginAttackPos[i] = ap.rootTransform.position + ap.rootTransform.TransformDirection(ap.offset); //what difference betw TransformDirection and TransformVector ?
            }

        } 
        public void EndAttack()
        {
            m_IsAttack = false;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach(AttackPoint attackPoint in attackPoints)
            {
                if(attackPoint.rootTransform !=null)
                {

                    Vector3 worldPosition = attackPoint.rootTransform.TransformVector(attackPoint.offset);
                   Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                   Gizmos.DrawSphere(attackPoint.rootTransform.position + worldPosition, attackPoint.radius); 
                }

            }
        }
#endif
    
    
    }


}
