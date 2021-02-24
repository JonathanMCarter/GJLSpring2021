using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;

namespace TotallyNotEvil.Sequences
{
    public class SequenceReader : MonoBehaviour
    {
        [SerializeField] private GameObject[] sequences;
        [SerializeField] private int pos;
        [SerializeField] private int sequencePos;

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
                sequencePos++;

                if (seq.Sequence.Length > sequencePos)
                    ProgressTutorial();
                else
                    seq.SequenceComplete = true;
            }
        }


        public void ProgressTutorial()
        {
            switch (seq.Sequence[sequencePos])
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
                case SequenceElements.Function:

                    seq.Function();

                    break;
                default:
                    break;
            }

            seq.RunCooldown(sequencePos);
        }


        public void NextSequence()
        {
            pos++;
            seq = sequences[pos].GetComponent<ILevelSequence>();
            sequencePos = 0;
            ProgressTutorial();
        }


        public void TriggerSpecificSequence(int value)
        {
            pos++;
            seq = sequences[value].GetComponent<ILevelSequence>();
            sequencePos = 0;
            ProgressTutorial();
        }


        public void TriggerSpecificSequence(GameObject value)
        {
            pos++;
            seq = value.GetComponent<ILevelSequence>();
            sequencePos = 0;
            ProgressTutorial();
        }
    }
}