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

        [Header("AI Stuff")]
        [SerializeField] private Vector2[] range;
        [SerializeField] private Vector2 posToMoveTo;
        [SerializeField] private bool canMove;

        private void Start()
        {
            canMove = true;
            GetGameObject = this.gameObject;
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }


        private void Update()
        {
            if (!IsPossessed && canMove)
            {
                // AI movement
                posToMoveTo = new Vector2(Random.Range(range[0].x, range[1].x), transform.position.y);
                StartCoroutine(ChooseMovePos());
                MoveAction(posToMoveTo);
            }
        }


        public void MoveAction(Vector2 dir)
        {
            if (IsPossessed)
                rb.velocity = new Vector2(dir.x * MoveSpeed, rb.velocity.y);
            else
                rb.position = Vector2.Lerp(transform.position, posToMoveTo, 5 * Time.deltaTime);
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


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Interactions.IInteractable>() != null)
                FindObjectOfType<PlayerController>().interaction = null;
        }


        private IEnumerator ChooseMovePos()
        {
            canMove = true;
            yield return new WaitForSeconds(Random.Range(2, 7));
            canMove = false;
        }
    }
}