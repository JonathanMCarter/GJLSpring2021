using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotallyNotEvil
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        private Actions actions;
        [SerializeField] private GameObject am;
        private IPocessable targeting;
        [SerializeField] private bool inBody;
        private Rigidbody2D rb;
        private RaycastHit2D hit;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Enable();
        }


        private void OnDisable()
        {
            actions.Disable();
        }


        // Start is called before the first frame update
        void Start()
        {
            rb = am.GetComponent<Rigidbody2D>();
            am.GetComponent<IPocessable>().IsPocessed = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!inBody)
            {
                if (hit = Physics2D.Raycast(am.transform.position, actions.Movement.Move.ReadValue<Vector2>().normalized * 100, Mathf.Infinity, targetLayer))
                {
                    Debug.Log(hit.collider.gameObject);

                    if (hit.collider.GetComponent<IPocessable>() != null)
                    {
                        Debug.Log("assigned");
                        targeting = hit.collider.GetComponent<IPocessable>();
                    }
                }


                actions.Pocess.Shoot.performed += shoot =>
                {
                    Debug.Log("SHOOT");

                    if (targeting != null)
                    {
                        Debug.Log("trying");
                        rb.velocity = ((targeting.obj.transform.position - am.transform.position).normalized * 10);
                        inBody = false;
                    }
                };
            }
            else
            {
                rb.velocity = new Vector2(actions.Movement.Move.ReadValue<Vector2>().x, 0) * 2;
            }
        }


        public void SetAm(IPocessable pos)
        {
            am = pos.obj;
            rb = am.GetComponent<Rigidbody2D>();
            inBody = true;
        }
    }
}