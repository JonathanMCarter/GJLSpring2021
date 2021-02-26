using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Gnome : MonoBehaviour
    {
        [SerializeField] private Sprite[] gnomeSprites;

        private SpriteRenderer sr;
        private Animator anim;

        private GameObject toMurder;
        private bool shouldMurder;
        private float murderTimer;
        private float murderTimeLimit;


        // Start is called before the first frame update
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }


        // Update is called once per frame
        void Update()
        {
            if (shouldMurder)
            {
                murderTimer += Time.deltaTime;

                if (murderTimer > murderTimeLimit)
                {
                    if (anim.GetBool("Murder"))
                        anim.SetBool("Murder", true);


                }
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IPossessable>() != null)
            {
                if (collision.GetComponent<Person>())
                {
                    shouldMurder = true;
                    sr.sprite = gnomeSprites[1];
                    toMurder = collision.gameObject;
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (shouldMurder)
            {
                shouldMurder = false;
                murderTimer = 0;
                toMurder = null;
                sr.sprite = gnomeSprites[0];
            }
        }
    }
}