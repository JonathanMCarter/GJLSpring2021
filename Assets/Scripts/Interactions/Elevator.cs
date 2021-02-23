using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    public class Elevator : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator anim;
        private PlayerController player;
        private LevelTransition trans;


        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            trans = GetComponent<LevelTransition>();
        }


        public void Interact()
        {
            Debug.Log("kfjdfj");
            anim.SetBool("LevelComplete", true);
            player.enabled = false;
            trans.Transition();
        }
    }
}