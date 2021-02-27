using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;

namespace TotallyNotEvil.Sequences
{
    public class PostIntroSequence : BaseLevelSequence, ILevelSequence
    {
        [Header("*** Not Base Class ***")]
        [SerializeField] private PlayerController player;
        [SerializeField] private Animator ghostMachine;
        [SerializeField] private Sprite ghost;

        private void Awake()
        {
            //player.am.SetActive(false);
        }


        public override void Function()
        {
            StartCoroutine(FunctionCO());
        }


        private IEnumerator FunctionCO()
        {
            player.am.transform.position = ghostMachine.transform.position;
            ghostMachine.SetTrigger("MakeGhost");
            yield return new WaitForSeconds(3f);
            ghostMachine.enabled = false;
            ghostMachine.gameObject.GetComponent<SpriteRenderer>().sprite = ghost;
            player.am.SetActive(true);
            player.am.GetComponent<IDamagable>().CanTakeDamage = true;
        }
    }
}