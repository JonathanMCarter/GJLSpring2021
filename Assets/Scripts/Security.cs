using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public class Security : MonoBehaviour
    {
        private Person person;
        private Rigidbody2D rb;


        private void Start()
        {
            person = GetComponent<Person>();
            rb = GetComponent<Rigidbody2D>();
        }


        // Update is called once per frame
        void Update()
        {
            if (person.IsPossessed && rb.constraints.HasFlag(RigidbodyConstraints2D.FreezePositionX))
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            else if (!person.IsPossessed && !rb.constraints.HasFlag(RigidbodyConstraints2D.FreezePositionX))
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}