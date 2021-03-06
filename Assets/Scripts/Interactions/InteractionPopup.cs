using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class InteractionPopup : MonoBehaviour
    {
        [SerializeField] private GameObject prompt;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")) && collision.GetComponent<IPossessable>().IsPossessed)
            {
                prompt.SetActive(true);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")) && collision.GetComponent<IPossessable>().IsPossessed)
            {
                prompt.SetActive(false);
            }
        }
    }
}