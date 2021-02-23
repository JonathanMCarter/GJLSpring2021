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
        private bool dialActive;
        private Actions actions;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Movement.Jump.performed += ProgressDialogue;
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
            if (!dial.fileHasEnded)
                dial.Input();
        }
    }
}