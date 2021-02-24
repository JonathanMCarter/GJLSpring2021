using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Sequences
{
    public class MakeRatMovableSequence : BaseLevelSequence, ILevelSequence
    {
        [SerializeField] private GameObject rat;

        public override void Function()
        {
            rat.GetComponent<IMoveable>().MoveSpeed = 4.5f;
        }
    }
}