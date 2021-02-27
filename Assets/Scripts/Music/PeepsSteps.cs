using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Audio
{
    public class PeepsSteps : PlayAudioFromGroup
    {
        [SerializeField] private AudioClip[] clips;

        public void FootSteps()
        {
            base.PlayRandomFromGroup(clips, .75f);
        }
    }
}