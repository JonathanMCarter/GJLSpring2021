using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class DeathObject : MonoBehaviour
    {
        public string cause;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IPossessable>() != null)
            {
                Death.PlayerHasDeath(cause);
            }
        }
    }
}