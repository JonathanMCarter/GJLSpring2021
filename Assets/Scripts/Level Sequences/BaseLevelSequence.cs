using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;


namespace TotallyNotEvil.Sequences
{
    public class BaseLevelSequence : MonoBehaviour
    {
        [SerializeField] private SequenceElements[] _elements;
        public SequenceElements[] Sequence { get { return _elements; } set { _elements = value; } }

        [SerializeField] private float[] _delay;
        public float[] DelayBetweenSequence { get { return _delay; } set { _delay = value; } }

        [SerializeField] private DialogueFile _file;
        public DialogueFile File { get { return _file; } set { _file = value; } }

        [SerializeField] private GameObject _prompt;
        public GameObject Prompt { get { return _prompt; } set { _prompt = value; } }

        [SerializeField] private bool _complete;
        public bool SequenceComplete { get { return _complete; } set { _complete = value; } }

        public bool CanProgress { get; set; }
        private bool isCoR;


        public void RunCooldown(int pos)
        {
            if (!isCoR)
                StartCoroutine(CooldownCO(pos));
        }


        public virtual void Function()
        {
        }


        private IEnumerator CooldownCO(int pos)
        {
            isCoR = true;
            CanProgress = false;
            yield return new WaitForSeconds(DelayBetweenSequence[pos]);
            CanProgress = true;
            isCoR = false;
        }
    }
}