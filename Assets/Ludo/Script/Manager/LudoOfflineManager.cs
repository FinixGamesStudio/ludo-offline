using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class LudoOfflineManager : MonoBehaviour
    {
        GameMode gameMode;
        int entryFees = 0;
        int winAmt = 0;
        int maxPlayerCount=2;

       // public lng ludoNumberGsNew;


        public void StartOfflineGame(string game)
        {
            switch (game)
            {
                case "CLASSIC":
                    gameMode = GameMode.classic;
                    break;
                case "NUMBER":
                    gameMode = GameMode.number;
                    break;
                case "DICE":
                    gameMode = GameMode.dice;
                    break;
            }

          
            if (maxPlayerCount == 2)
                winAmt = entryFees * 2;
            else
                winAmt = entryFees * 4;
          
         

          
            if (gameMode != GameMode.classic)
            {
              //  SetCookiePosition();
            }
          //  socketNumberEventReceiver.PlayerJoinData();
            //gamePlayed = PlayerPrefs.GetInt("gamePlayed");
            //gameWon = PlayerPrefs.GetInt("gameWon");
            //gameLoss = PlayerPrefs.GetInt("gameLoss");
            //UpdateGameStatistics(gamePlayed + 1, gameWon, gameLoss);






            //  MGPSDK.MGPGameManager.instance.sdkConfig.data.lobbyData.noOfPlayer = 4;
            //    ClickOnPlayerButton(4);
            //  dashBoardAPIRequestHandler.LobyyRequestData();
        }

      

        public enum GameMode
        {
            classic,
            dice,
            number
        }
    }
}