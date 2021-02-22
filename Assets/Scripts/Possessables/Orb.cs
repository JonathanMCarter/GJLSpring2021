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
        }


        private void OnCollisionStay2D(Collision2D collision)
        {
            if (canPossess)
            {
                // making a variable so it is easier to read (I use it like 3/4 times here so made sense).
                IPossessable _poss = collision.gameObject.GetComponent<IPossessable>();

                if (_poss != null)
                {
                    if (!_poss.IsPossessed)
                    {
                        Debug.Log("Possess Me!");
                        _poss.IsPossessed = true;

                        // set the player to control the hit IPossessable object.
                        FindObjectOfType<PlayerController>().SetAm(_poss);

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

            // stops the old object from being defined as possessed.
            was.IsPossessed = false;

            canPossess = true;
        }
    }
}