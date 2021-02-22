using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    [CreateAssetMenu(fileName = "New Thought", menuName = "TotallyNotEvil/Thought")]
    public class Thought : ScriptableObject
    {
        [TextArea]
        public string thoughtText;
    }
}