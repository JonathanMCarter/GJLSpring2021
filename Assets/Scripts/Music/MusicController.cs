using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarterGames.Assets.AudioManager;


namespace TotallyNotEvil.Audio
{
    // This script is vert janky as it was done last min when I could barely think, also it kept being a pain when it should've worked, hense no transitions....
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource[] sources;
        [SerializeField] private AudioClip[] clips;
        private bool hasFinishedFirst;
        [SerializeField] internal bool inBasement;
        [SerializeField] internal bool inOffice;
        [SerializeField] internal bool inCEOOffice;
        private bool isSecondLift;

        private void Start()
        {
            hasFinishedFirst = false;
            inBasement = true;
            inOffice = false;
            inCEOOffice = false;

            sources[0].clip = clips[0];
            sources[1].clip = clips[2];
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
        }


        public void PlayLiftMusic()
        {
            sources[0].Stop();
            sources[0].clip = clips[4];
            sources[0].Play();
        }

        public void ChangeMusic()
        {
            if (isSecondLift)
                ChangeToCEOMusic();
            else
                ChangeToOfficeMusic();
        }


        public void ChangeToOfficeMusic()
        {
            sources[0].Stop();
            sources[0].clip = clips[2];
            sources[0].Play();
            isSecondLift = true;
        }


        public void ChangeToCEOMusic()
        {
            sources[0].Stop();
            sources[0].clip = clips[3];
            sources[0].Play();
        }
    }
}