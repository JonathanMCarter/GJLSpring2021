using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        public Text dialName;

        [Header("UI Element that holds the character dialogue")]
        [Tooltip("The UI Text element that is going to hold the lines of dialouge for you story charcters.")]
        public Text dialText;

        // Int to check which element in the Dialogue list is next to be displayed
        private int dialStage = 0;

        // Checks is a courutine is running or not
        private bool isCoR;

        public Styles displayStyle;

        public bool inputPressed;
        public bool requireInput = true;
        public bool fileHasEnded = false;

        [Header("Characters used to define file read settings")]
        [Tooltip("This should match what you inputted into the 'File Read Settings' char after name, which controls where a story character's name ends in the dialouge files the script reads.")]
        public string nameChar = ":";
        [Tooltip("This should match what you inputted into the 'File Read Settings' char for new line, which controls where a story character's line of dialouge ends and a new one begins.")]
        public string newLineChar = "#";

        [Header("Type Writer Settings")]
        public int typeWriterCount = 1;


        // Stuff for events 'n' stuff 
        private Coroutine pauseCo;
        public Animator animToPlay;


        private void Update()
        {
            if (requireInput)
            {
                switch (displayStyle)
                {
                    case Styles.Default:

                        if (inputPressed)
                            DisplayNextLine();

                        break;
                    case Styles.TypeWriter:

                        if ((!isCoR) && (inputPressed))
                            StartCoroutine(TypeWriter(.005f));

                        break;
                    default:
                        break;
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
            if (dialStage < file.names.Count)
            {
                switch (file.names[dialStage])
                {
                    // Pause
                    case "@@@":
                        // Pauses dialogue for a little bit (for dramatic effect..............................................)
                        if (pauseCo == null)
                            pauseCo = StartCoroutine(PauseDial(3));

                        break;

                    // Play Animation
                    case "^^^":
                        // Put code to play animation
                        animToPlay.Play(file.dialogue[dialStage], -1);
                        break;

                    // End Dialogue
                    case "***":
                        dialStage = 0;
                        fileHasEnded = true;
                        break;

                    // Read Dial as normal
                    default:
                        dialName.text = file.names[dialStage];
                        dialText.text = file.dialogue[dialStage];
                        dialStage++;
                        inputPressed = false;
                        break;
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

            if (dialStage < file.names.Count)
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



        public void Input()
        {
            // if (!InputPressed) { InputPressed = true; }
            DisplayNextLine();
        }



        public void Reset()
        {
            if (inputPressed) { inputPressed = false; }
            if (fileHasEnded) { fileHasEnded = false; }
            dialStage = 0;
        }


        private IEnumerator PauseDial(float Delay)
        {
            yield return new WaitForSeconds(Delay);
            ++dialStage;
        }
    }
}