using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TotallyNotEvil.Interactions;

namespace TotallyNotEvil
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Currently In")]
        [Tooltip("The object the player currently controls")]
        [SerializeField] internal GameObject am;

        [Tooltip("Is the player in a body?")]
        [SerializeField] internal bool inBody;
        [SerializeField] private float repossessionDelay = 1f;
        [SerializeField] private bool startAsOrb;

        [Header("Movement Force")]
        [SerializeField] private float power;
        [SerializeField] private float mulitplier = 50f;
        [SerializeField] private float powerLimit = 250f;

        [Header("Orb & Aiming")]
        [SerializeField] private GameObject orbPrefab;

        [Header("Camera")]
        [SerializeField] private CameraController vCam;
        [SerializeField] private Canvas arrow;


        private GameObject orb;

        // input controls ref
        private Actions actions;
        private InputDevice device;
        private Camera cam;
        private LineRenderer lr;

        private IMoveable moveAM;
        internal IInteractable interaction;

        // component ref
        private Rigidbody2D rb;

        // is the user aiming
        private bool isAiming;


        private void OnDisable()
        {
            actions.Disable();
        }


        private void OnEnable()
        {
            actions = new Actions();

            // Draw & Shoot
            actions.Possess.Shoot.started += Drawing;
            actions.Possess.Shoot.canceled += Release;

            // interactions
            actions.Movement.Interact.performed += UseInteraction;

            // Input Check
            InputSystem.onActionChange += (obj, change) =>
            {
                ControllerChanged(obj, change);
            };

            actions.Enable();
        }


        private void Start()
        {
            // Sets up the orb
            orb = Instantiate(orbPrefab);
            orb.SetActive(false);

            if (startAsOrb)
            {
                orb.SetActive(true);
                am = orb;
            }

            // Defines the object in at the start as possessed.
            if (am.GetComponent<IPossessable>() != null)
                am.GetComponent<IPossessable>().IsPossessed = true;

            // References
            rb = am.GetComponent<Rigidbody2D>();
            cam = Camera.main;
            lr = GetComponent<LineRenderer>();
        }


        private void Update()
        {
            // if the player is in a body...
            if (inBody)
            {
                // is the player aiming...
                if (isAiming)
                {
                    IncreasePower();
                    AimingLine(am);
                }
                else
                {
                    // normal player movement (in possession of person/object/thing)
                    if (moveAM == null) moveAM = am.GetComponent<IMoveable>();
                    else
                    {
                        moveAM.MoveAction(actions.Movement.Move.ReadValue<Vector2>());

                        if (actions.Movement.Jump.phase == InputActionPhase.Performed)
                        {
                            moveAM.JumpAction();
                            //Debug.Log("jumped");
                        }
                    } 
                }

                arrow.transform.position = new Vector3(am.transform.position.x, am.transform.position.y + 1f, 0f);
            }
            else
            {
                // oh dear.... take dmg & allow player to shoot again. (dmg bit not done yet xD)
                if (isAiming)
                {
                    IncreasePower();
                    AimingLine(orb);
                }

                arrow.transform.position = new Vector3(orb.transform.position.x, orb.transform.position.y + 1f, 0f);
            }
        }


        internal void UseInteraction(InputAction.CallbackContext ctx)
        {
            if (interaction != null)
            {
                interaction.Interact();
            }
        }



        /// <summary>
        /// Sets the player to possess the IPossessable object passed through.
        /// </summary>
        /// <param name="pos">To Possess</param>
        public void SetAm(IPossessable pos)
        {
            am = pos.GetGameObject;
            if (am.GetComponent<IMoveable>() != null) moveAM = am.GetComponent<IMoveable>();
            rb = am.GetComponent<Rigidbody2D>();
            inBody = true;
            vCam.SetTargetAndFollow(am.transform);

            // thoughts
            if (am.GetComponentInChildren<Dialogue.DialogueThought>()) ShowThought(am.GetComponentInChildren<Dialogue.DialogueThought>());
        }


        /// <summary>
        /// Called when the user press the shoot button (either held down or tapped, like GetButtonDown in the old input system)
        /// </summary>
        /// <param name="ctx"></param>
        private void Drawing(InputAction.CallbackContext ctx)
        {
            isAiming = true;
        }


        /// <summary>
        /// Called when the user releases the shoot button (Like GetButtonUp in the old input system)
        /// </summary>
        /// <param name="ctx"></param>
        private void Release(InputAction.CallbackContext ctx)
        {
            if (inBody)
            {
                Collider2D _orbCollider = orb.GetComponent<Collider2D>();
                Collider2D _amCollider = am.GetComponent<Collider2D>();
                // ignore collision with current possession so to not hit the object on exit. (Sadly doesn't work for triggers...)
                Physics2D.IgnoreCollision(_orbCollider, _amCollider);

                // CARSON -> experimental : allows player to possess the same entity twice (after waiting a short amount of time
                StartCoroutine(IgnoreCollisionFalse(_orbCollider, _amCollider));

                // yeet the player (orb) around
                orb.SetActive(true);
                orb.transform.position = am.transform.position;

                // the player is no longer in a body
                inBody = false;

                // direction to shoot
                ShootOrbInDirection(am);
                
                orb.GetComponent<Orb>().Yeet(am.GetComponent<IPossessable>());
                rb.velocity = Vector2.zero;
                am = null;
                vCam.SetTargetAndFollow(orb.transform);
            }
            else
                ShootOrbInDirection(orb);


            // stop "aiming" & reset the power value
            lr.enabled = false;
            isAiming = false;
            power = 0f;
        }

        private IEnumerator IgnoreCollisionFalse(Collider2D colliderA, Collider2D colliderB) 
        {
            yield return new WaitForSeconds(repossessionDelay);
            Physics2D.IgnoreCollision(colliderA, colliderB, false);
        }


        /// <summary>
        /// Checks to see if the control input has changed (mostly for plugging in a controller etc).
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="change"></param>
        private void ControllerChanged(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                device = lastControl.device;
                //Debug.Log(device.displayName);
            }
        }


        /// <summary>
        /// Increases the power of the shot when called.
        /// </summary>
        private void IncreasePower()
        {
            power += Time.deltaTime * mulitplier;

            if (power > powerLimit)
                power = powerLimit;
        }


        /// <summary>
        /// Draws the line renderer from the player possessed character / orb to where they are aiming.
        /// </summary>
        /// <param name="player">What the player currently is</param>
        private void AimingLine(GameObject player)
        {

            // set the amount of alpha (transparency) equal to the proportion of power to power-limit, 
            // giving our player a visual indicator of how much power is going to be exerted in their shot
            float powerProportion = power / powerLimit;
            powerProportion = powerProportion * powerProportion; // this should make the player disinclined toward low-power
            Color lineColor = lr.endColor;
            lineColor.a = powerProportion;
            lr.endColor = lineColor;


            if (!lr.enabled)
                lr.enabled = true;

            lr.SetPosition(0, player.transform.position);

            if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                lr.SetPosition(1, (Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()));
            else
                lr.SetPosition(1, (Vector2)player.transform.position + actions.Movement.Move.ReadValue<Vector2>() * 5);
        }


        /// <summary>
        /// Shoots the player in the direction aiming at.
        /// </summary>
        /// <param name="player">What the player currently is</param>
        private void ShootOrbInDirection(GameObject player)
        {
            if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                orb.GetComponent<Rigidbody2D>().AddForce(((Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()) - (Vector2)player.transform.position).normalized * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
            else
                orb.GetComponent<Rigidbody2D>().AddForce(actions.Movement.Move.ReadValue<Vector2>() * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
        }


        /// <summary>
        /// Shows the thought bubble if the possess person has one and hasn't shown it before.
        /// </summary>
        /// <param name="think">The thought to pass through.</param>
        private void ShowThought(Dialogue.DialogueThought thought)
        {
            // needs to be a version of dialogue interaction with a bool that stops it, and the runs when possessed automatically when possessed.
            thought.ShowThought();
        }


        public void HurtPlayer(int dmg)
        {
            if (am.GetComponent<IDamagable>().CanTakeDamage)
            {
                am.GetComponent<IDamagable>().Health -= dmg;
            }
        }
    }
}