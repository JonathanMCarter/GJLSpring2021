using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TotallyNotEvil
{
    public class TrackCutscene : MonoBehaviour, ICutscene
    {
        [SerializeField] private float duration;
        [SerializeField] private float size = 20f;
        [SerializeField] private CinemachineVirtualCamera cam;
        [SerializeField] private GameObject toFollowAfter;

        [SerializeField] private Vector3[] _positions;
        public Vector3[] Positions { get { return _positions; } set { _positions = value; } }

        public bool IsCutsceneRunning { get; set; }


        private void Start()
        {
            // temp
            StartCutscene();
        }


        public void StartCutscene()
        {
            transform.position = _positions[0];
            IsCutsceneRunning = true;
            StartCoroutine(LerpToPosition());
        }


        private IEnumerator LerpToPosition()
        {
            float _time = 0;

            while (_time < duration)
            {
                transform.position = Vector3.Lerp(Positions[0], Positions[1], _time / duration);

                // dumb but works 
                cam.m_Lens.OrthographicSize = Vector2.Lerp(new Vector2(size, size), new Vector2(5f, 5f), _time / duration).x;

                _time += Time.deltaTime;
                yield return null;
            }

            transform.position = Positions[1];
            IsCutsceneRunning = false;

            yield return new WaitForSeconds(1.5f);

            // makes the camera follow the player after the cutscene is finished.
            cam.LookAt = toFollowAfter.transform;
            cam.Follow = toFollowAfter.transform;
        }
    }
}