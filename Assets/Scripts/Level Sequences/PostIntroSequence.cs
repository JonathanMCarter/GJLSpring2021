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

        public override void Function()
        {
            player.am.GetComponent<IDamagable>().CanTakeDamage = true;
        }
    }
}