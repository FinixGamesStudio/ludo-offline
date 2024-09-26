using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Ludo
{
    public class LudoSocketEvnetReceiver : MonoBehaviour
    {
        public int userStartIndex;

        public LudoGameManager gameManager;

        public LudoUiManager uiManager;

        public void ReciveData(string responseJsonString)
        {
            JObject responseJsonData = JObject.Parse(responseJsonString);
            string eventName = responseJsonData.GetValue("en").ToString();

            Debug.Log($"<color><b> TIME ||  {System.DateTime.Now.ToString("hh:mm:ss fff")} </b></color>||<color=blue><b> SEND EVENT </b></color><color><b>{eventName}</b></color>");
            Debug.Log("<color><b> TIME || " + System.DateTime.Now.ToString("hh:mm:ss fff") + " || </b></color><color=blue><b> SendDataWithAcknowledgement :- </b></color>" + responseJsonString);

            switch (eventName)
            {
                case "CONNECTION_SUCCESS":
                    gameManager.SendSingUpRequest();
                    break;
                case "JOIN_TABLE":
                    gameManager.SetUserDataOnBoard(responseJsonString);
                    break;
                case "GAME_TIMER_START":
                    uiManager.timerController.GameTimerStart(responseJsonString);
                    break;
                case "MAIN_GAME_TIMER_START":
                    uiManager.timerController.MainTimer(responseJsonString);
                    break;
                case "USER_TURN_START":
                    gameManager.SetUserTurnStartData(responseJsonString);
                    break;
                case "USER_EXTRA_TIME_START":
                    gameManager.OnResponseExtraTimer(responseJsonString);
                    break;
                case "TURN_MISSED":
                    gameManager.OnTurnMissed(responseJsonString);
                    break;
                case "BATTLE_FINISH":
                    uiManager.battelFinishCotroller.Battle(responseJsonString);
                    break;
                case "ALERT_POPUP":
                    uiManager.alertController.OnResponseAlert(responseJsonString);
                    break;
                case "LEAVE_TABLE":
                    gameManager.LeaveTable(responseJsonString);
                    break;
                case "MOVE_TOKEN":
                    gameManager.TokenMove(responseJsonString);
                    break;
                case "TIE_BREAKER":
                    gameManager.OnTieBreaker(responseJsonString);
                    break;
                case "HEART_BEAT":

                    break;
                case "HEART_BEAT_CLIENT":

                    break;
                case "SCORE_CHECK":
                    gameManager.ScoreCheck(responseJsonString);
                    break;
                case "SHOW_POPUP":
                    uiManager.commonPopupController.ShowPopUp(responseJsonString);
                    break;
                case "EMOJI":
                    uiManager.emojiController.OnEmojiResponse(responseJsonString);
                    break;
                case "DICE_ANIMATION_STARTED":
                    uiManager.diceAnimationController.OnDiceAnimationResponse(responseJsonString);
                    break;
                default:
                    break;
            }
        }
        public GameObject tieBreakerBg;
    }
}