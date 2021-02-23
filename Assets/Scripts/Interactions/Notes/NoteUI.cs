using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace TotallyNotEvil.Interactions
{
    public class NoteUI : MonoBehaviour
    {
        private Canvas canvas;
        private Text noteText;
        private PlayerController player;
        private Actions actions;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Movement.Back.performed += Back;
            actions.Enable();
        }


        private void OnDisable()
        {
            actions.Disable();
        }


        private void Start()
        {
            canvas = GetComponent<Canvas>();
            noteText = GetComponentsInChildren<Text>()[0];
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }


        public void SetNote(Note toRead)
        {
            player.enabled = false;
            canvas.enabled = true;
            noteText.GetComponent<Text>().text = toRead.noteContents;
        }


        public void CloseNote()
        {
            canvas.enabled = false;
            player.enabled = true;
        }


        private void Back(InputAction.CallbackContext ctx)
        {
            if (canvas.enabled)
                CloseNote();
        }
    }
}