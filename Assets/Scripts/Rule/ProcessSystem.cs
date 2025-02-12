using UnityEngine;

namespace Assets.Rule
{
    public class ProcessSystem : MonoBehaviour
    {
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

        }

        private void Ready()
        {
            gameRule = new GameRule(this);
            scoreSystem = new ScoreSystem();
            scoreSystem.ResetAll();
            turnCount = 1;
            state = ProcessState.Running;
            if (state == ProcessState.Running)
            {
                Debug.Log("ゲーム開始");
            }
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
    }
}