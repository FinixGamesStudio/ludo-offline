using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Ludo.SignUpAcknowledgementClass;
using lu;

namespace Ludo
{
    public class LudoMatchMakingController : MonoBehaviour
    {
        public LudoAiController aiController;

        public GenerateTheBots generateTheBots;

        public TextMeshProUGUI timerText;
        public int timerValue;

        public int noOfPlayer;

        private void Start()
        {
            StartMatchMaking();
        }

        private void StartMatchMaking()
        {
            timerValue = 30;
            InvokeRepeating(nameof(Timer), 1, 1f);
            StartCoroutine(StartAddingBots());
        }

        void Timer()
        {
            timerValue--;
            timerText.text = $"Searching for opponent... {timerValue}";
            if (timerValue < 0)
            {
                CancelInvoke(nameof(Timer));

            }

        }

        public void StartGamePlay()
        {
            Debug.Log("StartGamePlay");
        }

        IEnumerator StartAddingBots()
        {
            if (noOfPlayer == 2)
            {
                yield return new WaitForSeconds(Random.Range(1, 2f));
                aiController.playersInfoData.Add(GetOneBot());
                Debug.LogError("1");
                Debug.LogError("GOING TO BREAK");

                StartGamePlay();

                yield break;
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(1, 2f));
                Debug.LogError("1");
                aiController.playersInfoData.Add(GetOneBot());
                yield return new WaitForSeconds(Random.Range(2, 4f));
                Debug.LogError("2");
                aiController.playersInfoData.Add(GetOneBot());
                yield return new WaitForSeconds(Random.Range(2, 4f));
                Debug.LogError("3");
                aiController.playersInfoData.Add(GetOneBot());

                Debug.LogError("GOING TO BREAK");

                StartGamePlay();
                yield break;
            }
        }


        PlayerInfoData GetOneBot()
        {
            PlayerInfoData playerInfoData = generateTheBots.allBotDetails[Random.Range(0, generateTheBots.allBotDetails.Count)];
            return playerInfoData;
        }

    }

}
