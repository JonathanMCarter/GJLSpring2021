using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;


namespace TotallyNotEvil
{
    public enum SequenceElements { None = 0, Dialogue, UIPrompt, Function }
    public interface ILevelSequence
    {
        SequenceElements[] Sequence { get; set; }
        float[] DelayBetweenSequence { get; set; }
        DialogueFile File { get; set; }
        GameObject Prompt { get; set; }
        bool SequenceComplete { get; set; }
        bool CanProgress { get; set; }
        void RunCooldown(int pos);
        void Function();
    }
}