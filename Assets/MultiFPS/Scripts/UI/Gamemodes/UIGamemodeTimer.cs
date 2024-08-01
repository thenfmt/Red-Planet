using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// clock that shows by UI time to start/end round
/// </summary>
namespace MultiFPS.UI.Gamemodes
{
    public class UIGamemodeTimer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textTimer;
        
        
        private void Awake()
        {
            _textTimer.text = "00:00";
        }


        public void UpdateTimer(int seconds)
        {
            int minutes = 0;

            while (seconds >= 60)
            {
                seconds -= 60;
                minutes++;
            }
            _textTimer.text = minutes.ToString() + ":" + (seconds < 10 ? "0" : "") + seconds.ToString();
        }
    }
}
