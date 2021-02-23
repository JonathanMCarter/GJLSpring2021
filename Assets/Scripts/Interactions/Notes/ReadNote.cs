using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class ReadNote : MonoBehaviour, IInteractable
    {
        [SerializeField] private Note note;
        private NoteUI noteUI;
        private GameObject arrowSprite;


        private void Start()
        {
            noteUI = GameObject.FindGameObjectWithTag("NoteUI").GetComponent<NoteUI>();
            arrowSprite = transform.GetChild(0).gameObject;
        }


        public void Interact()
        {
            noteUI.SetNote(note);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")))
                arrowSprite.SetActive(true);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")))
            {
                arrowSprite.SetActive(false);
                FindObjectOfType<PlayerController>().interaction = null;
            }
        }
    }
}