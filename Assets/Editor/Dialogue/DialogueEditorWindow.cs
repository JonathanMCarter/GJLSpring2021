using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
    You shouldn't be here.....
    If something throws an error that stops you working then let me know...


    Dialouge Editor Window Script
    -=-=-=-=-=-=-=-=-=-=-=-

    Made by: Jonathan Carter
    Last Edited By: Jonathan Carter
    Date Edited Last: 09/11/19 - 

    Edit History:
    - 15/10/19 - Allowed for options when adding an event into the file line
    - 13/10/19 - Added buttons for events, these are only placeholder for now, but will do stuff once the option for events are implemented
    - 6/10/19 - To add this comment bit in (nothing else was changed)

    This script adds a new editor tab / window into the editor. This can be found under "Tools/Dialogue Editor" if it is not already open.
    The entire script makes that window work, allowing you to make and edit dialogue files in a nicer way,


    NOTE: This was made over summer for an asset store project that I've yet to finish, so it has branding as if I was to release it in the script, just ignore this stuff...

*/

namespace TotallyNotEvil.Dialogue
{
    public class DialogueEditorWindow : EditorWindow
    {
        public DialogueFile currentFile;
        private bool showFile;
        private int tab;

        private bool fileCreated;
        private bool createFile;

        private string fileName;
        private string textInputted;
        private int numberOfLines = 1;
        public List<string> newNames = new List<string>();
        public List<string> newDialogue = new List<string>();

        private Color32 addBtnColour = new Color32(55, 175, 95, 255);
        public Color32 removeBtnColour;

        public Rect deselectWindow;
        Vector2 scrollPos;

        public enum EventOptions
        {
            None,
            Cinematic,
            Pause,
            ExitDial,
            Animation
        };

        public EventOptions eventChoice;


        [MenuItem("Tools/Dialogue Editor Window", priority = 20)]
        public static void ShowWindow()
        {
            GetWindow<DialogueEditorWindow>("Dialogue Editor Window");
        }


        // The stuff that shows up in the window
        public void OnGUI()
        {
            deselectWindow = new Rect(0, 0, position.width, position.height);

            GUILayout.Space(15f);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            tab = GUILayout.Toolbar(tab, new string[] { "Create New File", "Edit Existing File" }, GUILayout.MaxWidth(250f), GUILayout.MaxHeight(25f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);

            newNames.Add("");
            newDialogue.Add("");

            switch (tab)
            {
                // If Creating a new file
                case 0:

                    GUILayout.Space(20);

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("File Name:", GUILayout.MaxWidth(65f));
                    fileName = EditorGUILayout.TextField(fileName);
                    GUILayout.FlexibleSpace();

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(10f);

                    // Displays the table headers
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("No:", GUILayout.MaxWidth(30f));
                    EditorGUILayout.LabelField("Character:", GUILayout.MaxWidth(200f));
                    EditorGUILayout.LabelField("Dialogue:", GUILayout.MinWidth(100f));
                    eventChoice = (EventOptions)EditorGUILayout.EnumPopup(eventChoice, GUILayout.MaxWidth(75f));

                    EditorGUILayout.EndHorizontal();

                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));

                    // Displays the table for the number of lines added to the file
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(30f));
                        newNames[i] = EditorGUILayout.TextField(newNames[i], GUILayout.MaxWidth(200f));
                        newDialogue[i] = EditorGUILayout.TextField(newDialogue[i]);

                        GUILayout.Space(10f);

                        GUI.backgroundColor = Color.yellow;

                        if (GUILayout.Button("~", GUILayout.Width(25)))
                        {
                            string choice;

                            switch (eventChoice)
                            {
                                case EventOptions.None:
                                    choice = "";
                                    break;
                                case EventOptions.Cinematic:
                                    choice = "###";
                                    break;
                                case EventOptions.Pause:
                                    choice = "@@@";
                                    break;
                                case EventOptions.ExitDial:
                                    choice = "***";
                                    break;
                                case EventOptions.Animation:
                                    choice = "^^^";
                                    break;
                                default:
                                    choice = "";
                                    break;
                            }

                            // this is where the event would be set, might make a enum to select a tye of event in the future...
                            newNames.Insert(i, choice);
                            newDialogue.Insert(i, choice);
                        }

                        GUI.backgroundColor = Color.green;
                        if (GUILayout.Button("+", GUILayout.Width(25)))
                        {
                            numberOfLines++;
                            newNames.Insert(i + 1, "");
                            newDialogue.Insert(i + 1, "");
                        }

                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("-", GUILayout.Width(25)))
                        {
                            if (i != 0)
                            {
                                numberOfLines--;
                                newNames.RemoveAt(i);
                                newDialogue.RemoveAt(i);
                            }
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                    }

