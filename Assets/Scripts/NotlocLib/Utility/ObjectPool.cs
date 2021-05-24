using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Notloc.Utility
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        Queue<T> availableObjects = new Queue<T>();

        private T prefab;
        private Transform parent;

        public void Initialize(T prefab, int count)
        {
            this.prefab = prefab;
            parent = new GameObject("ObjPool - " + prefab.GetType().ToString()).transform;
            parent.gameObject.hideFlags = HideFlags.HideAndDontSave;

            for (int i = 0; i < count; i++)
            {
                T newObj = Object.Instantiate(prefab, parent);
                newObj.gameObject.SetActive(false);
                availableObjects.Enqueue(newObj);
            }
        }

        public T Get()
        {
            if (availableObjects.Count > 0)
                return availableObjects.Dequeue();
            else
                return Object.Instantiate(prefab, parent);
        }

        public List<T> Get(int count)
        {
            List<T> objs = new List<T>();
            if (availableObjects.Count >= count)
            {
                for (int i = 0; i < count; i++)
                    objs.Add(availableObjects.Dequeue());
            }
            else
            {
                int available = availableObjects.Count;
                for (int i = 0; i < available; i++)
                    objs.Add(availableObjects.Dequeue());
                int needToCreate = count - available;
                for (int i = 0; i < needToCreate; i++)
                    objs.Add(Object.Instantiate(prefab, parent));
            }
            return objs;
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(parent, false);
            availableObjects.Enqueue(obj); 
        }

        public void ReturnToPool(ICollection<T> objs)
        {
            foreach (var obj in objs)
            {
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(parent, false);
                availableObjects.Enqueue(obj);
            }
        }
    }
}