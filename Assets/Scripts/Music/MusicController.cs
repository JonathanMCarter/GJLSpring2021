using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarterGames.Assets.AudioManager;


namespace TotallyNotEvil.Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource[] sources;
        [SerializeField] private AudioClip[] clips;
        private bool hasFinishedFirst;
        [SerializeField] internal bool inBasement;
        [SerializeField] internal bool inOffice;
        [SerializeField] internal bool inCEOOffice;

        private int stage; // 0 for basement -> 1 for lift -> 2 for office -> 3 for lift -> 4 for CEO

        private void Start()
        {
            hasFinishedFirst = false;
            inBasement = true;
            inOffice = false;
            inCEOOffice = false;
            stage = 0;

            sources[0].clip = clips[0];
            sources[1].clip = clips[4];
            sources[0].Play();
        }


        private void Update()
        {
            // basement & intro
            if (inBasement && !hasFinishedFirst)
            {
                if (sources[0].clip.Equals(clips[0]) && !sources[0].isPlaying)
                {
                    hasFinishedFirst = true;
                    sources[0].clip = clips[1];
                    sources[0].loop = true;
                    sources[0].Play();
                }
            }


            //// office
            //if (!inBasement && inOffice)
            //{
            //    BasementToOffice();
            //}

            //// ceo
            //if (!inBasement && !inOffice && inCEOOffice)
            //{
            //    OfficeToCEO();
            //}

            switch (stage) {
                case 1:
                    BasementToLift();
                    break;
                case 2:
                    LiftToOffice();
                    break;
                case 3:
                    OfficeToLift();
                    break;
                case 4:
                    LiftToCEO();
                    break;
                default:
                    break;
            }
        }


        public void BasementToOffice()
        {
            if (!sources[1].isPlaying)
                sources[1].Play();

            if (sources[0].volume > 0)
            {
                sources[0].volume -= 1 * Time.deltaTime;
                sources[1].volume += 1 * Time.deltaTime;
            }
        }

        public void BasementToLift()
        {
            if (!sources[1].isPlaying)
                sources[1].Play();

            if (sources[0].volume > 0)
            {
                sources[0].volume -= 1 * Time.deltaTime;
                sources[1].volume += 1 * Time.deltaTime;
            }
        }

        public void LiftToOffice()
        {
            if (!sources[0].clip.Equals(clips[2]))
            {
                sources[0].clip = clips[2];
            }
            if (!sources[0].isPlaying) {
                sources[0].Play();
            }

            if (sources[1].volume > 0)
            {
                sources[1].volume -= 1 * Time.deltaTime;
                sources[0].volume += 1 * Time.deltaTime;
            }
        }

        public void OfficeToLift()
        {
            if (!sources[1].isPlaying)
                sources[1].Play();

            if (sources[0].volume > 0)
            {
                sources[0].volume -= 1 * Time.deltaTime;
                sources[1].volume += 1 * Time.deltaTime;
            }
        }

        public void LiftToCEO()
        {
            if (!sources[0].clip.Equals(clips[3])) {
                sources[0].clip = clips[3];
            }
            if (!sources[0].isPlaying)
            {
                sources[0].Play();
            }

            if (sources[1].volume > 0)
            {
                sources[1].volume -= 1 * Time.deltaTime;
                sources[0].volume += 1 * Time.deltaTime;
            }
        }

        public void OfficeToCEO()
        {
            sources[0].clip = clips[3];
            sources[0].Play();

            if (sources[1].volume > 0)
            {
                sources[1].volume -= 1 * Time.deltaTime;
                sources[0].volume += 1 * Time.deltaTime;
            }
        }

        public void IncrementStage() {
            stage += 1;
            if (stage > 4) {
                Debug.LogError("stage index too big");
            }
        }
    }
}