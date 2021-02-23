using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class ReadNote : MonoBehaviour, IInteractable
    {
        [SerializeField] private Note note;
        private bool isReading;
        private NoteUI noteUI;


        private void Start()
        {
            noteUI = GameObject.FindGameObjectWithTag("NoteUI").GetComponent<NoteUI>();
        }


        public void Interact()
        {
            if (!isReading)
            {
                noteUI.SetNote(note);
            }
        }
    }
}