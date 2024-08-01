using MultiFPS.Gameplay.Gamemodes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS.UI.HUD
{
    public class UIScoreBoardPlayerElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtPlayerName;
        [SerializeField] private TextMeshProUGUI txtKills;
        [SerializeField] private TextMeshProUGUI txtDeaths;
        [SerializeField] private TextMeshProUGUI txtLatency;

        [Space] 
        [SerializeField] private Image imgBackground;


        public void Show(bool isShow)
        {
            gameObject.SetActive(isShow);
        }
        
        public void WriteData(string nick, int kills, int deaths, int team, short latency)
        {
            txtPlayerName.text = nick;
            txtKills.text = kills.ToString();
            txtDeaths.text = deaths.ToString();
            txtLatency.text = latency.ToString();

            //assign appropriate color for player in scoreboard depending on team, if player is not in any team, give him white color
            Color teamColor = team == -1 ? Color.white : ClientInterfaceManager.Instance.UIColorSet.TeamColors[team];

            imgBackground.color = teamColor;
            // txtPlayerName.color = teamColor;
            // txtKills.color = teamColor;
            // txtDeaths.color = teamColor;
        }
    }
}