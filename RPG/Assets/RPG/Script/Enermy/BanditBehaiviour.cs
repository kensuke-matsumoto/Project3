using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG
{
    public class BanditBehaiviour : MonoBehaviour
    {
      public float detectionRadius = 10.0f;
      public float detectionAngle = 90.0f; 
      public float timeToStopPursuit = 2.0f;
      public float timeToWaitOnPursuit = 2.0f;
      private PlayerController m_Target;
      private NavMeshAgent m_NavMeshAgent; 
      private Animator m_Animator;
      private float m_TimeSinceLostTarget = 0;
      private Vector3 m_OriginPosition;
       private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
       private readonly int m_HashNearBase = Animator.StringToHash("NearBase");    

        private void Awake()
        {
          m_NavMeshAgent = GetComponent<NavMeshAgent>();
          m_Animator = GetComponent<Animator>();        
          m_OriginPosition = transform.position;
          Debug.Log("Hello !");
          

        }
        private void Update()
        {
          var target = LookForPlayer();
          if(m_Target == null)
          {
            if(target != null)
            {
              m_Target = target;

            }
          }
          else
          {  
            m_NavMeshAgent.SetDestination(m_Target.transform.position);
            m_Animator.SetBool(m_HashInPursuit, true);

            if(target == null) 
            {
              m_TimeSinceLostTarget += Time.deltaTime;
              if(m_TimeSinceLostTarget >= timeToStopPursuit)
              {
                m_Target = null;
                m_NavMeshAgent.isStopped = true;
                m_Animator.SetBool(m_HashInPursuit, false);
                StartCoroutine(WaitOnPursuit());
             


              }
            } 
            else
            {
              m_TimeSinceLostTarget = 0;

            }     
           
          }
          Vector3 toBase = m_OriginPosition - transform.position;
          toBase.y = 0;
          m_Animator.SetBool(m_HashNearBase, toBase.magnitude < 0.01f);
          

        }
        private IEnumerator WaitOnPursuit()
        {
          yield return new WaitForSeconds(timeToWaitOnPursuit);
          m_NavMeshAgent.isStopped = false;
          m_NavMeshAgent.SetDestination(m_OriginPosition);



        }

        private PlayerController LookForPlayer()
        {
          if(PlayerController.Instance == null)
          {
            return null;
          }
          Vector3 enermyPosition = transform.position;
          Vector3 toPlayer = PlayerController.Instance.transform.position - enermyPosition; // target(player) - this object(enermy) in this case 
          toPlayer.y = 0;
         

          if(toPlayer.magnitude <= detectionRadius)
          {
            if(Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
              return PlayerController.Instance;

            }
            //Debug.Log("Detecting Player!!");            
          }
          else
          {
            //Debug.Log("Where are you ?"); 


          }

          return null;


        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
          Color c = new Color(0, 0, 0.7f, 0.4f);
          UnityEditor.Handles.color = c;

          Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0)
                                                               * transform.forward;

         //Debug.Log("rotatedForward" + rotatedForward);

         UnityEditor.Handles.DrawSolidArc(transform.position,
                                          Vector3.up, rotatedForward,
                                          detectionAngle,
                                          detectionRadius);
        }
#endif
    }
}
   

