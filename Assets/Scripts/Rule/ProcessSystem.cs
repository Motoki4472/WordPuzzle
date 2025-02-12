using UnityEngine;

namespace Assets.Rule
{
    public class ProcessSystem : MonoBehaviour
    {
        private GameRule gameRule = default;
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

        private void Ready()
        {
            //初期配置
            gameRule = new GameRule(this);
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
                state = ProcessState.Processing;
                gameRule.RightBoard();
            }
        }

        public void Left()
        {
            if (state == ProcessState.Running)
            {
                state = ProcessState.Processing;
                gameRule.LeftBoard();
            }
        }

        public void Up()
        {
            if (state == ProcessState.Running)
            {
                state = ProcessState.Processing;
                gameRule.UpBoard();
            }
        }

        public void Down()
        {
            if (state == ProcessState.Running)
            {
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
        }
    }
}