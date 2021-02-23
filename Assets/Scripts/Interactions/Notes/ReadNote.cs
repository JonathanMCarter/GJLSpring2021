using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class ReadNote : MonoBehaviour, IInteractable
    {
        [SerializeField] private Note note;
        private NoteUI noteUI;
        private SpriteRenderer arrowSprite;


        private void Start()
        {
            noteUI = GameObject.FindGameObjectWithTag("NoteUI").GetComponent<NoteUI>();
            arrowSprite = GetComponentsInChildren<SpriteRenderer>()[1];
        }


        public void Interact()
        {
            noteUI.SetNote(note);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")))
                arrowSprite.enabled = true;
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Possessable")))
            {
                arrowSprite.enabled = false;
                FindObjectOfType<PlayerController>().interaction = null;
            }
        }
    }
}