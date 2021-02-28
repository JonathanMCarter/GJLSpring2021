using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarterGames.Assets.AudioManager;


namespace TotallyNotEvil
{
    public class Rat : MonoBehaviour, IPossessable, IMoveable
    {
        // Moveable
        [SerializeField] private float _moveSpeed;
        public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

        [SerializeField] private float _jumpHeight;
        public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }


        // Possession
        [SerializeField] private bool _possessed;
        public bool IsPossessed { get { return _possessed; } set { _possessed = value; } }
        public GameObject GetGameObject { get; set; }

        [SerializeField] private UnityEngine.Events.UnityEvent e;
        [SerializeField] private bool possessionEventRan = false;
        [SerializeField] private bool eventEnabled = false;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Animator anim;
        private AudioManager am;
        private RigidbodyConstraints2D initialRestraints;

        void Start()
        {
            GetGameObject = this.gameObject;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            am = FindObjectOfType<AudioManager>();
            initialRestraints = rb.constraints;
        }


        private void Update()
        {
            if (IsPossessed && !possessionEventRan && eventEnabled)
            {
                e.Invoke();
                possessionEventRan = true;
            }
            else if (IsPossessed)
            {
                RatAnim();
            }

            if (!IsPossessed && !rb.constraints.HasFlag(RigidbodyConstraints2D.FreezePositionX))
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            else if (IsPossessed && rb.constraints.HasFlag(RigidbodyConstraints2D.FreezePositionX)) {
                rb.constraints = initialRestraints;
            }
        }


        public void MoveAction(Vector2 dir)
        {
            rb.velocity = new Vector2(dir.x * MoveSpeed, rb.velocity.y);
        }


        public void JumpAction()
        {
            // Rats can't jump, just for the sake of less work, the method still has to be here though xD.
        }


        /// <summary>
        /// Rat animation fucntionaility
        /// </summary>
        private void RatAnim()
        {
            // Is Moving?
            if (rb.velocity.normalized.x > .05f || rb.velocity.normalized.x < -.05f)
            {
                anim.SetBool("IsMoving", true);

                // Flipping sprites
                if (rb.velocity.normalized.x < -.01f)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            else
                anim.SetBool("IsMoving", false);
        }


        public void FootSteps()
        {
            am.PlayTracked("Rat movement loop");
        }
    }
}