using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TotallyNotEvil
{
    public class HasThought : MonoBehaviour, IThinkable
    {
        // thinks....
        public bool HasShownThought { get; set; }


        [SerializeField] private Thought _thought;
        public Thought CurrentThought { get { return _thought; } set { _thought = value; } }


        [SerializeField] private AssetReference _bubble;
        public AssetReference Bubble { get { return _bubble; } set { _bubble = value; } }


        private Transform bubbleCanvas;
        private AsyncOperationHandle<GameObject> operationHandle;
        private GameObject spawnedBubble;


        private void Start()
        {
            bubbleCanvas = GameObject.FindGameObjectWithTag("Thoughts").transform;
            SpawnBubble();
        }


        private void Update()
        {
            if (spawnedBubble)
                if (spawnedBubble.activeInHierarchy)
                    spawnedBubble.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        }


        public void ShowBubble()
        {
            spawnedBubble.SetActive(true);
            HasShownThought = true;
        }


        private void SpawnBubble()
        {
            operationHandle = Addressables.InstantiateAsync(Bubble, bubbleCanvas);
            operationHandle.Completed += handle =>
            {
                GameObject _go = handle.Result;
                _go.GetComponentInChildren<Text>().text = CurrentThought.thoughtText;
                _go.SetActive(false);
                spawnedBubble = _go;
            };
        }
    }
}