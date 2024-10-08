using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using static Ludo.BattleFinishClass;

namespace Ludo
{

    public class LudoBattelFinishController : MonoBehaviour
    {
        public LudoGameManager gameManager;

        public GameObject waitPanel, youWin, youLose, gameTie;

        public ParticleSystem winningPartical;

        public void CloseBattle() => gameObject.SetActive(false);

        public void Battle(string response)
        {
            gameObject.SetActive(true);
            waitPanel.SetActive(false);
            StartCoroutine(BattleFinish(response));
        }

        IEnumerator BattleFinish(string response)
        {
            //extraMove.SetActive(false);
            for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
            {
                gameManager.allPlayerHomeController[i].allPlayerToken.ForEach((coockie) => coockie.gameObject.SetActive(false));
                gameManager.allPlayerHomeController[i].TurnDataReset();
                gameManager.allPlayerHomeController[i].diceNumberText.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0f);
            BattleFinishBorad(response);
        }

        public BattleFinishResponse battleFinish;

        void BattleFinishBorad(string response)
        {
            SoundManager.instance.musicAudioSource.Stop();
            SoundManager.instance.soundAudioSource.Stop();
            SoundManager.instance.timeAudioSource.Stop();

            winningPartical.gameObject.SetActive(false);

            battleFinish = JsonConvert.DeserializeObject<BattleFinishResponse>(response);

            SetWinnerData(battleFinish.data);

            for (int i = 0; i < battleFinish.data.payload.players.Count; i++)
            {
                if (gameManager.gamePlayMode == GamePlayMode.Offline && battleFinish.data.payload.players[i].seatIndex == 0)
                {
                    if (battleFinish.data.payload.players[i].winType == "win")
                    {
                        winningPartical.gameObject.SetActive(true);
                        winningPartical.Play();
                        youWin.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.winAudio);
                    }
                    else if (battleFinish.data.payload.players[i].winType == "lost")
                    {
                        youLose.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.loseAudio);
                    }
                    else if (battleFinish.data.payload.players[i].winType == "tie")
                    {
                        gameTie.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.loseAudio);
                    }
                }
                else
                {
                    if (battleFinish.data.payload.players[i].winType == "win" && battleFinish.data.payload.players[i].userId == gameManager.signUpAcknowledgement.userId)
                    {
                        winningPartical.gameObject.SetActive(true);
                        winningPartical.Play();
                        youWin.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.winAudio);
                    }
                    else if (battleFinish.data.payload.players[i].winType == "lost" && battleFinish.data.payload.players[i].userId == gameManager.signUpAcknowledgement.userId)
                    {
                        youLose.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.loseAudio);
                    }
                    else if (battleFinish.data.payload.players[i].winType == "tie" && battleFinish.data.payload.players[i].userId == gameManager.signUpAcknowledgement.userId)
                    {
                        gameTie.SetActive(true);
                        SoundManager.instance.SoundPlay(SoundManager.instance.loseAudio);
                    }
                }


            }
        }


        public Sprite greenSprite;
        public Sprite blueSprite;
        public List<LudoBattelFinishRowUIController> fourPlayerDataList = new List<LudoBattelFinishRowUIController>();

        public void SetWinnerData(BattleFinishResponseData data)
        {
            for (int i = 0; i < fourPlayerDataList.Count; i++)
                fourPlayerDataList[i].gameObject.SetActive(false);

            for (int i = 0; i < data.payload.players.Count; i++)
                fourPlayerDataList[i].gameObject.SetActive(true);

            for (int i = 0; i < data.payload.players.Count; i++)
            {

                fourPlayerDataList[i].userName.text = data.payload.players[i].username;
                fourPlayerDataList[i].score.text = data.payload.players[i].score.ToString();
                fourPlayerDataList[i].winAmount.text = "₹" + data.payload.players[i].winAmount.ToString();

                //fourPlayerDataList[i].SetUserProfile(data.payload.players[i].avatar);

                if (data.payload.players[i].winType == "win")
                {
                    fourPlayerDataList[i].boxImage.GetComponent<Image>().sprite = greenSprite;
                    fourPlayerDataList[i].crown.SetActive(true);
                }
                else if (data.payload.players[i].winType == "lost")
                {
                    fourPlayerDataList[i].boxImage.GetComponent<Image>().sprite = blueSprite;
                    fourPlayerDataList[i].crown.SetActive(false);
                }
                else if (data.payload.players[i].winType == "tie")
                {
                    fourPlayerDataList[i].boxImage.GetComponent<Image>().sprite = greenSprite;
                    fourPlayerDataList[i].crown.SetActive(true);
                }
            }
        }

        public void ExitButton()
        {
            gameManager.OnClickExit();
        }
    }
}
