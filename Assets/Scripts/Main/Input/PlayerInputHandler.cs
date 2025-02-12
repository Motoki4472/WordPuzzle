using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Rule;

namespace Assets.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {

        private InputAction _moveAction;
        private bool _isMoving = false;
        [SerializeField] private ProcessSystem processSystem = default;
        public void Start()
        {
            var pInput = GetComponent<PlayerInput>();
            var actionMap = pInput.currentActionMap;

            _moveAction = actionMap["Move"];
        }

        public void Update()
        {
            //アクションからコントローラの入力値を取得
            Vector2 move = _moveAction.ReadValue<Vector2>();

            DetectDirection(move);
        }

        private void DetectDirection(Vector2 move)
        {
            if(move == Vector2.zero)
            {
                _isMoving = true;
            }
        

            if (Mathf.Abs(move.x) <= 0.8f && Mathf.Abs(move.y) <= 0.8f || !_isMoving)
                return;
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                if (move.x > 0)
                    Right();
                else
                    Left();
            }
            else
            {
                if (move.y > 0)
                    Up();
                else
                    Down();
            }
        }

        private void Right()
        {
            processSystem.Right();
            _isMoving = false;
        }

        private void Left()
        {
            processSystem.Left();
            _isMoving = false;
        }

        private void Up()
        {
            processSystem.Up();
            _isMoving = false;
        }

        private void Down()
        {
            processSystem.Down();
            _isMoving = false;
        }
    }
}
