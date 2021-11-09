using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class PlayerInput : MonoBehaviour
{
    // it's OK to change Vector3. it might be useful
    //private Vector2 m_Movement;
    //public Vector2 MoveInput

    private Vector3 m_Movement;
    public Vector3 MoveInput
    {
        get
        {
            return m_Movement;
        }
    }

    public bool IsMoveInput
    {
        get
        {
            return !Mathf.Approximately(MoveInput.magnitude, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Movement.Set(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

        
    }
}

}


