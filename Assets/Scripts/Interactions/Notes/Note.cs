using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil.Interactions
{
    [CreateAssetMenu(fileName = "New Note", menuName = "Totally Not Evil/Note")]
    public class Note : ScriptableObject
    {
        [TextArea]
        public string noteContents;
    }
}