using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TotallyNotEvil.Sequences
{
    public class TriggerSequence : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggerAction;
        [SerializeField] private bool hasBeenTrigggered;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<IPossessable>() != null)
            {
                if (collision.gameObject.GetComponent<IPossessable>().IsPossessed && !hasBeenTrigggered)
                {
                    triggerAction.Invoke();
                    hasBeenTrigggered = true;
                }
            }
        }
    }
}