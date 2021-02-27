using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TotallyNotEvil
{
    public static class Death
    {
        public static void PlayerHasDeath(string causeOfDeath = "")
        {
            // refs
            GameObject _go = GameObject.FindGameObjectWithTag("DeathUI");
            Animator _anim = _go.GetComponent<Animator>();
            Text _causeTxt = _go.GetComponentsInChildren<Text>()[1];
            Dialogue.DialogueScript _dial = GameObject.FindObjectOfType<Dialogue.DialogueScript>();

            // stop dial
            _dial.StopDial();

            // actions
            _anim.SetTrigger("IsDead");
            _causeTxt.text = causeOfDeath;
        }
    }
}