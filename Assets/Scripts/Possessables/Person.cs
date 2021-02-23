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


        private Rigidbody2D rb;
        private PlayerController player;

        [SerializeField] private LayerMask mask;
        [SerializeField] private LayerMask toIgnore;


        private void Start()
        {
            GetGameObject = this.gameObject;
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }


        public void MoveAction(Vector2 dir)
        {
            rb.velocity = new Vector2(dir.x * MoveSpeed, rb.velocity.y);
        }


        public void JumpAction()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
            }
        }


        private bool IsGrounded()
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, Vector2.one * 1.5f, 0f, Vector2.down, .05f, mask);

            if (_hit.collider != null)
            {
                Debug.Log(_hit.collider.gameObject.name);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Interactions.IInteractable>() != null)
                player.interaction = collision.GetComponent<Interactions.IInteractable>();
        }
    }
}