using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class BanditBehaiviour : MonoBehaviour
    {
      public float detectionRadius = 10.0f;
      public float detectionAngle = 90.0f; 
        private void Start()
        {
          Debug.Log(PlayerController.Instance);
        }
        private void Update()
        {
          LookForPlayer();

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
          //Debug.Log("toPlayer vector" + toPlayer);
          //Debug.Log("transform.forward" + transform.forward);
          //Debug.Log("Dot value" + Vector3.Dot(toPlayer.normalized, transform.forward));
          //Debug.Log("Cos value" + Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad));

          if(toPlayer.magnitude <= detectionRadius)
          {
            if(Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
              Debug.Log("Player has been detected !!");

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
   

