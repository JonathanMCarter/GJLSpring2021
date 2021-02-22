using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TotallyNotEvil
{
    public class UIToggle : MonoBehaviour
    {
        [SerializeField] private GameObject[] ui;

        private Actions actions;
        private InputDevice device;


        private void OnEnable()
        {
            actions = new Actions();

            // Input Check
            InputSystem.onActionChange += (obj, change) =>
            {
                ControllerChanged(obj, change);
            };

            actions.Enable();
        }


        private void OnDisable()
        {
            actions.Disable();
        }


        void Update()
        {
            if (device != null && (device.displayName.Equals("Mouse") || device.displayName.Equals("Keyboard")) && !ui[0].activeSelf)
            {
                ui[0].SetActive(true);
                ui[1].SetActive(false);
            }
            else if (device != null && (device.displayName.Contains("Xbox") && !ui[1].activeSelf))
            {
                ui[1].SetActive(true);
                ui[0].SetActive(false);
            }
        }


        /// <summary>
        /// Checks to see if the control input has changed (mostly for plugging in a controller etc).
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="change"></param>
        private void ControllerChanged(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                device = lastControl.device;
            }
        }
    }
}