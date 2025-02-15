using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Rule;
using System.Collections;

namespace Assets.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction _moveAction;
        private bool _isMoving = false;
        private bool _canMove = true; // 追加: 入力を受け付けるかどうかのフラグ
        [SerializeField] private ProcessSystem processSystem = default;

        public void Start()
        {
            var pInput = GetComponent<PlayerInput>();
            var actionMap = pInput.currentActionMap;

            _moveAction = actionMap["Move"];
        }

        public void Update()
        {
            if (!_canMove) return; // 入力を受け付けない場合は何もしない

            //アクションからコントローラの入力値を取得
            Vector2 move = _moveAction.ReadValue<Vector2>();

            DetectDirection(move);
        }

        private void DetectDirection(Vector2 move)
        {
            if (move == Vector2.zero)
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
            StartCoroutine(WaitForNextInput()); // 入力待機コルーチンを開始
        }

        private void Left()
        {
            processSystem.Left();
            _isMoving = false;
            StartCoroutine(WaitForNextInput()); // 入力待機コルーチンを開始
        }

        private void Up()
        {
            processSystem.Up();
            _isMoving = false;
            StartCoroutine(WaitForNextInput()); // 入力待機コルーチンを開始
        }

        private void Down()
        {
            processSystem.Down();
            _isMoving = false;
            StartCoroutine(WaitForNextInput()); // 入力待機コルーチンを開始
        }

        private IEnumerator WaitForNextInput()
        {
            _canMove = false; // 入力を受け付けないようにする
            yield return new WaitForSeconds(0.6f);
            _canMove = true; // 入力を再度受け付けるようにする
        }
    }
}