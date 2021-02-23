using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    [CreateAssetMenu(fileName = "New Thought", menuName = "Totally Not Evil/Thought")]
    public class Thought : ScriptableObject
    {
        [TextArea]
        public string thoughtText;
    }
}