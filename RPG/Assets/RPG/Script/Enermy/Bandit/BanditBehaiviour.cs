using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG
{
    public class BanditBehaiviour : MonoBehaviour, IMessageReceiver
    {
      public PlayerScanner playerScanner;
      public float timeToStopPursuit = 2.0f;
      public float timeToWaitOnPursuit = 2.0f;
      public float attackDistance = 1.1f;
      public bool HasFollowTarget
      {
        get{return m_FollowTarget != null;}
      }
      private PlayerController m_FollowTarget;
      private EnermyController m_EnermyController;
     
      private Animator m_Animator;
      private float m_TimeSinceLostTarget = 0;
      private Vector3 m_OriginPosition;
      private Quaternion m_OriginRotation;
       private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
       private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
      private readonly int m_HashAttack = Animator.StringToHash("Attack");      

        private void Awake()
        {
          m_EnermyController = GetComponent<EnermyController>();
          m_Animator = GetComponent<Animator>(); 
          m_OriginRotation = transform.rotation;       
          m_OriginPosition = transform.position;
          //Debug.Log("Hello !");


        }
        private void Update()
        {
          GuardPosition();
        }
        private void GuardPosition()
        {
          var detectedTarget = playerScanner.Detect(transform);
          bool hasDetectedTarget = detectedTarget != null;

          if(hasDetectedTarget){ m_FollowTarget = detectedTarget; }

          if(HasFollowTarget)
          {
            AttackOrFollowTarget();
            if(hasDetectedTarget)
            {
              m_TimeSinceLostTarget = 0;
            }
            else
            {
             StopPursuit(); 
            }
          }       
          CheckIfNearBase();         

        }
        public void OneReceiveMessage(MessageType type)
        {
          Debug.Log("Bandit Behaiviour : " + type);

        }
        private void AttackOrFollowTarget()
        {
          Vector3 toTarget = m_FollowTarget.transform.position - transform.position;
            if(toTarget.magnitude <= attackDistance)
            {
              AttackTarget(toTarget);       
            }
            else
            {
              FollowTarget();              
            }
        }
        private void StopPursuit()
        {
          m_TimeSinceLostTarget += Time.deltaTime;
              if(m_TimeSinceLostTarget >= timeToStopPursuit)
              {
                m_FollowTarget = null;
                
                m_Animator.SetBool(m_HashInPursuit, false);
                StartCoroutine(WaitBeforeReturn());           
              }         
        }
        private void AttackTarget(Vector3 toTarget)
        {
          var toTargetRotation = Quaternion.LookRotation(toTarget);
              transform.rotation = Quaternion.RotateTowards(transform.rotation, toTargetRotation, 180 * Time.deltaTime);
              m_EnermyController.StopFollowTarget();
             
              m_Animator.SetTrigger(m_HashAttack);
        }
        private void FollowTarget()
        {
          m_Animator.SetBool(m_HashInPursuit, true); 
          m_EnermyController.FollowTarget(m_FollowTarget.transform.position);
        }
        private void CheckIfNearBase()
        {
          Vector3 toBase = m_OriginPosition - transform.position;
          toBase.y = 0;

          bool nearBase = toBase.magnitude < 0.01f ;
          m_Animator.SetBool(m_HashNearBase, nearBase);

          if(nearBase)
          {
            Quaternion targetRotation =Quaternion.RotateTowards(transform.rotation, m_OriginRotation, 360 * Time.deltaTime);
            transform.rotation = targetRotation;
          }        
        }
        private IEnumerator WaitBeforeReturn()
        {
          yield return new WaitForSeconds(timeToWaitOnPursuit);
          m_EnermyController.FollowTarget(m_OriginPosition);      
        }
        

       

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
          Color c = new Color(0, 0, 0.7f, 0.4f);
          UnityEditor.Handles.color = c;

          Vector3 rotatedForward = Quaternion.Euler(0, -playerScanner.detectionAngle * 0.5f, 0)
                                                               * transform.forward;

         //Debug.Log("rotatedForward" + rotatedForward);

         UnityEditor.Handles.DrawSolidArc(transform.position,
                                          Vector3.up, rotatedForward,
                                          playerScanner.detectionAngle,
                                          playerScanner.detectionRadius);

         UnityEditor.Handles.DrawSolidArc(transform.position,
                                          Vector3.up, rotatedForward,
                                          360,
                                          playerScanner.meleeDetectionRadius);
        }
#endif
    }
}
   

