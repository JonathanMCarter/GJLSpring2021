using System.Collections.Generic;
using UnityEngine;

/*
    Dialouge File Scriptable Object
    -=-=-=-=-=-=-=-=-=-=-=-

    Made by: Jonathan Carter
    Last Edited By: Jonathan Carter
    Date Edited Last: 6/10/19

    While this could be defined in the same script, I kept it seperate for now,
    This just has 2 list of string that hold the dialogue you input from the editor window

*/

namespace TotallyNotEvil.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue File", menuName = "Carter Games/Dialogue File")]
    public class DialogueFile : ScriptableObject
    {
        public List<string> names;
        public List<string> dialogue;
    }
}