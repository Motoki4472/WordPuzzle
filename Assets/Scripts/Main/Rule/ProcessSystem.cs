using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.UISystem;
using Assets.WordBlock;
using DG.Tweening;
using Assets.Result;

namespace Assets.Rule
{
    public class ProcessSystem : MonoBehaviour
    {
        [SerializeField] private Assets.UISystem.UISystem uiSystem = default;
        [SerializeField] private GenerateWordBlock generateWordBlock = default;
        [SerializeField] private GameObject EndGameEffect = default;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip moveSE;
        [SerializeField] private AudioClip generateSE;
        [SerializeField] private AudioClip disappearSE;
        private GameRule gameRule = default;
        private ScoreSystem scoreSystem = default;
        private int turnCount = 0;
        public int gameMode = 2;
        private ProcessState currentProcessState;
        private ProcessState state;
        private int classicHighScore = 0;
        private int endlessHighScore = 0;
        private enum ProcessState
        {
            Ready,
            Running,
            Processing,
            Stop,
            End
        }

        public void Start()
        {
            state = ProcessState.Ready;
            Ready();
            if (gameMode == 2)
            {
                classicHighScore = PlayerPrefs.GetInt("ClassicHighScore", 0);
            }
            if (gameMode == 0)
            {
                endlessHighScore = PlayerPrefs.GetInt("EndlessHighScore", 0);
            }
        }

        public void Update()
        {
            if (state != ProcessState.Stop)
            {
                if (gameRule.GetGameMode() != 1) // BountyHuntModeではない場合のみスコアを更新
                {
                    scoreSystem.ScoreUpdate();
                }
                uiSystem.SetScoreText(scoreSystem.GetScore());
                uiSystem.SetTurnText(turnCount);
                // BountyHuntModeのためにRemainingTurnを表示
                if (gameRule.GetGameMode() == 1)
                {
                    uiSystem.SetRemainingTurnText(gameRule.GetRemainingTurn());
                }
                if (gameRule.GetGameMode() == 2)
                {
                    if (scoreSystem.GetScore() > classicHighScore)
                    {
                        classicHighScore = scoreSystem.GetScore();
                        PlayerPrefs.SetInt("ClassicHighScore", classicHighScore);
                    }
                    uiSystem.SetHighScoreText(classicHighScore);
                    uiSystem.SetRemainingTurnText(gameRule.GetRemainingTurn());
                }
                if (gameRule.GetGameMode() == 0)
                {
                    if (scoreSystem.GetScore() > endlessHighScore)
                    {
                        endlessHighScore = scoreSystem.GetScore();
                        PlayerPrefs.SetInt("EndlessHighScore", endlessHighScore);
                    }
                    uiSystem.SetHighScoreText(endlessHighScore);
                }
            }
        }

        private void Ready()
        {
            gameRule = new GameRule(this);
            scoreSystem = new ScoreSystem();
            scoreSystem.ResetAll();
            turnCount = 0;
            state = ProcessState.Running;
            if (state == ProcessState.Running)
            {
                Debug.Log("ゲーム開始");
            }
            if (gameMode == 2)
            {
                classicHighScore = PlayerPrefs.GetInt("ClassicHighScore", 0);
            }
            if (gameMode == 0)
            {
                endlessHighScore = PlayerPrefs.GetInt("EndlessHighScore", 0);
            }
        }

        private void GameOver()
        {
            if (gameMode == 0)
            {
                if (scoreSystem.GetScore() > endlessHighScore)
                {
                    endlessHighScore = scoreSystem.GetScore();
                    PlayerPrefs.SetInt("EndlessHighScore", endlessHighScore);
                }
                SceneManager.sceneLoaded += EndlessSceneLoaded;
                SceneManager.LoadScene("Scenes/Develop/EndlessResult");
            }
            else if (gameMode == 1)
            {
                SceneManager.sceneLoaded += BountyHuntSceneLoaded;
                SceneManager.LoadScene("Scenes/Develop/BountyHuntResult");
            }
            else if (gameMode == 2)
            {
                if (scoreSystem.GetScore() > classicHighScore)
                {
                    classicHighScore = scoreSystem.GetScore();
                    PlayerPrefs.SetInt("ClassicHighScore", classicHighScore);
                }
                SceneManager.sceneLoaded += ClassicSceneLoaded;
                SceneManager.LoadScene("Scenes/Develop/ClassicResult");
            }


        }

        private void EndlessSceneLoaded(Scene next, LoadSceneMode mode)
        {
            var ChangeResultText = GameObject.Find("ChangeResultText").GetComponent<ChangeResultText>();
            ChangeResultText.score = scoreSystem.GetScore();
            ChangeResultText.turn = turnCount;
            SceneManager.sceneLoaded -= EndlessSceneLoaded;
        }

