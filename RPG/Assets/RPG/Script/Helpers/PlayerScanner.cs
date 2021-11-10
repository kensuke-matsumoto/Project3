using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

[System.Serializable]
public class PlayerScanner 
{
     public float detectionRadius = 10.0f;
      public float detectionAngle = 90.0f; 
    public PlayerController Detect(Transform detector)
        {
          if(PlayerController.Instance == null)
          {
            return null;
          }
          Vector3 enermyPosition = detector.position;
          Vector3 toPlayer = PlayerController.Instance.transform.position - detector.position; // target(player) - this object(enermy) in this case 
          toPlayer.y = 0;
         

          if(toPlayer.magnitude <= detectionRadius)
          {
            if(Vector3.Dot(toPlayer.normalized, detector.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
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
}
