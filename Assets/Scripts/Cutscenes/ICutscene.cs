using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    /// <summary>
    /// Could use cinemachine, but not 100% farmiliar with it all yet, so gonna stick with what I know here and make my own shots.
    /// </summary>
    public interface ICutscene
    {
        bool IsCutsceneRunning { get; set; }
        Vector3[] Positions { get; set; }
        void StartCutscene();
    }
}