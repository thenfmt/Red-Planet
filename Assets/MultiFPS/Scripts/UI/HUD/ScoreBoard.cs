using MultiFPS.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MultiFPS;

namespace MultiFPS.UI.HUD
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private GameObject scoreBoard;
        [SerializeField] private Transform content;
        [SerializeField] private UIScoreBoardPlayerElement playerPresenter;

        private List<UIScoreBoardPlayerElement> _instantiatedPresenters = new List<UIScoreBoardPlayerElement>();

        Coroutine c_refresher;

        void Start()
        {
            playerPresenter.Show(false);
            _instantiatedPresenters.Add(playerPresenter);
        }

        IEnumerator RefreshScoreboard()
        {
            while (true)
            {
                List<PlayerInstance> players = new List<PlayerInstance>(GameManager.Players.Values);
                players = players.OrderByDescending(x => x.Kills).ToList();

                for (int i = 0; i < players.Count; i++)
                {
                    PlayerInstance player = players[i];
                    if (i < _instantiatedPresenters.Count)
                    {
                        _instantiatedPresenters[i].Show(true);
                        _instantiatedPresenters[i].WriteData(player.PlayerInfo.Username, player.Kills, player.Deaths, player.Team, player.Latency);
                    }
                    else
                    {
                        var presenter = Instantiate(playerPresenter, content.position, content.rotation, content);
                        presenter.Show(true);
                        presenter.WriteData(player.PlayerInfo.Username, player.Kills, player.Deaths, player.Team, player.Latency);

                        _instantiatedPresenters.Add(presenter);
                    }
                }

                if (players.Count < _instantiatedPresenters.Count)
                {
                    for (int i = players.Count; i < _instantiatedPresenters.Count; i++)
                    {
                        _instantiatedPresenters[i].Show(false);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        void Update()
        {
            scoreBoard.SetActive(Input.GetKey(KeyCode.Tab) && ClientFrontend.GamePlayInput());

            if (scoreBoard.activeSelf && c_refresher == null)
            {
                c_refresher = StartCoroutine(RefreshScoreboard());
            }

            if (!scoreBoard.activeSelf && c_refresher != null)
            {
                StopCoroutine(c_refresher);
                c_refresher = null;
            }

        }
    }
}