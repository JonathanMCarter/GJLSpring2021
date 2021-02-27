using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CarterGames.Assets.AudioManager;

/*
    This is an old script I made, not perfect, but saves writing it from scratch...

    Dialouge Script
    -=-=-=-=-=-=-=-=-=-=-=-

    Made by: Jonathan Carter
    Last Edited By: Jonathan Carter
    Date Edited Last: 6/10/19

    This script handles the dialouge manager, its not finished as I put it into the project as this was a summer project I didn't finish.
    So, some things may not work...

*/

namespace TotallyNotEvil.Dialogue
{
    public enum Styles
    {
        Default,
        TypeWriter,
        Custom,
    };

    public class DialogueScript : MonoBehaviour
    {
        // The Active Text File - This is used to populate the list when updated
        [Header("Current Dialouge File")]
        [Tooltip("This is the current dialouge text file selected by the script, if this isn't the file you called then something has gone wrong.")]
        public DialogueFile file;

        // References to the displayed name and text area
        [Header("UI Element For Story Character Name")]
        [Tooltip("The UI Text element that is going to be used in your project to hold the Story Characters Name when they are talking.")]
        [SerializeField] private Text dialName;

        [Header("UI Element that holds the character dialogue")]
        [Tooltip("The UI Text element that is going to hold the lines of dialouge for you story charcters.")]
        [SerializeField] private Text dialText;

        // Int to check which element in the Dialogue list is next to be displayed
        [SerializeField] private int dialStage = 0;

        // Checks is a courutine is running or not
        [SerializeField] private bool isCoR;

        public Styles displayStyle;

        [SerializeField] private bool inputPressed;
        [SerializeField] private bool requireInput = true;
        [SerializeField] internal bool fileHasEnded = false;
        private bool hasReadLine;

        [Header("Type Writer Settings")]
        [SerializeField] private int typeWriterCount = 1;


        // Stuff for events 'n' stuff 
        [SerializeField] private Coroutine pauseCo;

        // auto dial stuff
        int _lineCount = 0;
        int _placeInLine = 0;
        string _sentence = "";



        private void Start()
        {
            hasReadLine = true;
        }


        private void Update()
        {
            if (requireInput)
            {
                if (!isCoR && !hasReadLine)
                {
                    Debug.Log("type");
                    StartCoroutine(TypeWriter(.005f));
                }
            }
        }


        // Changes the active file in the script
        public void ChangeFile(DialogueFile input)
        {
            file = input;
            Reset();
        }


        // Reads the next line of the dialogue sequence
        public void DisplayNextLine()
        {
            hasReadLine = false;

            if (dialStage < file.names.Count)
            {
                if (displayStyle.Equals(Styles.Default))
                {
                    dialName.text = file.names[dialStage];
                    dialText.text = file.dialogue[dialStage];
                    dialStage++;
                    inputPressed = false;
                }
            }
            else
            {
                dialName.text = "";
                dialText.text = "";
                fileHasEnded = true;
            }
        }



        // Display Option - Type Writer Style
        private IEnumerator TypeWriter(float delay)
        {
            isCoR = true;

            string _sentence = "";

            if (dialStage <= file.names.Count)
            {
                if (file.dialogue[dialStage] != null)
                {
                    _sentence = file.dialogue[dialStage].ToString().Substring(0, typeWriterCount);
                }

                if (_sentence.Length == file.dialogue[dialStage].Length)
                {
                    _sentence = file.dialogue[dialStage].ToString();
                    dialText.text = _sentence;
                    inputPressed = false;
                    dialStage++;
                    typeWriterCount = 0;
                    hasReadLine = true;
                }
                else
                {
                    dialName.text = file.names[dialStage];
                    dialText.text = _sentence;
                    typeWriterCount++;
                }
            }
            else
            {
                dialName.text = "";
                dialText.text = "";
                fileHasEnded = true;
            }

            yield return new WaitForSeconds(delay);

            isCoR = false;
        }



        private IEnumerator WaitToHide()
        {
            if (file.durationToShow != null && file.durationToShow[dialStage] > 0)
                yield return new WaitForSeconds(file.durationToShow[dialStage]);
            else
                yield return new WaitForSeconds(2f);

            dialName.text = "";
            dialText.text = "";
        }



        public void Input()
        {
            DisplayNextLine();
        }



        public void Reset()
        {
            if (inputPressed) { inputPressed = false; }
            if (fileHasEnded) { fileHasEnded = false; }
            dialStage = 0;
        }


        public void AutoDial()
        {
            StopAllCoroutines();
            _lineCount = 0;
            _placeInLine = 0;
            StartCoroutine(ShowDialAuto());
        }



        private IEnumerator ShowDialAuto()
        {
            if (file.dialogue.Count > _lineCount)
            {
                PlayDialSounds();

                if (file.dialogue[_lineCount] == _sentence)
                {
                    _lineCount++;
                    _sentence = "";
                    _placeInLine = 0;
                    yield return new WaitForSeconds(file.durationToShow[_lineCount - 1]);
                    StartCoroutine(ShowDialAuto());
                }
                else
                {
                    _sentence = file.dialogue[_lineCount].Substring(0, _placeInLine);
                    dialName.text = file.names[_lineCount];
                    dialText.text = _sentence;
                    _placeInLine++;
                    yield return new WaitForSeconds(.025f);
                    StartCoroutine(ShowDialAuto());
                }
            }
            else
            {
                yield return new WaitForSeconds(file.timeUntilHide);
                dialName.text = "";
                dialText.text = "";
                fileHasEnded = true;
                StopDialAudio();
            }

            yield break;
        }


        private void PlayDialSounds()
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }


        private void StopDialAudio()
        {
            GetComponent<AudioSource>().Stop();
        }


        public void StopDial()
        {
            StopAllCoroutines();
            dialName.text = "";
            dialText.text = "";
            GetComponent<AudioSource>().Stop();
        }
    }
}