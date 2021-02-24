using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TotallyNotEvil.Dialogue;

namespace TotallyNotEvil
{
    public class TrackCutscene : MonoBehaviour, ICutscene
    {
        [SerializeField] private float duration;
        [SerializeField] private float size = 20f;
        [SerializeField] private CinemachineVirtualCamera cam;
        [SerializeField] private GameObject toFollowAfter;

        // Dialouge to play during cutscene.
        [SerializeField] private DialogueFile file;
        [SerializeField] private DialogueScript dial;


        // ICutscene
        [SerializeField] private Vector3[] _positions;
        public Vector3[] Positions { get { return _positions; } set { _positions = value; } }

        public bool IsCutsceneRunning { get; set; }
        [SerializeField] private Sequences.SequenceReader tut;
        private bool tutorialCalled;

        private void Start()
        {
            // temp
            StartCutscene();
        }


        private void Update()
        {
            if (!IsCutsceneRunning && !tutorialCalled)
            {
                tut.ProgressTutorial();
                tutorialCalled = true;
            }

            if (!toFollowAfter)
                toFollowAfter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().am;
        }


        public void StartCutscene()
        {
            transform.position = _positions[0];
            IsCutsceneRunning = true;
            StartCoroutine(LerpToPosition());
            dial.ChangeFile(file);
            dial.AutoDial();
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