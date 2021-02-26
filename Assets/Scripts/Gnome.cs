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

        private SpriteRenderer toMurderSR;
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

                    if (toMurderSR && toMurderSR.color.a > 0)
                        toMurderSR.color = new Color32(255, 255, 255, (byte)(-1 * Time.deltaTime));
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
                    toMurderSR = collision.GetComponent<SpriteRenderer>();
                    sr.sprite = gnomeSprites[1];
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (shouldMurder)
            {
                shouldMurder = false;
                murderTimer = 0;
                toMurderSR = null;
                sr.sprite = gnomeSprites[0];
            }
        }
    }
}