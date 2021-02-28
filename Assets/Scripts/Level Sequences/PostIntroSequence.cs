using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;
using TotallyNotEvil.Audio;
using CarterGames.Assets.AudioManager;


namespace TotallyNotEvil.Sequences
{
    public class PostIntroSequence : BaseLevelSequence, ILevelSequence
    {
        [Header("*** Not Base Class ***")]
        [SerializeField] private PlayerController player;
        [SerializeField] private Animator ghostMachine;
        [SerializeField] private Sprite ghost;
        [SerializeField] Person[] basementPeople;
        private AudioManager audio;

        private void Awake()
        {
            //player.am.SetActive(false);
        }

        private void Start()
        {
            audio = FindObjectOfType<AudioManager>();
        }


        public override void Function()
        {
            StartCoroutine(FunctionCO());
        }


        private IEnumerator FunctionCO()
        {
            player.am.transform.position = ghostMachine.transform.position;
            ghostMachine.SetTrigger("MakeGhost");
            audio.Play("Ghost machine zap");
            yield return new WaitForSeconds(3f);
            ghostMachine.enabled = false;
            ghostMachine.gameObject.GetComponent<SpriteRenderer>().sprite = ghost;
            player.am.SetActive(true);
            yield return new WaitForSeconds(1);
            player.orb.GetComponent<IDamagable>().CanTakeDamage = true;
            //Debug.Log(player.am.GetComponent<IDamagable>().CanTakeDamage);
            foreach (var person in basementPeople)
            {
                person.GetComponent<PeepsSteps>().IsMuted = false;
            }
        }
    }
}