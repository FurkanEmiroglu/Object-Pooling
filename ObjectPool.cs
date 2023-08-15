using System.Collections.Generic;
using UnityEngine;

namespace FE.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly int expand_step = 10;

        private readonly Queue<T> m_items;
        
        private readonly Transform m_parent;

        private readonly T m_prefab;

        public ObjectPool(T prefab, int count)
        {
            m_items = new Queue<T>(count);
            m_prefab = prefab;
            InitializePool(prefab, count);
        }

        public ObjectPool(T prefab, int count, Transform parent)
        {
            m_items = new Queue<T>(count);
            m_prefab = prefab;
            m_parent = parent;
            InitializePool(prefab, count, parent);
        }
        public ObjectPool(T prefab, int count, int expandStep, Transform parent)
        {
            m_items = new Queue<T>(count);
            expand_step = expandStep;
            m_prefab = prefab;
            m_parent = parent;
            InitializePool(prefab, count, parent);
        }

        public T Get()
        {
            if (m_items.Count == 0)
            {
                int c = 0;
                if (expand_step == 0)
                {
                    Debug.LogWarning("Expansion step is 0, pool is empty!");
                }
                else
                {
                    while (c < expand_step)
                    {
                        InstantiateInstance(m_prefab, m_parent);
                        c++;
                    }
                }
            }

            return m_items.Dequeue();
        }

        public void Return(T t)
        {
            m_items.Enqueue(t);
        }

        private void InitializePool(T prefab, int count)
        {
            for (int i = 0; i < count; i++) InstantiateInstance(prefab);
        }

        private void InitializePool(T prefab, int count, Transform parent)
        {
            for (int i = 0; i < count; i++) InstantiateInstance(prefab, parent);
        }

        private void InstantiateInstance(T prefab)
        {
            T instance = Object.Instantiate(prefab);
            m_items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }

        private void InstantiateInstance(T prefab, Transform parent)
        {
            T instance = Object.Instantiate(prefab, parent, true);
            m_items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
    }
}