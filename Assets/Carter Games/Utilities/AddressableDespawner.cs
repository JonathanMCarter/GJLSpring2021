using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/****************************************************************************************************************************
 * 
 *  --{   Carter Games Utilities Script   }--
 *							  
 *  Addressables Despawner
 *	    Automatically despawns an addressable object after a set time.
 *			
 *  Written by:
 *      Jonathan Carter
 *      E: jonathan@carter.games
 *      W: https://jonathan.carter.games
 *			        
 *	Last Updated: 22/02/2021 (d/m/y)						
 * 
****************************************************************************************************************************/

namespace CarterGames.Utilities
{
    public class AddressableDespawner : MonoBehaviour
    {
        /// <summary>
        /// Float | defines how long the object has before it is removed.
        /// </summary>
        [Header("Despawn Delay")]
        [Tooltip("Set this to define how long the object will wait before despawning. Default Value = 1")]
        [SerializeField] private float despawnTime = 1f;


        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unity OnEnable | When the object is enabled, start the corutine that will despawn the object.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            StartCoroutine(DespawnCo());
        }


        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unity OnDisable | When the object is disabled, stop all corutines so it doesn't run more than it needs to.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OnDisable()
        {
            StopAllCoroutines();
        }


        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Coroutine | Despawns the object this is attached to as and when it is enabled.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private IEnumerator DespawnCo()
        {
            // waits for the defined time.
            yield return new WaitForSeconds(despawnTime);

            // releases and removes the asset.
            Addressables.ReleaseInstance(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}