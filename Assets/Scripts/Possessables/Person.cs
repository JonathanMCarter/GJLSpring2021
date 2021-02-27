using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotallyNotEvil
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Person : MonoBehaviour, IPossessable, IMoveable
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

        // Possession Sprite
        [SerializeField] private Sprite possessedIdleSprite;
        private Sprite defaultSprite;
        private SpriteRenderer sr;

        // Anim stuff
        private Animator anim;


        private Rigidbody2D rb;
        private PlayerController player;

        private LayerMask masks;

        [Header("AI Stuff")]
        [SerializeField] private bool canWander = true;
        [SerializeField] private Vector2[] range;
        [SerializeField] private Vector2 posToMoveTo;
        [SerializeField] private float moveSpeedAI = 5f;
        [SerializeField] private float arrivalTolerance = 0.5f; //acceptable distance to destination to have arrived
        [SerializeField] private float minTravelDistance = 1f; //radius within which the AI does not choose positions from
        [SerializeField] private bool canMove;
        [SerializeField] private bool inMotion;

        bool startingJump;

        private Vector2 personShape;

        private void Start()
        {
            canMove = true;
            inMotion = false;
            startingJump = false;
            GetGameObject = this.gameObject;
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            sr = GetComponent<SpriteRenderer>();

            // set the default sprite
            defaultSprite = GetComponent<SpriteRenderer>().sprite;

            // anim
            anim = GetComponent<Animator>();

            //set person shape (for IsGrounded)
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            // boxCollider.size gives values half as big as we might naively expect
            // doubling the y value gives our ordinary height
            // keeping the x value half of the width makes sure our boxCast (later in the code) doesn't 
            // get triggered by accidental side-contact overlap between persons - avoiding unexpected rocket boots
            personShape = new Vector2(boxCollider.size.x, boxCollider.size.y*2);

            masks = LayerMask.GetMask("World", "Possessable");
        }


        private void Update()
        {
            if (!IsPossessed && canMove) 
            {
                // AI movement

                // define movement goal if NPC can wander and is on the ground and not in motion
                if (!inMotion) 
                {
                    if (canWander)
                    {
                        if (IsGrounded())
                        { //being grounded allows us to set the correct y position for posToMoveTo

                            posToMoveTo = new Vector2(RandomXNotTooClose(), transform.position.y);

                            //Debug.Log(Vector2.Distance(transform.position, posToMoveTo)); //should show that the distance of the move is greater than void-radius

                            StartCoroutine(ChooseMovePos());
                        }
                    }
                }
                else // if inMotion, pursue movement goal
                {
                    MoveAction(posToMoveTo);
                }
                // TODO allow AI to arrive at a given location
                if (Vector2.Distance(transform.position, posToMoveTo) < arrivalTolerance) {
                    inMotion = false;
                }
            }

            // runs the anims
            PeopleAnim();
        }

        private void LateUpdate()
        {
            // putting sprite changes in a late update assures we can override the animator

            // edits sprite to possessed sprite.
            if (IsPossessed && sr.sprite.Equals(defaultSprite))
            {
                sr.sprite = possessedIdleSprite;
            }
            else if (!IsPossessed && sr.sprite.Equals(possessedIdleSprite))
            {
                sr.sprite = defaultSprite;
            }
        }


        public void MoveAction(Vector2 dir)
        {
            if (IsPossessed)
                rb.velocity = new Vector2(dir.x * MoveSpeed, rb.velocity.y);
            else
                rb.position = Vector2.Lerp(transform.position, posToMoveTo, moveSpeedAI * Time.smoothDeltaTime);
        }


        public void JumpAction()
        {
            Debug.Log("IsGrounded: " + IsGrounded().ToString());
            if (IsGrounded() && !startingJump)
            {
                startingJump = true;
                StartCoroutine(JumpCoolDown());
                rb.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            }
        }


        private bool IsGrounded()
        {
            RaycastHit2D[] _hits = Physics2D.BoxCastAll(transform.position, personShape, 0f, Vector2.down, .05f, masks);

            if (_hits != null && _hits.Length != 0)
            {
                for (int i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider.gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }


            return false;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Interactions.IInteractable>() != null)
                player.interaction = collision.GetComponent<Interactions.IInteractable>();
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Interactions.IInteractable>() != null)
                FindObjectOfType<PlayerController>().interaction = null;
        }


        private IEnumerator ChooseMovePos()
        {
            canMove = false;
            yield return new WaitForSeconds(Random.Range(2, 4));
            canMove = true;
            inMotion = true;
        }
        private IEnumerator JumpCoolDown() {
            yield return new WaitForSeconds(.5f);
            startingJump = false;
        }

        private float RandomXNotTooClose() {
            // we want to make sure our new random position is not too close to our initial position

            float initialX = transform.position.x;
            float overallRange = range[1].x - range[0].x;
            // throw error if 2 * minimum travel is bigger than the overall range
            if (overallRange < 2 * minTravelDistance) {
                Debug.LogError("the overall range for the AI must be greater than double the minimum travel distance. gameObject-name: " + gameObject.name);
            }
            // throw error if range[0].x is not smaller than range[1].x;
            if (range[0].x >= range[1].x) {
                Debug.LogError("lower value of range (index 0) must be strictly smaller than upper value (index 1) for AI. gameObject-name: " + gameObject.name);
            }

            // start the random count at the initial position + min travel distance - intial lower bound 
            // the lower value is subtracted for the modulo in the next step to work correct. 
            float lowerBound = (initialX + minTravelDistance) - range[0].x;
            // upper bound at the (new) lowerBound + initial range - 2 * min travel distance
            float upperBound = (lowerBound + overallRange) - 2 * minTravelDistance;
            // apply a modulo of the overall range to a number generated between the new upper and lower bounds
            float wrappedValue = Random.Range(lowerBound, upperBound) % overallRange;
            // finally, add back the initial lower bound
            return wrappedValue + range[0].x;
        }



        /// <summary>
        /// Handles all the anim for the people
        /// </summary>
        private void PeopleAnim()
        {
            // is possessed?
            if (IsPossessed && !anim.GetBool("IsPossessed"))
                anim.SetBool("IsPossessed", true);
            else if (!IsPossessed && anim.GetBool("IsPossessed"))
                anim.SetBool("IsPossessed", false);


            // is walking?
            if ((IsPossessed && (rb.velocity.normalized.x > .1f || rb.velocity.normalized.x < -.1f)) || (!IsPossessed && inMotion))
            {
                anim.SetBool("IsWalking", true);

                if (rb.velocity.normalized.x < -.05f)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            else if (anim.GetBool("IsWalking"))
                anim.SetBool("IsWalking", false);
                
        }
    }
}