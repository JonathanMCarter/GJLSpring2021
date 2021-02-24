using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private Rigidbody2D rb;


        void Start()
        {
            GetGameObject = this.gameObject;
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsPossessed && !possessionEventRan)
            {
                e.Invoke();
                possessionEventRan = true;
            }
        }



        public void MoveAction(Vector2 dir)
        {
            rb.velocity = new Vector2(dir.x * MoveSpeed, rb.velocity.y);
        }


        public void JumpAction()
        {
            // Rats can't jump, just for the sake of less work xD.
        }
    }
}