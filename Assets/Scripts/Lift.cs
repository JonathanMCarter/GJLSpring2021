using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace TotallyNotEvil
{
    public class Lift : MonoBehaviour
    {
        [SerializeField] private bool isInLift;
        [SerializeField] internal bool inputPressed;
        [SerializeField] private GameObject toGoTo;
        [SerializeField] private Animator liftUI;

        [SerializeField] private GameObject toTeleport;
        [SerializeField] private bool isCoR;

        [SerializeField] private bool hasKey;

        private Actions actions;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Movement.Interact.performed += Interact;
            actions.Enable();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            actions.Disable();
        }


        private void Update()
        {
            if (isInLift && inputPressed && !isCoR && toTeleport && hasKey)
            {
                StartCoroutine(TeleportCo());
            }
        }


        private void Interact(InputAction.CallbackContext ctx)
        {
            inputPressed = !inputPressed;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IPossessable>() != null)
            {
                isInLift = true;
                toTeleport = collision.gameObject;
            }
        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isInLift)
            {
                isInLift = false;
                toTeleport = null;
            }
        }


        private IEnumerator TeleportCo()
        {
            isCoR = true;
            liftUI.SetBool("LevelComplete", true);
            yield return new WaitForSeconds(2f);
            toTeleport.transform.position = toGoTo.transform.position;
            yield return new WaitForSeconds(1f);
            isCoR = false;
        }
    }
}