using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Audio
{
    public class PeepsSteps : PlayAudioFromGroup
    {
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private bool _muted;
        public bool IsMuted { get { return _muted; } set { _muted = value; } }

        public void FootSteps()
        {
            if (!_muted)
            {
                base.PlayRandomFromGroup(clips, .75f);
            }
        }
    }
}