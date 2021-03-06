using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace TotallyNotEvil
{
    public class Orb : MonoBehaviour, IDamagable
    {
        // assinged to avoid GD
        private WaitForSeconds wait;
        [SerializeField] private bool canPossess;

        private PlayerController player;


        // damage visual via vignette ideally (gonna work on it on weds)
        [SerializeField] private Volume data;

        // Damage
        public bool CanTakeDamage { get; set; }
        public int Health { get; set; }

        private bool isCoR;
        private bool hasCalledDeath;

        // Animation
        private Animator anim;
        private Rigidbody2D rb;
        private SpriteRenderer sr;


        private void OnDisable()
        {
            StopAllCoroutines();
            isCoR = false;
        }

        private void OnEnable()
        {
            CanTakeDamage = false;
            Health = 10;
        }


        private void Start()
        {
            wait = new WaitForSeconds(.5f);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            data = FindObjectOfType<Volume>();

            sr.sortingOrder = 4;
        }


        private void Update()
        {
            //Debug.Log(CanTakeDamage + " : " + Health);

            if (!player.inBody && !isCoR)
            {
                if (CanTakeDamage)
                    StartCoroutine(DamageOverTime());
            }


            // if dead - die
            if (Health <= 0 && !hasCalledDeath)
            {
                string[] _deathMessages = new string[] { "Faded Away...", "AFK", "Froze in horror" };
                Death.PlayerHasDeath(_deathMessages[Random.Range(0, _deathMessages.Length - 1)]);
                hasCalledDeath = true;
            }

            SpectreAnim();
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
            data.profile.TryGet(out Vignette _vin);
            _vin.intensity.value += .05f;
        }


        private IEnumerator DamageOverTime(float delay = 2.5f)
        {
            isCoR = true;
            yield return new WaitForSeconds(delay);
            TakeDamage(1);
            isCoR = false;
        }


        /// <summary>
        /// Handles almsot all the spectre anims, aiming stuff is on the player controller
        /// </summary>
        private void SpectreAnim()
        {
            // Moving

            if (rb.velocity.normalized.x > .1f || rb.velocity.normalized.x < -.1f)
            {
                anim.SetBool("IsMoving", true);

                if (rb.velocity.normalized.x < -.05f)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            else
                anim.SetBool("IsMoving", false);


            // aiming sprite flip
            if (player.isAiming)
            {
                if (player.lr.GetPosition(1).x < transform.localPosition.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }


            // Flying
            if (rb.velocity.normalized.y == 0f)
            {
                anim.SetBool("IsFlying", false);
            }


            // Flying Direction
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
        }
    }
}