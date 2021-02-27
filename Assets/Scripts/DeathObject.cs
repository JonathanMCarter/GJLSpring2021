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
                if (collision.GetComponent<IPossessable>().IsPossessed)
                    Death.PlayerHasDeath(cause);
                else
                    collision.gameObject.SetActive(false);
            }
        }
    }
}