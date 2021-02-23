using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class InteractionPopup : MonoBehaviour
    {
        private SpriteRenderer promptSprite;


        private void Start()
        {
            promptSprite = GetComponent<SpriteRenderer>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")) && collision.GetComponent<IPossessable>().IsPossessed)
            {
                promptSprite.enabled = true;
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")) && collision.GetComponent<IPossessable>().IsPossessed)
            {
                promptSprite.enabled = false;
            }
        }
    }
}