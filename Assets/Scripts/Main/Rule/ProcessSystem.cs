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
        [SerializeField] private AudioSource audioSource; // 追加
        [SerializeField] private AudioClip moveSE; // 追加
        [SerializeField] private AudioClip generateSE; // 追加
        [SerializeField] private AudioClip disappearSE; // 追加
        private GameRule gameRule = default;
        private ScoreSystem scoreSystem = default;
        private int turnCount = 0;
        private ProcessState currentProcessState;
        private ProcessState state;
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
        }

        public void Update()
        {
            if (state != ProcessState.Stop)
            {
                scoreSystem.ScoreUpdate();
                uiSystem.SetScoreText(scoreSystem.GetScore());
                uiSystem.SetTurnText(turnCount);
                uiSystem.SetHighScoreText(scoreSystem.GetHighScore());
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
        }

        private void GameOver()
        {
            SceneManager.sceneLoaded += GameSceneLoaded;
            SceneManager.LoadScene("Scenes/Develop/Result");

        }

        private void GameSceneLoaded(Scene next, LoadSceneMode mode)
        {
            var ChangeResultText = GameObject.Find("ChangeResultText").GetComponent<ChangeResultText>();
            ChangeResultText.score = scoreSystem.GetScore();
            ChangeResultText.turn = turnCount;
            SceneManager.sceneLoaded -= GameSceneLoaded;
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
                PlayMoveSE(); // 追加
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
                PlayMoveSE(); // 追加
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
                PlayMoveSE(); // 追加
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
                PlayMoveSE(); // 追加
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

        // アニメーション関連のメソッド
        public void GenerateWordBlockOnBoard(int x, int y, string word,int direction)
        {
            generateWordBlock.GenerateWordBlockOnBoard(x, y, word, direction);
            PlayGenerateSE(); // 追加
        }

        public void DisappearWordBlockOnBoard(List<WordBlockOnBoard> deleteList)
        {
            generateWordBlock.DisappearWordBlockOnBoard(deleteList);
            if(deleteList.Count > 0)
            {
                PlayDisappearSE(); // 追加
            }
        }

        public Tween MoveWordBlockOnBoard(int x, int y, int direction)
        {
            return generateWordBlock.MoveWordBlockOnBoard(x, y, direction);
        }

        private void PlayMoveSE() // 追加
        {
            if (audioSource != null && moveSE != null)
            {
                audioSource.PlayOneShot(moveSE);
            }
        }

        private void PlayGenerateSE() // 追加
        {
            if (audioSource != null && generateSE != null)
            {
                audioSource.PlayOneShot(generateSE);
            }
        }

        private void PlayDisappearSE() // 追加
        {
            if (audioSource != null && disappearSE != null)
            {
                audioSource.PlayOneShot(disappearSE);
            }
        }
    }
}