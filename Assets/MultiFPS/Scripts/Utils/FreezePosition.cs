using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MultiFPS.UI
{
    // This script is to fix animation move
    public class FreezePosition: MonoBehaviour
    {
        [SerializeField] private bool isFreezeX;
        [SerializeField] private bool isFreezeY;
        [SerializeField] private bool isFreezeZ;
        
        [Space, SerializeField] private Vector3 targetPos;
        
        private void LateUpdate()
        {
            transform.localPosition = new Vector3(
                isFreezeX ? targetPos.x : transform.localPosition.x,
                isFreezeY ? targetPos.y : transform.localPosition.y,
                isFreezeZ ? targetPos.z : transform.localPosition.z
                );
        }
    }
}