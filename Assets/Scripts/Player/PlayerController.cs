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

        [SerializeField] private GameObject orbPrefab;
        private GameObject orb;

        // input controls ref
        private Actions actions;
        private InputDevice device;
        private Camera cam;

        private IMoveable moveAM;

        // component ref
        private Rigidbody2D rb;

        // is the user aiming
        [SerializeField] private bool isAiming;
        [SerializeField] private LayerMask mask;

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
            // for debugging only, can be removed if causing issues xD
            //if (am)
            //{
            ////    Debug.DrawLine(am.transform.position, actions.Movement.Move.ReadValue<Vector2>() * 10, Color.red);
            ////    Debug.DrawLine(am.transform.position, actions.Movement.MousePos.ReadValue<Vector2>() * 10, Color.blue);
            //    //Debug.DrawLine(am.transform.position, cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()), Color.green);
            //}
            

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


                #region (Old Code)
                // Old - Raycast Based Movement (Mostly used to check is possession worked xD
                //if (hit = Physics2D.Raycast(am.transform.position, actions.Movement.Move.ReadValue<Vector2>().normalized * 100, Mathf.Infinity, targetLayer))
                //{
                //    if (hit.collider.GetComponent<IPossessable>() != null)
                //    {
                //        targeting = hit.collider.GetComponent<IPossessable>();
                //    }
                //}


                //actions.Pocess.Shoot.performed += shoot =>
                //{
                //    if (targeting != null)
                //    {
                //        rb.velocity = ((targeting.obj.transform.position - am.transform.position).normalized * 10);
                //        inBody = false;
                //    }
                //};
                #endregion
            }
            else
            {
                // oh dear.... take dmg & allow player to shoot again.
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

                if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                {
                    Debug.Log("PC");
                    orb.GetComponent<Rigidbody2D>().AddForce(((Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()) - (Vector2)am.transform.position).normalized * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.Log("Console");
                    orb.GetComponent<Rigidbody2D>().AddForce(actions.Movement.Move.ReadValue<Vector2>() * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                }

                orb.GetComponent<Orb>().Yeet(am.GetComponent<IPossessable>());

                am = null;
            }
            else
            {
                if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")))
                {
                    Debug.Log("PC");
                    orb.GetComponent<Rigidbody2D>().AddForce(((Vector2)cam.ScreenToWorldPoint(actions.Movement.MousePos.ReadValue<Vector2>()) - (Vector2)transform.position).normalized * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.Log("Console");
                    orb.GetComponent<Rigidbody2D>().AddForce(actions.Movement.Move.ReadValue<Vector2>() * 10 * power * Time.deltaTime, ForceMode2D.Impulse);
                }
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