        private void BountyHuntSceneLoaded(Scene next, LoadSceneMode mode)
        {
            var ChangeBountyHuntResult = GameObject.Find("ChangeResultText").GetComponent<ChangeBountyHuntResultText>();
            ChangeBountyHuntResult.RemainingTurn = gameRule.GetRemainingTurn();
            ChangeBountyHuntResult.PassedTurn = turnCount;
            ChangeBountyHuntResult.BountyList = gameRule.GetBountyList();
            SceneManager.sceneLoaded -= BountyHuntSceneLoaded;
        }

        private void ClassicSceneLoaded(Scene next, LoadSceneMode mode)
        {
            var ChangeResultText = GameObject.Find("ChangeResultText").GetComponent<ChangeResultText>();
            ChangeResultText.score = scoreSystem.GetScore();
            ChangeResultText.turn = turnCount;
            SceneManager.sceneLoaded -= ClassicSceneLoaded;
        }

        // 以下は入力用にカプセル化されたメソッド

        public void Right()
        {
            if (state == ProcessState.Running)
            {
                turnCount++;
                scoreSystem.AddTurnCountFactor();
                state = ProcessState.Processing;
                gameRule.RightBoard();
                PlayMoveSE();
            }
        }

        public void Left()
        {
            if (state == ProcessState.Running)
            {
                turnCount++;
                scoreSystem.AddTurnCountFactor();
                state = ProcessState.Processing;
                gameRule.LeftBoard();
                PlayMoveSE();
            }
        }

        public void Up()
        {
            if (state == ProcessState.Running)
            {
                turnCount++;
                scoreSystem.AddTurnCountFactor();
                state = ProcessState.Processing;
                gameRule.UpBoard();
                PlayMoveSE();
            }
        }

        public void Down()
        {
            if (state == ProcessState.Running)
            {
                turnCount++;
                scoreSystem.AddTurnCountFactor();
                state = ProcessState.Processing;
                gameRule.DownBoard();
                PlayMoveSE();
            }
        }

        public void SetProcessStateToRunning()
        {
            state = ProcessState.Running;
        }

        public void SetProcessStateToStop()
        {
            currentProcessState = state;
            state = ProcessState.Stop;
        }

        public void UnsetProcessStateToStop()
        {
            state = currentProcessState;
        }

        public void SetProcessStateToEnd()
        {
            state = ProcessState.End;
            Debug.Log("ゲーム終了");
            EndGameEffect.transform.localPosition = new Vector3(0, 0, 0);
            EndGameEffect.GetComponent<CanvasGroup>().DOFade(1, 0.3f).OnComplete(() => GameOver());
        }

        // Score関係のメソッド

        public int GetScore()
        {
            return scoreSystem.GetScore();
        }

        public int GetComboCount()
        {
            return scoreSystem.GetComboCount();
        }

        public void AddScore()
        {
            scoreSystem.AddScore();
        }

        public void AddComboCount()
        {
            scoreSystem.AddComboCount();
        }

        public void ResetComboCount()
        {
            scoreSystem.ResetComboCount();
        }

        public void AddWordCountFactor()
        {
            scoreSystem.AddWordCountFactor();
        }

        public void ResetWordCountFactor()
        {
            scoreSystem.ResetWordCountFactor();
        }

        public int GetTurnCount()
        {
            return turnCount;
        }

        public void AddDeletedWord(string word)
        {
            uiSystem.SetWord(word);
        }

        public void StrikeThroughBountyHuntWord(string word)
        {
            uiSystem.StrikeThroughWord(word);
        }

        public void SetBountyHuntList(List<string> BountyHuntList)
        {
            uiSystem.SetBountyHuntList(BountyHuntList);
        }

        // アニメーション関連のメソッド
        public void GenerateWordBlockOnBoard(int x, int y, string word, int direction)
        {
            generateWordBlock.GenerateWordBlockOnBoard(x, y, word, direction);
            PlayGenerateSE();
        }

        public void DisappearWordBlockOnBoard(List<WordBlockOnBoard> deleteList)
        {
            generateWordBlock.DisappearWordBlockOnBoard(deleteList);
            if (deleteList.Count > 0)
            {
                PlayDisappearSE();
            }
        }

        public Tween MoveWordBlockOnBoard(int x, int y, int direction)
        {
            return generateWordBlock.MoveWordBlockOnBoard(x, y, direction);
        }

        private void PlayMoveSE()
        {
            if (audioSource != null && moveSE != null)
            {
                audioSource.PlayOneShot(moveSE);
            }
        }

        private void PlayGenerateSE()
        {
            if (audioSource != null && generateSE != null)
            {
                audioSource.PlayOneShot(generateSE);
            }
        }

        private void PlayDisappearSE()
        {
            if (audioSource != null && disappearSE != null)
            {
                audioSource.PlayOneShot(disappearSE);
            }
        }

        public int GetGameMode()
        {
            return gameMode;
        }
    }
}