using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class DashBoardManager : MonoBehaviour
    {
        public Sprite noButtonSprite;
        public Sprite yesButtonSprite;

        public Image classicButton;
        public Image numberButton;
        public Image diceButton;
        public Image onlineButton;
        public Image offlineButton;

        public LudoSocketConnection socketConnection;
        public GameObject dashBordPanal;


        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "CLASSIC":
                    ResetButton();
                    classicButton.sprite = yesButtonSprite;
                    break;
                case "NUMBER":
                    ResetButton();
                    numberButton.sprite = yesButtonSprite;
                    break;
                case "DICE":
                    ResetButton();
                    diceButton.sprite = yesButtonSprite;
                    break;
                default:
                    break;
            }
            LudoGameManager.instace.signUpRequestSDKData.gameModeName = buttonName;
            LudoGameManager.instace.signUpRequestSDKData.gameType = buttonName;
        }

        public void ClickOnGameTypeButton(string buttonName)
        {
            switch (buttonName)
            {
                case "ONLINE":
                    ResetGameTypeButton();
                    onlineButton.sprite = yesButtonSprite;
                    LudoGameManager.instace.gamePlayMode = GamePlayMode.Online;
                    break;
                case "OFFLINE":
                    ResetGameTypeButton();
                    offlineButton.sprite = yesButtonSprite;
                    LudoGameManager.instace.gamePlayMode = GamePlayMode.Offline;
                    break;
                default:
                    break;
            }
        }

        public void ResetButton()
        {
            classicButton.sprite = noButtonSprite;
            numberButton.sprite = noButtonSprite;
            diceButton.sprite = noButtonSprite;
          
        }
        public void ResetGameTypeButton()
        {
            onlineButton.sprite = noButtonSprite;
            offlineButton.sprite = noButtonSprite;
        }

        public void ClickOnPLayButton()
        {
            dashBordPanal.SetActive(false);
            if (LudoGameManager.instace.gamePlayMode == GamePlayMode.Online)
            {
                socketConnection.LudoSocketConnectionStart(socketConnection.socketUrl);// Call socket 
            }
            else
            {
               LudoGameManager.instace.aiController.StartGamePlayWithAI();
              //  LudoGameManager.instace.offlineManager.StartOfflineGame(LudoGameManager.instace.signUpRequestSDKData.gameModeName);
            }
        }
    }
}