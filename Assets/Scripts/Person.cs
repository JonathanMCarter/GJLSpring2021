using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TotallyNotEvil
{
    public class Person : MonoBehaviour, IPossessable
    {
        [SerializeField] private bool _possessed;
        public bool IsPossessed { get { return _possessed; } set { _possessed = value; } }
        public GameObject GetGameObject { get; set; }


        private void Start()
        {
            GetGameObject = this.gameObject;
        }
    }
}