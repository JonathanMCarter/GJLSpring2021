using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace TotallyNotEvil.LightEffects
{
    public class FireLightEffect : MonoBehaviour
    {
        [SerializeField] private Light2D fireLight;
        [SerializeField] private bool shouldFlicker;
        [SerializeField] private float[] flickerMinMax;

        private bool isCoR;


        private void OnDisable()
        {
            StopAllCoroutines();
        }


        void Start()
        {
            if (shouldFlicker && !isCoR) 
            {
                StartCoroutine(FlickerCO());
            }
        }


        private IEnumerator FlickerCO()
        {
            isCoR = true;
            fireLight.intensity = Random.Range(flickerMinMax[0], flickerMinMax[1]);
            yield return new WaitForSeconds(Random.Range(.05f, .5f));
            isCoR = false;
            StartCoroutine(FlickerCO());
        }
    }
}