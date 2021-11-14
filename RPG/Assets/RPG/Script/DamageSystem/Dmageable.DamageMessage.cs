using UnityEngine;
using System.Collections;

namespace RPG
{
    public partial class Damageable : MonoBehaviour
    {
       public struct DamageMessage
       {
           public MonoBehaviour damager;
           public int amount;
           public Vector3 damageSource;

       } 

    }
}