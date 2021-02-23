using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TotallyNotEvil
{
    public class LevelTransition : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private bool manualTransition;
        [SerializeField] private Animator anim;


        private void Start()
        {
            if (manualTransition)
            {
                StartCoroutine(WaitThenChange(4));
            }
        }


        public void SetScene(string scene)
        {
            sceneName = scene;
        }


        public void Transition()
        {
            StartCoroutine(WaitThenChange());
        }


        private IEnumerator WaitThenChange(float delay = 2f)
        {
            yield return new WaitForSeconds(delay);

            if (manualTransition)
            {
                anim.SetTrigger("Close");
                yield return new WaitForSeconds(2f);
            }

            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}