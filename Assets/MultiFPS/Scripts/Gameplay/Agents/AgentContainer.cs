using System.Collections;
using System.Collections.Generic;
using MultiFPS.Gameplay;
using UnityEngine;
namespace MultiFPS
{
    [CreateAssetMenu(fileName = "Agent", menuName = "MultiFPS/Agent")]
    public class AgentContainer : ScriptableObject
    {
        public int SpawnID;
        public string AgentName;

        [Space] 
        public Sprite icon;
        
        [Space]
        public GameObject MenuPrefab;
    }
}