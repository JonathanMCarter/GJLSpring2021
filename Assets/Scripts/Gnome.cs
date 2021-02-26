using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Gnome : MonoBehaviour
    {
        [SerializeField] private Sprite[] gnomeSprite;

        [SerializeField] private SpriteRenderer sr;
        private Animator anim;

        [SerializeField] private GameObject toMurder;
        [SerializeField] private bool shouldMurder;
        [SerializeField] private float murderTimer;
        [SerializeField] private float murderTimeLimit;

        private bool isCoR;
        private PlayerController player;


        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<PlayerController>();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            anim.enabled = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (shouldMurder)
            {
                sr.sprite = gnomeSprite[1];

                murderTimer += Time.deltaTime;

                if (murderTimer > murderTimeLimit)
                {
                    anim.enabled = true;

                    if (!anim.GetBool("Murder"))
                    {
                        anim.SetBool("Murder", true);

                        if (!isCoR)
                            StartCoroutine(Kill());
                    }
                }
            }
            else
            {
                sr.sprite = gnomeSprite[0];

                if (anim.GetBool("Murder"))
                {
                    anim.SetBool("Murder", false);
                    anim.enabled = false;
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
                    toMurder = collision.gameObject;
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (shouldMurder)
            {
                StopAllCoroutines();
                shouldMurder = false;
                murderTimer = 0;
                toMurder = null;
            }
        }


        private IEnumerator Kill()
        {
            isCoR = true;
            yield return new WaitForSeconds(1f);
            player.DePossess();
            toMurder.SetActive(false);
            isCoR = false;
        }
    }
}