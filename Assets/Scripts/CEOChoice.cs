using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotallyNotEvil.Dialogue;
using UnityEngine.SceneManagement;

namespace TotallyNotEvil
{
    public class CEOChoice : MonoBehaviour
    {
        [SerializeField] private bool takeJob;
        [SerializeField] private DialogueFile ceoFile;
        [SerializeField] private DialogueFile[] choices;
        [SerializeField] private Animator fadeToBlack;
        [SerializeField] private Canvas optionsUI;

        private DialogueScript dial;
        private bool isCoR;

        public bool choiceMade;


        private void Start()
        {
            dial = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueScript>();
        }


        private void Update()
        {
            if (choiceMade)
            {
                if (takeJob)
                    TakeCEOJob();
                else
                    KillCEO();
            }

            if (dial.file.Equals(ceoFile) && dial.fileHasEnded && !choiceMade)
            {
                ShowOptions();
            }
        }


        public void ShowOptions()
        {
            optionsUI.enabled = true;
        }


        public void HideOptions()
        {
            optionsUI.enabled = false;
        }


        public void TakeCEOJob()
        {
            if (!isCoR)
                StartCoroutine(TakeCEOJobCo());
        }


        public void KillCEO()
        {
            if (!isCoR)
                StartCoroutine(KillCEOCo());
        }


        private IEnumerator TakeCEOJobCo()
        {
            isCoR = true;
            HideOptions();
            dial.ChangeFile(choices[0]);
            dial.AutoDial();
            fadeToBlack.SetBool("HasChosen", true);
            yield return new WaitForSeconds(7.5f);
            SceneManager.LoadScene("End");
        }


        private IEnumerator KillCEOCo()
        {
            isCoR = true;
            HideOptions();
            dial.ChangeFile(choices[1]);
            dial.AutoDial();
            fadeToBlack.SetBool("HasChosen", true);
            yield return new WaitForSeconds(7.5f);
            SceneManager.LoadScene("End");
        }
    }
}