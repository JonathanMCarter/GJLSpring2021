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


        private void Start()
        {
            canvas = GetComponent<Canvas>();
            noteText = GetComponentsInChildren<Text>()[0];
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }


        private void Update()
        {
            if (canvas.enabled)
                if (actions.Movement.Back.phase == InputActionPhase.Performed)
                    CloseNote();
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
    }
}