using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG
{
   public partial class Damageable : MonoBehaviour
     {
      
       [Range(0, 360f)]
       public float hitAngle = 360.0f;
      public int maxHitPoint;
      public int CurrentHitPoint {get; private set; }

      public List<MonoBehaviour> onDamageMessageReceivers;
     

      private void Awake()
      {
         CurrentHitPoint = maxHitPoint;
      }

       public void ApplyDamage(DamageMessage data)
        {
           if(CurrentHitPoint <= 0)
           {
              return;
           }
           
           Vector3 positionToDamager = data.damageSource - transform.position;

           positionToDamager.y = 0;

           if(Vector3.Angle(transform.forward, positionToDamager) > hitAngle * 0.5 )
           {
              return;
           }
           CurrentHitPoint -= data.amount;

           var messageType = CurrentHitPoint <= 0 ? MessageType.DEAD : MessageType.DAMAGED;
           //Debug.Log(CurrentHitPoint);
           for(int i = 0; i < onDamageMessageReceivers.Count; i++)
           {
              var receiver = onDamageMessageReceivers[i] as IMessageReceiver; // should be Confirmed
              receiver.OneReceiveMessage(messageType);
            
           }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
           UnityEditor.Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
           Vector3 rotatedForward = 
                Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * transform.forward;

          UnityEditor.Handles.DrawSolidArc(transform.position,
                                           transform.up,
                                           rotatedForward,
                                           hitAngle,
                                           1.0f);
         

        }
#endif
   
     }

}


