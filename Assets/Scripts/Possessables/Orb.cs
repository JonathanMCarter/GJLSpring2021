using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Orb : MonoBehaviour, IDamagable
    {
        // assinged to avoid GD
        private WaitForSeconds wait;
        [SerializeField] private bool canPossess;

        private PlayerController player;

        // damage visual via vignette ideally (gonna work on it on weds)
        [SerializeField] private UnityEngine.Rendering.Universal.Vignette vig;

        // Damage
        public bool CanTakeDamage { get; set; }
        public int Health { get; set; }

        private bool isCoR;


        // Animation
        private Animator anim;
        private Rigidbody2D rb;
        private SpriteRenderer sr;


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnEnable()
        {
            Health = 10;
        }

        private void Start()
        {
            CanTakeDamage = false;
            wait = new WaitForSeconds(.5f);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            Debug.Log(CanTakeDamage + " : " + Health);

            if (!player.inBody && !isCoR)
            {
                if (CanTakeDamage)
                    StartCoroutine(DamageOverTime());
            }


            // if dead - die (will show a death ui eventually)
            if (Health <= 0)
            {
                gameObject.SetActive(false);
            }

            Debug.Log(rb.velocity.normalized.x);

            if (rb.velocity.normalized.x > .1f || rb.velocity.normalized.x < -.1f)
            {
                anim.SetBool("IsMoving", true);
            }
            else
                anim.SetBool("IsMoving", false);


            if (rb.velocity.normalized.y < .01f && rb.velocity.normalized.y > -.01f)
            {
                anim.SetBool("IsFlying", false);
            }


            // flying
            if (anim.GetBool("IsFlying"))
            {
                if (rb.velocity.normalized.y > .25f)
                {
                    // up
                    anim.SetInteger("FlyDirection", 1);
                }
                else if (rb.velocity.normalized.y < .25f && rb.velocity.normalized.y > -.25f)
                {
                    // stright (ish)
                    anim.SetInteger("FlyDirection", 0);
                }
                else if (rb.velocity.normalized.y < -.25f)
                {
                    // down
                    anim.SetInteger("FlyDirection", -1);
                }

                if (rb.velocity.normalized.x > .5f)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            else if (!anim.GetBool("IsAiming") && !anim.GetBool("IsFlying"))
            {
                if (rb.velocity.normalized.x > .5f)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
        }


        private void OnCollisionStay2D(Collision2D collision)
        {
            if (canPossess)
            {
                // making a variable so it is easier to read (I use it like 3/4 times here so made sense).
                IPossessable _poss = collision.gameObject.GetComponent<IPossessable>();

                if (_poss != null)
                {
                    if (!_poss.IsPossessed)
                    {
                        Debug.Log("Possess Me!");
                        _poss.IsPossessed = true;

                        // set the player to control the hit IPossessable object.
                        player.SetAm(_poss);

                        // disables the orb
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Im Possessed!");
                    }
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Interactions.IInteractable>() != null)
                player.interaction = collision.GetComponent<Interactions.IInteractable>();
        }


        /// <summary>
        /// Fire!!!! - just runs the co, that stops the player ending up in the same obj as they were in.
        /// </summary>
        public void Yeet(IPossessable was)
        {
            StartCoroutine(ExitHostCooldown(was));
        }


        /// <summary>
        /// (Co) - gives a slight cooldown between exiting a person/object/thing.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExitHostCooldown(IPossessable was)
        {
            canPossess = false;
            yield return wait;

            // stops the old object from being defined as possessed.
            was.IsPossessed = false;

            canPossess = true;
        }


        /// <summary>
        /// Takes damage.
        /// </summary>
        /// <param name="dmg"></param>
        public void TakeDamage(int dmg)
        {
            Health -= dmg;
        }


        private IEnumerator DamageOverTime(float delay = 1f)
        {
            isCoR = true;
            yield return new WaitForSeconds(delay);
            TakeDamage(1);
            isCoR = false;
        }
    }
}