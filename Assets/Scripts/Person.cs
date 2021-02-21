using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotallyNotEvil
{
    public class Person : MonoBehaviour, IPocessable
    {
        public bool IsPocessed { get; set; }
        public GameObject obj { get; set; }


        private void Start()
        {
            obj = this.gameObject;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsPocessed)
            {
                if (collision.GetComponent<IPocessable>() != null)
                {
                    if (!collision.GetComponent<IPocessable>().IsPocessed)
                    {
                        Debug.Log("dkhfdjhf");
                        collision.GetComponent<IPocessable>().IsPocessed = true;
                        FindObjectOfType<PlayerController>().SetAm(collision.GetComponent<IPocessable>());
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}