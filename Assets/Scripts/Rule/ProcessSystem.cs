using UnityEngine;

namespace Rule
{
    public class ProcessSystem : MonoBehaviour
    {
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
            ProcessState state = ProcessState.Ready;
        }
    }
}