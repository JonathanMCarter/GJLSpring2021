using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TotallyNotEvil
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Currently In")]
        [Tooltip("The object the player currently controls")]
        [SerializeField] internal GameObject am;

        [Tooltip("Is the player in a body?")]
        [SerializeField] private bool inBody;

        [Header("Movement Force")]
        [SerializeField] private float power;
        [SerializeField] private float mulitplier = 50f;
        [SerializeField] private float powerLimit = 250f;

        [Header("Orb")]
        [SerializeField] private GameObject orbPrefab;

        [Header("Camera")]
        [SerializeField] private CameraController vCam;


        private GameObject orb;

        // input controls ref
        private Actions actions;
        private InputDevice device;
        private Camera cam;

        private IMoveable moveAM;

        // component ref
        private Rigidbody2D rb;

        // is the user aiming
        private bool isAiming;



        private void OnDisable()
        {
            actions.Disable();
        }



        private void Awake()
        {
            actions = new Actions();

            // Draw & Shoot
            actions.Possess.Shoot.started += Drawing;
            actions.Possess.Shoot.canceled += Release;

            // Input Check
            InputSystem.onActionChange += (obj, change) =>
            {
                ControllerChanged(obj, change);
            };

            actions.Enable();
        }


        private void Start()
        {
            rb = am.GetComponent<Rigidbody2D>();
            cam = Camera.main;

            // defines the object in at the start as possessed.
            am.GetComponent<IPossessable>().IsPossessed = true;

            orb = Instantiate(orbPrefab);
            orb.SetActive(false);
        }


        private void Update()
        {
            if (inBody)
            {
                if (isAiming)
                {
                    power += Time.deltaTime * mulitplier;

                    if (power > powerLimit)
                        power = powerLimit;
                }
                else
                {
                    // normal player movement (in possession of person/object/thing)
                    if (moveAM == null) moveAM = am.GetComponent<IMoveable>();
                    else
                    {
                        moveAM.MoveAction(actions.Movement.Move.ReadValue<Vector2>());

                        if (actions.Movement.Jump.phase == InputActionPhase.Performed)
                            moveAM.JumpAction();
                    }
                }
            }
            else
            {
                // oh dear.... take dmg & allow player to shoot again. (dmg bit not done yet xD)
                if (isAiming)
                {
                    power += Time.deltaTime * mulitplier;

                    if (power > powerLimit)
                        power = powerLimit;
                }
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
        }


        /// <summary>
        /// Called when the user press the shoot button (either held down or tapped)
        /// </summary>
        /// <param name="ctx"></param>
        private void Drawing(InputAction.CallbackContext ctx)
        {
            isAiming = true;
        }


        /// <summary>
        /// Called when the user releases the shoot button
        /// </summary>
        /// <param name="ctx"></param>
        private void Release(InputAction.CallbackContext ctx)
        {
            // ignore collision with current possession so to not hit the object on exit. (Sadly doesn't work for triggers...)
            if (inBody)
            {
                Physics2D.IgnoreCollision(orb.GetComponent<Collider2D>(), am.GetComponent<Collider2D>());

                // yeet the player (orb) around
                orb.SetActive(true);

                orb.transform.position = am.transform.position;

                inBody = false;

                // direction to shoot
                if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                    orb.GetComponent<Rigidbody2D>().AddForce(((Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()) - (Vector2)am.transform.position).normalized * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                else
                    orb.GetComponent<Rigidbody2D>().AddForce(actions.Movement.Move.ReadValue<Vector2>() * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                

                orb.GetComponent<Orb>().Yeet(am.GetComponent<IPossessable>());

                am = null;

                vCam.SetTargetAndFollow(orb.transform);
            }
            else
            {
                // allows for movement if not in a body xD
                if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                    orb.GetComponent<Rigidbody2D>().AddForce(((Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()) - (Vector2)transform.position).normalized * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                else
                    orb.GetComponent<Rigidbody2D>().AddForce(actions.Movement.Move.ReadValue<Vector2>() * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
            }

            // stop "aiming" & reset the pwoer value
            isAiming = false;
            power = 0f;
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
                Debug.Log(device.displayName);
            }
        }
    }
}