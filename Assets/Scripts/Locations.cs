using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TotallyNotEvil
{
    public class Locations : MonoBehaviour
    {
        private Text locationTxt;
        private Animator anim;
        

        private void Start()
        {
            locationTxt = GetComponentInChildren<Text>();
            anim = GetComponent<Animator>();
        }


        public void SetLocation(string location)
        {
            anim.SetTrigger("ShowLocation");
            locationTxt.text = location;
        }
    }
}