                    // Creates a new Dialogue File with the name and text inputted
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Create File", GUILayout.MaxWidth(100f)))
                    {
                        createFile = true;
                    }
                    GUI.backgroundColor = Color.white;
                    if (GUILayout.Button("Reset File", GUILayout.MaxWidth(100f)))
                    {
                        ClearLists();
                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndScrollView();

                    break;

                // If editing an exsisting file
                case 1:

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Here you can edit already exsisting files as if you were creating them.");
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    // Label & Box for the user to select the Dialogue FIle they wish to edit
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("Dialogue File: ", GUILayout.MaxWidth(80f));
                    currentFile = (DialogueFile)EditorGUILayout.ObjectField(currentFile, typeof(DialogueFile), false);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    if (currentFile != null)
                    {
                        GUILayout.Space(20);

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField("Edit File Name:", GUILayout.MaxWidth(100f));
                        currentFile.name = EditorGUILayout.TextField(currentFile.name);
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(10);
                        EditorGUILayout.LabelField("Contents of File:", GUILayout.MaxWidth(100f));
                        GUILayout.Space(5);

                        // Table Headers
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("No:", GUILayout.MaxWidth(30f));
                        EditorGUILayout.LabelField("Character:", GUILayout.MaxWidth(200f));
                        EditorGUILayout.LabelField("Dialogue:", GUILayout.MinWidth(100f));
                        eventChoice = (EventOptions)EditorGUILayout.EnumPopup(eventChoice, GUILayout.MaxWidth(75f));
                        EditorGUILayout.EndHorizontal();

                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));

                        // Displays the current file's dialogue in the correct boxes and allow it to be edited
                        for (int i = 0; i < currentFile.names.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(30f));
                            currentFile.names[i] = EditorGUILayout.TextField(currentFile.names[i], GUILayout.MaxWidth(200f));
                            currentFile.dialogue[i] = EditorGUILayout.TextField(currentFile.dialogue[i], GUILayout.MaxWidth(400f));
                            GUILayout.Space(10);

                            GUI.backgroundColor = Color.yellow;

                            if (GUILayout.Button("~", GUILayout.Width(25)))
                            {
                                string Choice;

                                switch (eventChoice)
                                {
                                    case EventOptions.None:
                                        Choice = "";
                                        break;
                                    case EventOptions.Cinematic:
                                        Choice = "###";
                                        break;
                                    case EventOptions.Pause:
                                        Choice = "@@@";
                                        break;
                                    case EventOptions.ExitDial:
                                        Choice = "***";
                                        break;
                                    case EventOptions.Animation:
                                        Choice = "^^^";
                                        break;
                                    default:
                                        Choice = "";
                                        break;
                                }

                                // this is where the event would be set, might make a enum to select a tye of event in the future...
                                newNames.Insert(i, Choice);
                                newDialogue.Insert(i, Choice);
                            }

                            GUI.backgroundColor = Color.green;
                            if (GUILayout.Button("+", GUILayout.Width(25)))
                            {
                                numberOfLines++;
                                currentFile.names.Insert(i + 1, "");
                                currentFile.dialogue.Insert(i + 1, "");

                                Debug.Log("Adding new line");
                            }
                            GUI.backgroundColor = Color.red;
                            if (GUILayout.Button("-", GUILayout.Width(25)))
                            {
                                if (i != 0)
                                {
                                    numberOfLines--;
                                    currentFile.names.RemoveAt(i);
                                    currentFile.dialogue.RemoveAt(i);
                                }
                            }
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    else
                    { EditorGUILayout.LabelField("No Dialogue File Selected!"); }
                    break;
                default:
                    break;
            }

            // Makes it so you can deselect elements in the window by adding a button the size of the window that you can't see under everything
            //make sure the following code is at the very end of OnGUI Function
            if (GUI.Button(deselectWindow, "", GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }


        private void Update()
        {
            if (createFile)
            {
                CreateNewFile();
                createFile = false;
            }
        }


        private void CreateNewFile()
        {
            DialogueFile asset = ScriptableObject.CreateInstance<DialogueFile>();

            asset.names = new List<string>();
            asset.dialogue = new List<string>();

            for (int i = 0; i < numberOfLines; i++)
            {
                asset.names.Add(newNames[i]);
                asset.dialogue.Add(newDialogue[i]);
            }

            AssetDatabase.CreateAsset(asset, "Assets/Dialogue/" + fileName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            ClearLists();
        }


        private void ClearLists()
        {
            if (fileName != null)
            {
                fileName = "";
            }

            newNames.Clear();
            newDialogue.Clear();
            numberOfLines = 1;
            newNames.Add("");
            newDialogue.Add("");
            eventChoice = EventOptions.None;
        }

        // Old Function : Checked to see if the window size was too small to display stuff correctly, however as I got a scroll view working this isn't needed
        //private bool CheckSize()
        //{
        //    if (position.width < 400f)
        //    {
        //        EditorGUILayout.LabelField("Window is too small to display... please make the window bigger");
        //        return false;
        //    }
        //    else if (position.height < 150f)
        //    {
        //        EditorGUILayout.LabelField("Window is too small to display... please make the window bigger");
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}