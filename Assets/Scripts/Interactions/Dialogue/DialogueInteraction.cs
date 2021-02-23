using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;
using UnityEngine.InputSystem;


namespace TotallyNotEvil.Interactions
{
    public class DialogueInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueFile dialogueFile;
        [SerializeField] private Canvas dialCanvas;

        private DialogueScript dial;
        private Actions actions;
        [SerializeField] private PlayerController player;
        [SerializeField] private bool canTalk = false;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Movement.Interact.performed += ProgressDialogue;
            actions.Enable();
        }


        private void OnDisable()
        {
            actions.Disable();    
        }


        private void Start()
        {
            dial = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
        }


        public void Interact()
        {
            dial.ChangeFile(dialogueFile);
            dial.Input();
        }


        public void ProgressDialogue(InputAction.CallbackContext ctx)
        {
            if (canTalk)
            {
                if (!dial.fileHasEnded)
                    dial.Input();
                else
                {
                    dial.Reset();
                    dial.Input();
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IPossessable>() != null)
            {
                if (collision.GetComponent<IPossessable>().IsPossessed)
                {
                    canTalk = true;
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<IPossessable>() != null)
            {
                if (collision.GetComponent<IPossessable>().IsPossessed)
                {
                    canTalk =false;
                }
            }
        }
    }
}