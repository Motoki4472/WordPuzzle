using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Assets.Result
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ChangeResultText : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText = default;
        [SerializeField] private TMP_Text highScoreText = default;
        [SerializeField] private TMP_Text turnText = default;
        private TextMeshProUGUI ScoreTextUGUI;
        private TextMeshProUGUI HighScoreTextUGUI;
        private TextMeshProUGUI TurnTextUGUI;
        public int score = 0;
        public int turn = 0;
        private const string HighScoreKey = "HighScore";

        private void Start()
        {
            scoreText.text = $"SCORE: {score}";
            highScoreText.text = $"HIGH SCORE: {PlayerPrefs.GetInt(HighScoreKey, 0)}";
            turnText.text = $"TURN: {turn}";
            ScoreTextUGUI = scoreText.GetComponent<TextMeshProUGUI>();
            HighScoreTextUGUI = highScoreText.GetComponent<TextMeshProUGUI>();
            TurnTextUGUI = turnText.GetComponent<TextMeshProUGUI>();
            ScoreTextUGUI.maxVisibleCharacters = 0;
            HighScoreTextUGUI.maxVisibleCharacters = 0;
            TurnTextUGUI.maxVisibleCharacters = 0;

            ScoreTextUGUI.DOMaxVisibleCharacters(ScoreTextUGUI.text.Length, 0.9375f).SetDelay(1.3f);
            HighScoreTextUGUI.DOMaxVisibleCharacters(HighScoreTextUGUI.text.Length, 1.5f).SetDelay(1.6f);
            TurnTextUGUI.DOMaxVisibleCharacters(TurnTextUGUI.text.Length, 0.75f).SetDelay(1.0f);
        }

    }

}