﻿using UnityEngine;

namespace Yudiz.StarterKit.Utilities
{
    public class IndestructibleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
                OnAwake();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void OnAwake()
        {

        }
    }
}