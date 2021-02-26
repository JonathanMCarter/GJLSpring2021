using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Lift : MonoBehaviour
    {
        [SerializeField] private bool isInLift;
        [SerializeField] internal bool inputPressed;
        [SerializeField] private GameObject toGoTo;
        [SerializeField] private Animator liftUI;

        private GameObject toTeleport;
        private bool isCoR;


        private void OnDisable()
        {
            StopAllCoroutines();
        }


        private void Update()
        {
            if (isInLift && inputPressed && !isCoR && toTeleport)
            {
                StartCoroutine(TeleportCo());
            }
        }


        private IEnumerator TeleportCo()
        {
            isCoR = true;
            liftUI.SetBool("IsMoving", true);
            yield return new WaitForSeconds(1f);
            toTeleport.transform.position = toGoTo.transform.position;
            yield return new WaitForSeconds(1f);
            isCoR = false;
        }
    }
}