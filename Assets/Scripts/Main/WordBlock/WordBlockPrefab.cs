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
            // 大文字にして表示
            text.text = word.ToUpper();
        }

        // 各アニメーション(生成、選択、消滅、移動、壁にぶつかる)
        public Tween GenerateAnimation(int direction, Vector2 targetPosition)
        {
            text.DOFade(1, 0.6f).From(0);
            Vector3 Offset = Vector3.zero;
            switch (direction)
            {
                case 0: Offset = Vector3.left * 50; break;
                case 1: Offset = Vector3.right * 50; break;
                case 2: Offset = Vector3.up * 50; break;
                case 3: Offset = Vector3.down * 50; break;
            }
            transform.position += Offset;
            return transform.DOLocalMove(targetPosition, 0.6f);
        }

        public Tween SelectAnimation()
        {
            text.DOColor(Color.yellow, 2f);
            transform.DOScale(1.1f, 0.3f);
            return transform.DOMoveY(transform.position.y + 5, 0.3f);
        }

        public Tween DisappearAnimation()
        {
            return text.DOFade(0, 0.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
        public Tween MoveAnimation(Vector3 targetPosition)
        {
            return transform.DOLocalMove(targetPosition, 0.6f);
        }

        public Tween HitWallAnimation(int wallType)
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
            return sequence;
        }
    }
}