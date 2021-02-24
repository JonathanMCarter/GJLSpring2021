using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;

namespace TotallyNotEvil
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private GameObject[] sequences;
        [SerializeField] private int pos;

        [SerializeField] private DialogueScript dial;
        ILevelSequence seq = null;


        private void Start()
        {
            if (sequences[pos].GetComponent<ILevelSequence>() != null) seq = sequences[pos].GetComponent<ILevelSequence>();
        }


        private void Update()
        {
            if (seq.CanProgress)
            {
                pos++;

                if (seq.Sequence.Length > pos)
                    ProgressTutorial();
                else
                    seq.SequenceComplete = true;
            }
        }


        public void ProgressTutorial()
        {
            switch (seq.Sequence[pos])
            {
                case SequenceElements.None:
                    // nothing xD
                    break;
                case SequenceElements.Dialogue:

                    dial.ChangeFile(seq.File);
                    dial.AutoDial();

                    break;
                case SequenceElements.UIPrompt:

                    seq.Prompt.SetActive(true);

                    break;
                default:
                    break;
            }

            seq.RunCooldown(pos);
        }
    }
}