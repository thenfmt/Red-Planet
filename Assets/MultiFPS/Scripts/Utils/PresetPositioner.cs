using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MultiFPS.Scripts.Utils
{
    public class PresetPositioner : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private Vector3 position;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 scale;

        [Space] 
        [SerializeField] private bool isLocal;
        [SerializeField] private bool isResetOnEnable;

        private void OnEnable()
        {
            if(isResetOnEnable)
            {
                Reset();
            }
        }

        [Button]
        public void Setup()
        {
            scale = transform.localScale;
            position = isLocal ? transform.localPosition : transform.position;
            rotation = isLocal ? transform.localRotation : transform.rotation;
        }

        public void Reset()
        {
            transform.localScale = scale;
            if (isLocal)
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }
    }
}