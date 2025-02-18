using UnityEngine;

namespace Assets.Rule
{
    public class ScoreSystem
    {
        private int score = 0;
        private float WordCountFactor = 1.0f;
        private float TurnCountFactor = 1.0f;
        private int ComboCount = 0;
        private int addScore = 0;
        private const string HighScoreKey = "HighScore";
        private const string ClassicHighScoreKey = "ClassicHighScore";
        private const string EndlessHighScoreKey = "EndlessHighScore";

        public void ScoreUpdate()
        {
            if(addScore > 100)
            {
                score += 10;
                addScore -= 10;
            }
            if(addScore > 0)
            {
                score ++;
                addScore --;
            }

            SaveHighScore();
            SaveEndlessHighScore();
        }

        public int GetScore()
        {
            return score;
        }

        public int GetComboCount()
        {
            return ComboCount;
        }

        public void AddScore()
        {
            addScore += (int)(50 * WordCountFactor * TurnCountFactor);
        }

        public void AddComboCount()
        {
            ComboCount++;
        }

        public void ResetComboCount()
        {
            ComboCount = 0;
        }

        public void AddWordCountFactor()
        {
            WordCountFactor += 0.1f;
        }

        public void ResetWordCountFactor()
        {
            WordCountFactor = 1.0f;
        }

        public void AddTurnCountFactor()
        {
            TurnCountFactor += 0.025f;
        }

        public void ResetTurnCountFactor()
        {
            TurnCountFactor = 1.0f;
        }

        public void ResetAll()
        {
            score = 0;
            WordCountFactor = 1.0f;
            TurnCountFactor = 1.0f;
            ComboCount = 0;
            addScore = 0;
            SaveHighScore();
            SaveEndlessHighScore();
        }

        public void SaveHighScore()
        {
            int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
            if (score > highScore)
            {
                PlayerPrefs.SetInt(HighScoreKey, score);
                PlayerPrefs.Save();
            }
        }

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HighScoreKey);
        }

        public void SaveClassicHighScore()
        {
            int classicHighScore = PlayerPrefs.GetInt(ClassicHighScoreKey, 0);
            if (score > classicHighScore)
            {
                PlayerPrefs.SetInt(ClassicHighScoreKey, score);
                PlayerPrefs.Save();
            }
        }

        public int GetClassicHighScore()
        {
            return PlayerPrefs.GetInt(ClassicHighScoreKey, 0);
        }

        public void ResetClassicHighScore()
        {
            PlayerPrefs.DeleteKey(ClassicHighScoreKey);
        }

        public void SaveEndlessHighScore()
        {
            int endlessHighScore = PlayerPrefs.GetInt(EndlessHighScoreKey, 0);
            if (score > endlessHighScore)
            {
                PlayerPrefs.SetInt(EndlessHighScoreKey, score);
                PlayerPrefs.Save();
            }
        }

        public int GetEndlessHighScore()
        {
            return PlayerPrefs.GetInt(EndlessHighScoreKey, 0);
        }

        public void ResetEndlessHighScore()
        {
            PlayerPrefs.DeleteKey(EndlessHighScoreKey);
        }
    }
}