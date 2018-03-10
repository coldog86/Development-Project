using System.Collections.Generic;
using UnityEngine;

namespace Object
{
    // Identifies the pool that a GameObject came from
    public class PooledObject : MonoBehaviour
    {
        public SimpleObjectPool ObjectPool;
    }

    // Simple object pooling class
    public class SimpleObjectPool : MonoBehaviour
    {
        [Header("Component")]
        public GameObject Prefab = null;

        private readonly Stack<GameObject> _inactivePrefabInstances = new Stack<GameObject>();

        // Return an instance of the prefab
        public GameObject GetObject()
        {
            GameObject spawnedGameObject = null;

            if (_inactivePrefabInstances.Count > 0)
                spawnedGameObject = _inactivePrefabInstances.Pop();
            else
            {
                spawnedGameObject = Instantiate(Prefab);

                var pooledObject = spawnedGameObject.AddComponent<PooledObject>();
                pooledObject.ObjectPool = this;
            }

            spawnedGameObject.SetActive(true);

            return spawnedGameObject;
        }

        // Return an instance of the prefab to the pool
        public void ReturnObject(GameObject gameObjectToReturn)
        {
            var pooledObject = gameObjectToReturn.GetComponent<PooledObject>();

            if (pooledObject && pooledObject.ObjectPool == this)
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