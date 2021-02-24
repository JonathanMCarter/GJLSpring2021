using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Dialogue
{
    public class DialogueThought : MonoBehaviour
    {
        [SerializeField] private DialogueFile file;
        [SerializeField] private Canvas dialogueUI;

        private bool hasSeenThought;
        private DialogueScript dial;


        private void Start()
        {
            dial = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
        }


        public void ShowThought()
        {
            if (!hasSeenThought)
            {
                dial.ChangeFile(file);
                dial.AutoDial();
            }
        }
    }
}