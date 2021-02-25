using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace TotallyNotEvil
{
    public class PauseScript : MonoBehaviour
    {
        [SerializeField] private Animator pauseUI;
        private Actions actions;
        public bool isPaused;


        private void OnEnable()
        {
            actions = new Actions();
            actions.Control.Pause.performed += TogglePause;
        }


        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseUI.SetBool("PauseGame", true);
        }


        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseUI.SetBool("PauseGame", false);
        }


        public void TogglePause(InputAction.CallbackContext ctx)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
}