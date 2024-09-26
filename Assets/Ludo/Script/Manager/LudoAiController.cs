using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using static Ludo.TurnMissedResponseClass;
using static Ludo.MoveTokenResponseClass;
using static Ludo.SignUpAcknowledgementClass;
using static Ludo.JoinTableResponseClass;
using static Ludo.UserTurnStartResponseClass;
using static Ludo.GameTimerStartResponseClass;
using static Ludo.UserExtraTimeStartResponseClass;
using static Ludo.BattleFinishClass;
using static Ludo.DiceAnimationResponseClass;
using static Ludo.GameMainTimerResponseClass;
using static Ludo.TieBreakerResponseClass;

namespace Ludo
{
    public class LudoAiController : MonoBehaviour
    {
        public LudoGameManager gameManager;

        public List<PlayerInfoData> playersInfoData;

        public List<PlayerInfoData> activePlayerInfoData;

        public PlayerInfoData selfPlayer;

        [Header("=================================================")]

        public List<string> allResponse;

        //public SignUpAcknowledgement signUpAcknowledgement;

        public JoinTableResponse joinTableResponse;

        public GameTimerStartResponse gameTimerStartResponse;

        public UserTurnStartResponse userTurnStartResponse;

        public UserExtraTimeStartResponse userExtraTimeStartResponse;

        public TurnMissedResponse turnMissedResponseData;

        public MoveTokenResponse moveTokenResponse;

        public DiceAnimationResponse diceAnimationResponse;

        public GameMainTimerResponse gameMainTimer;


        public void StartGamePlayWithAI() => StartCoroutine(AI());

        public int currentTurn;

        public static readonly List<float> ProbabilityConfigHalf = new List<float> { 0.15f, 0.15f, 0.15f, 0.19f, 0.18f, 0.18f };

        public List<int> moves;

        public TieBreakerResponse tieBreakerResponse;

        void Awake()
        {
            //  BattleFinish();
        }


        IEnumerator AI()
        {
            Debug.Log(JsonConvert.SerializeObject(tieBreakerResponse));

            string mainTimer = JsonConvert.SerializeObject(gameMainTimer);

            Debug.Log(mainTimer);

            moves = ProbabilityGenerator.GenerateMovesBasedOnProbability(24, ProbabilityConfigHalf);

            gameManager.OnSignUpAcknowledgement(allResponse[0]);// SELF USER

            playersInfoData = playersInfoData.OrderBy(a => Guid.NewGuid()).ToList();

            activePlayerInfoData.Add(selfPlayer);

            joinTableResponse = JsonConvert.DeserializeObject<JoinTableResponse>(allResponse[1]);
            joinTableResponse.data.playerInfo.Clear();
            joinTableResponse.data.playerInfo.Add(selfPlayer);

            yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(joinTableResponse));

            for (int i = 0; i < gameManager.signUpRequestSDKData.minPlayer - 1; i++)
                activePlayerInfoData.Add(playersInfoData[i]);

            joinTableResponse.data.playerInfo.Clear();
            joinTableResponse.data.playerInfo = activePlayerInfoData;
            joinTableResponse.data.playerMoves = moves.OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < joinTableResponse.data.playerInfo.Count; i++)
                if (joinTableResponse.data.playerInfo[i] != selfPlayer)
                    joinTableResponse.data.playerInfo[i].seatIndex = i * gameManager.signUpRequestSDKData.minPlayer == 2 ? 2 : 1;

            yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(joinTableResponse));

            yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(allResponse[3]);//GAME_TIMER_START
        }


        public void MainTimerAi() => LudoGameManager.instace.socketEvnetReceiver.ReciveData(allResponse[14]);

        public void AiUserTurn()
        {
            gameManager.tokenController.CoockieManage();
            userTurnStartResponse = JsonConvert.DeserializeObject<UserTurnStartResponse>(allResponse[4]);

            for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
            {
                gameManager.allPlayerHomeController[i].isUserExtraTurnFinish = false;
                gameManager.allPlayerHomeController[i].isUserTurnFinish = false;

                List<int> tokenDetails = new List<int> { 0, 0, 0, 0 };
                gameManager.allPlayerHomeController[i].playerInfoData.tokenDetails = new List<int>(tokenDetails);
                Debug.Log(gameManager.allPlayerHomeController[i].playerInfoData.tokenDetails.Count);

                for (int j = 0; j < gameManager.allPlayerHomeController[i].allPlayerToken.Count; j++)
                {
                  //  Debug.Log(j);
                    tokenDetails[j] = gameManager.allPlayerHomeController[i].allPlayerToken[j].myLastBoxIndex;
                }

                gameManager.allPlayerHomeController[i].playerInfoData.tokenDetails = new List<int>(tokenDetails);
            }

            currentTurn++;
            if (currentTurn >= activePlayerInfoData.Count)
                currentTurn = 0;

            if (activePlayerInfoData[currentTurn].seatIndex == selfPlayer.seatIndex)
                movesLeft--;

            if (movesLeft <= 0)
            {
                Debug.LogError("------------");
                //Battle Win
                if (!gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                {
                    moves = moves.OrderBy(a => Guid.NewGuid()).ToList();
                    movesLeft = moves.Count;
                }
                else
                {
                    BattleFinish();
                    return;
                }
            }

            userTurnStartResponse.data.startTurnSeatIndex = activePlayerInfoData[currentTurn].seatIndex;
            userTurnStartResponse.data.movesLeft = movesLeft;
            if (activePlayerInfoData[currentTurn].seatIndex == selfPlayer.seatIndex)
                userTurnStartResponse.data.diceValue = gameManager.signUpAcknowledgement.data.playerMoves[24 - movesLeft];
            else
            {
                LudoHomeController home = gameManager.allPlayerHomeController.Find(player => player.staticSeatIndex == activePlayerInfoData[currentTurn].seatIndex);
                int diceValue = home.playersMoves[24 - movesLeft];
                userTurnStartResponse.data.diceValue = diceValue;
            }
            //yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(userTurnStartResponse));

            //BoatMove();
        }


        public void DiceAnimation() => StartCoroutine(DiceAnimationAI());

        IEnumerator DiceAnimationAI()
        {
            diceAnimationResponse = (JsonConvert.DeserializeObject<DiceAnimationResponse>(allResponse[13]));
            diceAnimationResponse.data.startTurnSeatIndex = activePlayerInfoData[currentTurn].seatIndex;
            diceAnimationResponse.data.diceValue = userTurnStartResponse.data.diceValue;
            yield return new WaitForSeconds(0f);

            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(diceAnimationResponse));
        }

        public void BoatMove() => StartCoroutine(AiMakeAMove());

        IEnumerator AiMakeAMove()
        {
            Debug.Log("============");
            yield return new WaitForSeconds(1f);
            if (gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                AiMoveToken(UnityEngine.Random.Range(0, 4));
            else
            {
                LudoHomeController home = gameManager.allPlayerHomeController.Find(player => player.staticSeatIndex == activePlayerInfoData[currentTurn].seatIndex);
                home.DiceAnimation();
            }
        }

        public void DiceModeAIMoveToken() => StartCoroutine(DiceModeAIMove());

        IEnumerator DiceModeAIMove()
        {
            yield return new WaitForSeconds(1f);

            LudoHomeController home = gameManager.allPlayerHomeController.Find(player => player.staticSeatIndex == activePlayerInfoData[currentTurn].seatIndex);

            List<LudoTokenController> allToken = home.allPlayerToken.Where(player => player.myLastBoxIndex == -1).ToList();
            List<LudoTokenController> allTokenThatCanMove = home.allPlayerToken.Where(player => player.myLastBoxIndex >= 0 && player.myLastBoxIndex + userTurnStartResponse.data.diceValue <= 56).ToList();

            //Debug.LogError("======> allTokenThatCanMove " + allTokenThatCanMove.Count);
            //Debug.LogError("======> allToken.Count ==  " + allToken.Count);
            //Debug.LogError("======> diceValue " + userTurnStartResponse.data.diceValue);
            //Debug.LogError("======> 1 " + (allToken.Count == 4 && userTurnStartResponse.data.diceValue != 6));
            //Debug.LogError("======> 2 " + (allTokenThatCanMove.Count > 0 && allToken.Count < 4));

            if (allTokenThatCanMove.Count > 0)
                Debug.LogError("======> allTokenThatCanMove " + allTokenThatCanMove[UnityEngine.Random.Range(0, allTokenThatCanMove.Count)].tokenIndex);

            if (allToken.Count == 4 && userTurnStartResponse.data.diceValue != 6)
                AiUserTurn();
            else if (allToken.Count == 4 && userTurnStartResponse.data.diceValue == 6)
                AiMoveToken(0);
            else if (allTokenThatCanMove.Count > 0 && allToken.Count < 4)
                AiMoveToken(allTokenThatCanMove[UnityEngine.Random.Range(0, allTokenThatCanMove.Count)].tokenIndex);
            else if (allTokenThatCanMove.Count == 0)
                if (allToken.Count > 0 && userTurnStartResponse.data.diceValue == 6)
                    AiMoveToken(allToken[UnityEngine.Random.Range(0, allToken.Count)].tokenIndex);
                else
                    AiUserTurn();
            else
            {
                Debug.LogError("========== HERE ===>>>>>>>>>>");
            }

        }

        public void AiMoveToken(int tokenIndex) => StartCoroutine(AiUserMoveToken(tokenIndex));
        IEnumerator AiUserMoveToken(int tokenIndex)
        {
            moveTokenResponse = JsonConvert.DeserializeObject<MoveTokenResponse>(allResponse[5]);
            moveTokenResponse.data.tokenMove = tokenIndex;
            moveTokenResponse.data.movementValue = userTurnStartResponse.data.diceValue;
            Debug.LogError("  diceValue =====>  " + userTurnStartResponse.data.diceValue);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(moveTokenResponse));
            yield return new WaitForSeconds(1f);
        }
        public void AiUserExtraTurn() => StartCoroutine(AiExtraTurn());
        public int movesLeft;
        IEnumerator AiExtraTurn()
        {
            LudoHomeController home = gameManager.allPlayerHomeController.Find(x => x.playerInfoData.seatIndex == activePlayerInfoData[currentTurn].seatIndex);
            home.isUserTurnFinish = true;

            yield return new WaitForSeconds(1f);

            userExtraTimeStartResponse = JsonConvert.DeserializeObject<UserExtraTimeStartResponse>(allResponse[9]);
            userExtraTimeStartResponse.data.startTurnSeatIndex = activePlayerInfoData[currentTurn].seatIndex;

            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(userExtraTimeStartResponse));
        }

        public void AiUserTurnMissed() => StartCoroutine(UserTurnMissed());

        IEnumerator UserTurnMissed()
        {
            LudoHomeController home = gameManager.allPlayerHomeController.Find(x => x.playerInfoData.seatIndex == activePlayerInfoData[currentTurn].seatIndex);
            home.isUserTurnFinish = true;
            home.isUserExtraTurnFinish = true;
            home.playerInfoData.missedTurnCount++;
            home.TurnMissedCouter(home.playerInfoData.missedTurnCount);

            if (home.playerInfoData.missedTurnCount >= 3)
            {
                if (activePlayerInfoData[currentTurn].seatIndex == selfPlayer.seatIndex)
                    LudoGameManager.instace.socketEvnetReceiver.ReciveData(allResponse[10]);
                else
                {
                    yield return null;
                    LudoGameManager.instace.socketEvnetReceiver.ReciveData(allResponse[11]);
                }
            }

            yield return new WaitForSeconds(1f);

            AiUserTurn();
        }

        public void CheckForTokenkill(LudoTokenController tokenController, LudoBoxProperties boxProperties)
        {
            if (gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER"))
                if (gameManager.signUpAcknowledgement.data.thisseatIndex == gameManager.userTurnStartResponseData.data.startTurnSeatIndex)
                    gameManager.moveText.text = (gameManager.userTurnStartResponseData.data.movesLeft - 1).ToString();

            for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
                gameManager.allPlayerHomeController[i].UpdateUserScore(0);

            Debug.LogError("1 ==========> " + boxProperties.name);
            if (boxProperties.allTokenInSameBox.Count == 2 && boxProperties.boxType != BoxType.Star)
            {

                Debug.LogError("2 ==========>");
                if (boxProperties.allTokenInSameBox[0].homeController.staticSeatIndex != boxProperties.allTokenInSameBox[1].homeController.staticSeatIndex)
                {
                    boxProperties.allTokenInSameBox[0].KillMove();
                    Vector3 targetPosition = boxProperties.allTokenInSameBox[0].playersWayPoints.wayPointsForTokenMove[boxProperties.allTokenInSameBox[0].myLastBoxIndex + 1].transform.GetChild(0).position;
                    gameManager.killPratical.transform.position = targetPosition;
                    gameManager.killPratical.Play();
                    SoundManager.instance.soundAudioSource.Stop();
                    SoundManager.instance.TokenKill(SoundManager.instance.killAudio);
                }
                else
                {
                    if (!gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER") && gameManager.uiManager.diceAnimationController.diceAnimationResponse.data.diceValue == 6)
                        OnKillOffline();
                    else
                        AiUserTurn();
                }
            }
            else if (boxProperties.allTokenInSameBox.Count < 2 || boxProperties.allTokenInSameBox.Count > 2)
                if (!gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER") && gameManager.uiManager.diceAnimationController.diceAnimationResponse.data.diceValue == 6)
                    OnKillOffline();
                else
                    AiUserTurn();
            else if (boxProperties.allTokenInSameBox.Count == 2 && boxProperties.boxType == BoxType.Star)
                if (!gameManager.signUpRequestSDKData.gameModeName.Equals("NUMBER") && gameManager.uiManager.diceAnimationController.diceAnimationResponse.data.diceValue == 6)
                    OnKillOffline();
                else
                    AiUserTurn();
        }

        public void OnKillOffline() => StartCoroutine(OnKillGiveTurnToSameUser());

        IEnumerator OnKillGiveTurnToSameUser()
        {
            userTurnStartResponse.data.startTurnSeatIndex = activePlayerInfoData[currentTurn].seatIndex;
            userTurnStartResponse.data.isExtraTurn = true;
            userTurnStartResponse.data.movesLeft = movesLeft;
            userTurnStartResponse.data.diceValue = UnityEngine.Random.Range(1, 6);

            yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(userTurnStartResponse));
        }

        public void SendEmoji(string response) => LudoGameManager.instace.socketEvnetReceiver.ReciveData(response);

        public void BattleFinish()
        {
            Debug.LogError("Duplicate BattleFinish in the list:");
            var duplicates = gameManager.allPlayerHomeController.GroupBy(x => x.OfflineScore()).Where(g => g.Count() > 1).SelectMany(g => g).ToList();


          /*  for (int i = 0; i < duplicates.Count; i++)
            {
                Debug.LogError("================== " + i + " || " + duplicates[i].playerInfoData.seatIndex + " || " + duplicates[i].playerInfoData.username);
            }*/
            Debug.Log("Duplicate elements in the list:");
            foreach (var item in duplicates)
            {
                Debug.Log(item.playerInfoData.seatIndex);
                Debug.Log("Offline Score ====>" + item.OfflineScore());
            }

            if (duplicates.Count == 2)
            {
                Debug.LogError("IF");
                tieBreakerResponse = JsonConvert.DeserializeObject<TieBreakerResponse>(allResponse[15]);
                tieBreakerResponse.data.userData = new List<UserData>();

                for (int i = 0; i < ludoHomeControllers.Count; i++)
                {
                    if (ludoHomeControllers[i].playerInfoData.seatIndex != -1)
                    {
                        UserData userData = new UserData();
                        userData.seatIndex = ludoHomeControllers[i].playerInfoData.seatIndex;

                        ludoHomeControllers[i].allPlayerToken = ludoHomeControllers[i].allPlayerToken.OrderByDescending(controller => controller.myLastBoxIndex).ToList();

                        userData.tokenIndex = ludoHomeControllers[i].allPlayerToken[0].tokenIndex;
                        userData.furthestToken = ludoHomeControllers[i].allPlayerToken[0].myLastBoxIndex;
                        tieBreakerResponse.data.userData.Add(userData);
                    }
                }

                List<UserData> allUserData = tieBreakerResponse.data.userData.OrderByDescending(controller => controller.furthestToken).ToList();
                tieBreakerResponse.data.winnerIndex = allUserData[0].seatIndex;

                LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(tieBreakerResponse));

            }
            else
            {
                Debug.LogError("ELSE");

                StartCoroutine(BattleFinishAI());
            }
        }

        public List<LudoHomeController> ludoHomeControllers;

        public BattleFinishResponse battleFinishResponse;

        IEnumerator BattleFinishAI()
        {
            battleFinishResponse = JsonConvert.DeserializeObject<BattleFinishResponse>(allResponse[11]);
            battleFinishResponse.data.payload.players = new List<BattleFinishResponsePlayer>();

            ludoHomeControllers = new List<LudoHomeController>();

            for (int i = 0; i < gameManager.allPlayerHomeController.Count; i++)
                if (gameManager.allPlayerHomeController[i].playerInfoData.seatIndex != -1)
                    ludoHomeControllers.Add(gameManager.allPlayerHomeController[i]);

            ludoHomeControllers = ludoHomeControllers.OrderBy(controller => controller.OfflineScore()).ToList();

            for (int i = 0; i < ludoHomeControllers.Count; i++)
            {
                if (ludoHomeControllers[i].playerInfoData.seatIndex != -1)
                {
                    BattleFinishResponsePlayer responsePlayer = new BattleFinishResponsePlayer();
                    responsePlayer.username = ludoHomeControllers[i].playerInfoData.username;
                    responsePlayer.userId = ludoHomeControllers[i].playerInfoData.userId;
                    responsePlayer.avatar = ludoHomeControllers[i].playerInfoData.userProfile;
                    responsePlayer.score = ludoHomeControllers[i].OfflineScore();

                    if (i == 0)
                        responsePlayer.winType = "win";
                    else
                        responsePlayer.winType = "lost";

                    battleFinishResponse.data.payload.players.Add(responsePlayer);
                }
            }

            yield return new WaitForSeconds(1f);
            LudoGameManager.instace.socketEvnetReceiver.ReciveData(JsonConvert.SerializeObject(battleFinishResponse));
        }

        void IsBattleFinishOrTie()
        {

        }
    }
}
