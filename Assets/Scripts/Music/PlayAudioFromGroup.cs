using JetBrains.Annotations;
using UnityEngine;

/*
*  Copyright (c) Jonathan Carter
*  E: jonathan@carter.games
*  W: https://jonathan.carter.games/
*/

namespace TotallyNotEvil
{
    public class PlayAudioFromGroup : MonoBehaviour
    {
        [SerializeField] private GameObject soundPrefab;

        public void PlayRandomFromGroup(AudioClip[] clips, float _volume = 1)
        {
            GameObject clip = Instantiate(soundPrefab);
            clip.GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
            clip.GetComponent<AudioSource>().volume = _volume;
            clip.GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            clip.GetComponent<AudioSource>().Play();
            Destroy(clip, clip.GetComponent<AudioSource>().clip.length);
        }

        public void PlayClip(AudioClip _clip, float _volume = 1)
        {
            GameObject clip = Instantiate(soundPrefab);
            clip.GetComponent<AudioSource>().clip = _clip;
            clip.GetComponent<AudioSource>().volume = _volume;
            clip.GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            clip.GetComponent<AudioSource>().Play();
            Destroy(clip, clip.GetComponent<AudioSource>().clip.length);
        }

        public void PlayFromTime(AudioClip _clip, float time, float _volume = 1)
        {
            GameObject clip = Instantiate(soundPrefab);
            clip.GetComponent<AudioSource>().clip = _clip;
            clip.GetComponent<AudioSource>().time = time;
            clip.GetComponent<AudioSource>().volume = _volume;
            clip.GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            clip.GetComponent<AudioSource>().Play();
            Destroy(clip, clip.GetComponent<AudioSource>().clip.length);


        }

        public void PlayWithDelay(AudioClip[] clips, float delay, float volume = 1)
        {
            GameObject clip = Instantiate(soundPrefab);
            clip.GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
            clip.GetComponent<AudioSource>().volume = volume;
            clip.GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            clip.GetComponent<AudioSource>().PlayDelayed(delay);                            // Only difference, played with a delay rather that right away
            Destroy(clip, clip.GetComponent<AudioSource>().clip.length);
        }
    }
}