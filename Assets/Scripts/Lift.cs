using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarterGames.Assets.AudioManager;
using TotallyNotEvil.Audio;

namespace TotallyNotEvil
{
    public class Lift : MonoBehaviour
    {
        [SerializeField] private bool isInFrontOfLift;
        [SerializeField] internal bool inputPressed;
        [SerializeField] private GameObject toGoTo;
        [SerializeField] private Animator liftUI;

        [SerializeField] private GameObject toTeleport;
        [SerializeField] private bool isCoR;
        [SerializeField] private GameObject[] peopleToUnmute;
        [SerializeField] private GameObject[] peopleToMute;

        public bool hasKey;

        private Actions actions;
        private AudioManager am;

        [SerializeField] private bool toOffice;
        [SerializeField] private bool toCEO;
        private MusicController mc;


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

        private void Start()
        {
            am = FindObjectOfType<AudioManager>();
            mc = FindObjectOfType<MusicController>();
        }


        private void Update()
        {
            if (isInFrontOfLift && inputPressed && !isCoR && toTeleport && hasKey)
            {
                //Debug.Log("?");
                StartCoroutine(TeleportCo());
            }

            if (isCoR) {
                if (toOffice) mc.BasementToLift();
                else mc.LiftToOffice();
            }

            if (!hasKey && inputPressed) {
                inputPressed = false;
            }

        }


        private void Interact(InputAction.CallbackContext ctx)
        {
            if(isInFrontOfLift)
            inputPressed = true;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.GetComponent<IPossessable>() != null)
            {
                if (collision.GetComponent<IPossessable>().IsPossessed) // don't allow NPCs in the elevator 
                {
                    isInFrontOfLift = true;
                    toTeleport = collision.gameObject;
                }
            }
        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isInFrontOfLift)
            {
                isInFrontOfLift = false;
                toTeleport = null;
            }
        }


        private IEnumerator TeleportCo()
        {
            inputPressed = false;
            isCoR = true;

            foreach (var person in peopleToMute)
            {
                person.GetComponent<PeepsSteps>().IsMuted = true;
            }

            mc.IncrementStage(); //change music
            liftUI.SetBool("LevelComplete", true);
            yield return new WaitForSeconds(1.5f);
            am.Play("Elevator door close");
            yield return new WaitForSeconds(4.5f);
            toTeleport.transform.position = toGoTo.transform.position;
            yield return new WaitForSeconds(1f);
            mc.IncrementStage(); //change music
            isCoR = false;

            foreach (var person in peopleToUnmute) {
                person.GetComponent<PeepsSteps>().IsMuted = false;
            }

            liftUI.SetBool("LevelComplete", false);
        }


        public void HasGotKey()
        {
            hasKey = true;
        }
    }
}