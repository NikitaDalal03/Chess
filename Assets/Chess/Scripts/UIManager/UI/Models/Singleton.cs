using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.StarterKit.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            Instance = this as T;
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }
    }
}