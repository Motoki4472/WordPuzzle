using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Assets.WordBlock
{
    public class WordBlockPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        public void Initialize(string word)
        {
            text.text = word;
        }

        // 各アニメーション(生成、選択、消滅、移動、壁にぶつかる)
        public void GenerateAnimation(int direction, Vector2 targetPosition)
        {
            text.DOFade(1, 1.0f).From(0);
            Vector3 Offset = Vector3.zero;
            switch (direction)
            {
                case 0: Offset = Vector3.left * 100; break;
                case 1: Offset = Vector3.right * 100; break;
                case 2: Offset = Vector3.up * 100; break;
                case 3: Offset = Vector3.down * 100; break;
            }
            transform.position += Offset;
            transform.DOLocalMove(targetPosition, 1.0f);

        }

        public void SelectAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(text.DOColor(Color.yellow, 0.5f));
            sequence.Join(transform.DOScale(1.2f, 0.5f));
            sequence.Join(transform.DOMoveY(transform.position.y + 10, 0.5f));
        }

        public Tween DisappearAnimation()
        {
            return text.DOFade(0, 1.0f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        public void MoveAnimation(Vector3 targetPosition)
        {
            transform.DOLocalMove(targetPosition, 1.0f);
            Debug.Log("Move");
        }

        public void HitWallAnimation(int wallType)
        {
            Vector3 direction = Vector3.zero;
            switch (wallType)
            {
                case 0: direction = Vector3.left; break;
                case 1: direction = Vector3.right; break;
                case 2: direction = Vector3.up; break;
                case 3: direction = Vector3.down; break;
            }
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(transform.position + direction * 10, 0.2f));
            sequence.Append(transform.DOMove(transform.position, 0.2f));
            sequence.Join(transform.DOShakePosition(0.2f, 10, 10));
        }
    }
}