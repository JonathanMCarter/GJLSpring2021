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
        [SerializeField] private bool isInLift;
        [SerializeField] internal bool inputPressed;
        [SerializeField] private GameObject toGoTo;
        [SerializeField] private Animator liftUI;

        [SerializeField] private GameObject toTeleport;
        [SerializeField] private bool isCoR;

        public bool hasKey;

        private Actions actions;
        private AudioManager am;
        private MusicController music;

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
            music = FindObjectOfType<MusicController>();
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

                if (toOffice)
                {
                    mc.inOffice = true;
                    mc.inBasement = false;
                }

                if (toCEO)
                {
                    mc.inCEOOffice = true;
                    mc.inOffice = false;
                }
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
            yield return new WaitForSeconds(1f);
            am.Play("Elevator door close");
            music.PlayLiftMusic();
            yield return new WaitForSeconds(1f);
            toTeleport.transform.position = toGoTo.transform.position;
            yield return new WaitForSeconds(3f);
            music.ChangeMusic();
            isCoR = false;
        }


        public void HasGotKey()
        {
            hasKey = true;
        }
    }
}