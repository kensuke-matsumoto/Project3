using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    

  
    // Update is called once per frame
    void LateUpdate()
    {
        if(!target)
        {
            return;
        }

        float currentRotationAngle = transform.eulerAngles.y;
        float wantedRotationAngle = target.eulerAngles.y;

        // a + (b -a) * t
        //a: CURRENT degree 
        //b: WANTED degree
        //c:Constat(0~1.0) frequency
        // Important!!
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle,
            wantedRotationAngle,
            0.5f);
        
        // Current Rotation 45

        transform.position = new Vector3(
            target.position.x,
            5.0f,
            target.position.z);

        // currentRtationAngle degree rotation around Y axis
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // rotate vector forward currentRotationAngle  angle degreed aroud Y axis
        Vector3 rotatedPosition = currentRotation * Vector3.forward;

        transform.position -= rotatedPosition * 10;
        transform.LookAt(target);

       



    }
  
}
