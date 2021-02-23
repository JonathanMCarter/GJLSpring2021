using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool canBeOpeneded;
        private bool isOpen;
        public bool hasKey = false;


        public void Interact()
        {
            if (!isOpen)
                OpenDoor();
            else
                CloseDoor();
        }


        private void OpenDoor()
        {
            isOpen = true;
        }


        private void CloseDoor()
        {
            isOpen = false;
        }
    }
}