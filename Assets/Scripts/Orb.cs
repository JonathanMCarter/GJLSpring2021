using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Orb : MonoBehaviour
    {
        // assinged to avoid GD
        private WaitForSeconds wait;
        [SerializeField] private bool canPossess;


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            wait = new WaitForSeconds(.5f);
            GetComponentsInChildren<CircleCollider2D>()[1].enabled = false;
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (canPossess)
            {
                Debug.Log("can possess");
                if (collision.GetComponent<IPossessable>() != null)
                {
                    if (!collision.GetComponent<IPossessable>().IsPossessed)
                    {
                        Debug.Log("Possess Me!");
                        collision.GetComponent<IPossessable>().IsPossessed = true;

                        // set the player to control the hit IPossessable object.
                        FindObjectOfType<PlayerController>().SetAm(collision.GetComponent<IPossessable>());

                        // disables the orb
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Im Possessed!");
                    }
                }
            }
        }


        /// <summary>
        /// Fire!!!! - just runs the co, that stops the player ending up in the same obj as they were in.
        /// </summary>
        public void Yeet(IPossessable was)
        {
            StopAllCoroutines();
            StartCoroutine(ExitHostCooldown(was));
        }


        /// <summary>
        /// (Co) - gives a slight cooldown between exiting a person/object/thing.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExitHostCooldown(IPossessable was)
        {
            canPossess = false;
            yield return wait;
            GetComponentsInChildren<CircleCollider2D>()[1].enabled = true;
            was.IsPossessed = false;
            canPossess = true;
        }
    }
}