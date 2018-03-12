using System.Collections.Generic;
using UnityEngine;

namespace Object
{
    // Identifies the pool that a GameObject came from
    public class PooledObject : MonoBehaviour
    {
        public SimpleObjectPool objectPool;
    }

    // Simple object pooling class
    public class SimpleObjectPool : MonoBehaviour
    {
        [Header("Component")]
        public GameObject answerButtonPrefab;

        private Stack<GameObject> _inactivePrefabInstances = new Stack<GameObject>();

        // Return an instance of the prefab
        public GameObject GetObject()
        {
            GameObject spawnedGameObject = null;

            if (_inactivePrefabInstances.Count > 0)
                spawnedGameObject = _inactivePrefabInstances.Pop();
            else
            {
                spawnedGameObject = Instantiate(answerButtonPrefab);

                PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
                pooledObject.objectPool = this;
            }

            spawnedGameObject.SetActive(true);

            return spawnedGameObject;
        }

        // Return an instance of the prefab to the pool
        public void ReturnObject(GameObject gameObjectToReturn)
        {
            PooledObject pooledObject = gameObjectToReturn.GetComponent<PooledObject>();

            if (pooledObject && pooledObject.objectPool == this)
            {
                gameObjectToReturn.SetActive(false);
                _inactivePrefabInstances.Push(gameObjectToReturn);
            }
            else
            {
                Debug.LogWarning(gameObjectToReturn.name + " was returned to a pool.");
                Destroy(gameObjectToReturn);
            }
        }
    }
}