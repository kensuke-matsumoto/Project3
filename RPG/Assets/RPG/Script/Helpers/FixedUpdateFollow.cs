using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG
{
    public class FixedUpdateFollow : MonoBehaviour
 {
     public Transform toFollow;
   

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = toFollow.position;
        transform.rotation = toFollow.rotation;
        
    }
 }

}

