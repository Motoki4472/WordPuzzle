using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.WordBlock;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Assets.Rule
{
    public class GameRule
    {
        private WordBlockOnBoard[,] Board = new WordBlockOnBoard[5, 5];
        private List<WordBlockOnBoard[]> ConnectList = new List<WordBlockOnBoard[]>();
        private List<WordBlockOnBoard> deleteList = new List<WordBlockOnBoard>();
        private WordList WordList = new WordList();
        private ProcessSystem processSystem;
        private List<string> BountyList = new List<string>();
        private int gameMode = 0;
        private int RemainingTurn = -1;

        public GameRule(ProcessSystem processSystem)
        {
            this.processSystem = processSystem;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Board[i, j] = new WordBlockOnBoard(i, j, null);
                }
            }
            InitialBoard();
            gameMode = processSystem.GetGameMode();
            if(gameMode == 1)
            {
                RemainingTurn = 20;
                BountyList = WordList.BountyHuntList;
                Debug.Log("BountyList: " + string.Join(", ", BountyList));
                processSystem.SetBountyHuntList(BountyList); // BountyHuntListをセット
            }
            if(gameMode == 2)
            {
                RemainingTurn = 100;
            }
            DebugLogBoard();
        }

        private void InitialBoard()
        {
            // 重複なしでランダムな位置に5個の文字を生成
            for (int i = 0; i < 5; i++)
            {
                int RandomX = Random.Range(0, 5);
                int RandomY = Random.Range(0, 5);
                while (Board[RandomX, RandomY].GetWord() != null)
                {
                    RandomX = Random.Range(0, 5);
                    RandomY = Random.Range(0, 5);
                }
                int wordId = Random.Range(0, 25);
                string word = ((char)(wordId + 97)).ToString();
                Board[RandomX, RandomY].SetWord(word);
                int direction = Random.Range(0, 4);
                processSystem.GenerateWordBlockOnBoard(RandomX, RandomY, word, direction);
            }
        }

        private void DebugLogBoard()
        {
            string[] raw = new string[5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[j, i].GetWord() == null)
                    {
                        raw[i] += "[]";
                    }
                    else
                    {
                        raw[i] += "[" + Board[j, i].GetWord() + "]";
                    }
                }
            }
            Debug.Log(string.Join("\n", raw));
        }

        //盤面の操作
        public void UpBoard()
        {
            // 左上から右下に向かって探索
            Sequence sequence = DOTween.Sequence();
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        if (Board[i, j + 1].GetWord() != null)
                        {
                            sequence.Join(processSystem.MoveWordBlockOnBoard(i, j + 1, 3));
                        }
                        Board[i, j].SetWord(Board[i, j + 1].GetWord());
                        Board[i, j + 1].SetWord(null);
                    }
                }
            }
            //最下段にランダムな文字を生成
            int RandomX = Random.Range(0, 5);
            sequence.AppendCallback(() => GenerateRandomWordBlock(RandomX, 4, true, 3));
            sequence.OnComplete(() => CheckConnect());
        }

        public void DownBoard()
        {
            // 左下から右上に向かって探索
            Sequence sequence = DOTween.Sequence();
            for (int j = 4; j > 0; j--)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        if (Board[i, j - 1].GetWord() != null)
                        {
                            sequence.Join(processSystem.MoveWordBlockOnBoard(i, j - 1, 2));
                        }
                        Board[i, j].SetWord(Board[i, j - 1].GetWord());
                        Board[i, j - 1].SetWord(null);
                    }
                }
            }
            //最上段にランダムな文字を生成
            int RandomX = Random.Range(0, 5);
            sequence.AppendCallback(() => GenerateRandomWordBlock(RandomX, 0, true, 2));
            sequence.OnComplete(() => CheckConnect());
        }

        public void RightBoard()
        {
            // 右上から左下に向かって探索
            Sequence sequence = DOTween.Sequence();
            for (int i = 4; i > 0; i--)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        if (Board[i - 1, j].GetWord() != null)
                        {
                            sequence.Join(processSystem.MoveWordBlockOnBoard(i - 1, j, 1));
                        }
                        Board[i, j].SetWord(Board[i - 1, j].GetWord());
                        Board[i - 1, j].SetWord(null);
                    }
                }
            }
            // 最左列にランダムな文字を生成
            int RandomY = Random.Range(0, 5);
            sequence.AppendCallback(() => GenerateRandomWordBlock(0, RandomY, false, 0));
            sequence.OnComplete(() => CheckConnect());
        }

        public void LeftBoard()
        {
            //左上から下に探索、右にずれて右下まで探索
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        if (Board[i + 1, j].GetWord() != null)
                        {
                            sequence.Join(processSystem.MoveWordBlockOnBoard(i + 1, j, 0));
                        }
                        Board[i, j].SetWord(Board[i + 1, j].GetWord());
                        Board[i + 1, j].SetWord(null);
                    }
                }
            }
            // 最右列にランダムな文字を生成
            int RandomY = Random.Range(0, 5);
            sequence.AppendCallback(() => GenerateRandomWordBlock(4, RandomY, false, 1));
            sequence.OnComplete(() => CheckConnect());
        }

        private void GenerateRandomWordBlock(int x, int y, bool isRow, int direction)
        {
            // 0:左 1:右 2:上 3:下
            while (Board[x, y].GetWord() != null)
            {
                if (isRow)
                {
                    x++;
                }
                else
                {
                    y++;
                }
                if (x > 4)
                {
                    x = 0;
                }
                if (y > 4)
                {
                    y = 0;
                }
            }

            string word = "";
            if (gameMode == 1 && Random.Range(0, 100) < 25)
            {
                List<string> unavailableWords = new List<string>();
                List<string> availableWords = new List<string>();
                foreach(var block in Board)
                {
                    if(block.GetWord() != null)
                    {
                        unavailableWords.Add(block.GetWord());
                    }
                }
                foreach(var Bounty in BountyList)
                {
                    foreach(var wordInBounty in Bounty)
                    {
                        if (!unavailableWords.Contains(wordInBounty.ToString()))
                        {
                            availableWords.Add(wordInBounty.ToString());
                            break;
                        }
                    }
                }
                if (availableWords.Count == 0)
                {
                    word = WordList.GenerateRandomWord(1);
                }
                else
                {
                    word = availableWords[Random.Range(0, availableWords.Count)];
                }

            }

            if (word == "")
            {
                word = WordList.GenerateRandomWord(1); // 1文字のランダムワードを生成
            }

            Board[x, y].SetWord(word);
            processSystem.GenerateWordBlockOnBoard(x, y, word, direction);
        }

        private void CheckGameEnd()
        {
            if(gameMode == 1 && BountyList.Count == 0 || RemainingTurn == 0)
            {
                // endsceneのステータスを書き換えるか別のシーンを作る
                processSystem.SetProcessStateToEnd();
            }
            if (gameMode == 2 && RemainingTurn == 0)
            {
                processSystem.SetProcessStateToEnd();
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        return;
                    }
                }
            }
            processSystem.SetProcessStateToEnd();
        }

        //盤面内の文字のつながりを検査
        private void CheckConnect()
        {
            ConnectList.Clear();
            RemainingTurn--;

            for (int i = 0; i < 5; i++)
            {
                CheckConnectHorizontal(i);
                CheckConnectVertical(i);
            }
            // ConnectListをログに表示
            // 文字数多い順でソート
            ConnectList.Sort((a, b) => b.Length - a.Length);
            for (int i = 0; i < ConnectList.Count; i++)
            {
                string word = "";
                for (int j = 0; j < ConnectList[i].Length; j++)
                {
                    if (ConnectList[i][j] != null)
                    {
                        word += ConnectList[i][j].GetWord();
                    }
                }
                Debug.Log("Connected word: " + word);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => DeleteWords());
            sequence.OnComplete(() =>
            {
                processSystem.SetProcessStateToRunning();
                CheckGameEnd();
                DebugLogBoard();
            });
        }

        // 横方向のつながりの検査
        private void CheckConnectHorizontal(int y)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int length = 2; length <= 5 - i; length++)
                {
                    string word = "";
                    WordBlockOnBoard[] connect = new WordBlockOnBoard[length];
                    for (int j = 0; j < length; j++)
                    {
                        if (Board[i + j, y].GetWord() == null)
                        {
                            break;
                        }
                        word += Board[i + j, y].GetWord();
                        connect[j] = Board[i + j, y];
                    }
                    if (word.Length == length)
                    {
                        ConnectList.Add(connect);
                    }
                }
            }
            RemoveDuplicatesFromConnectList();
        }

        // 縦方向のつながりの検査
        private void CheckConnectVertical(int x)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int length = 2; length <= 5 - i; length++)
                {
                    string word = "";
                    WordBlockOnBoard[] connect = new WordBlockOnBoard[length];
                    for (int j = 0; j < length; j++)
                    {
                        if (Board[x, i + j].GetWord() == null)
                        {
                            break;
                        }
                        word += Board[x, i + j].GetWord();
                        connect[j] = Board[x, i + j];
                    }
                    if (word.Length == length)
                    {
                        ConnectList.Add(connect);
                    }
                }
            }
            RemoveDuplicatesFromConnectList();
        }

        private void RemoveDuplicatesFromConnectList()
        {
            for (int i = 0; i < ConnectList.Count; i++)
            {
                for (int j = i + 1; j < ConnectList.Count; j++)
                {
                    if (ConnectList[i] == ConnectList[j])
                    {
                        ConnectList.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private void DeleteWords()
        {
            bool isCombo = false;
            // ConnectListを文字数多い順にソート
            ConnectList.Sort((a, b) => b.Length - a.Length);
            deleteList.Clear();
            // 各要素について辞書と照らし合わせて消す
            for (int i = 0; i < ConnectList.Count; i++)
            {
                string word = "";
                for (int j = 0; j < ConnectList[i].Length; j++)
                {
                    if (ConnectList[i][j] != null) // null チェックを追加
                    {
                        word += ConnectList[i][j].GetWord();
                    }
                }
                if (WordList.IsExistWord(word))
                {
                    isCombo = true;
                    processSystem.AddDeletedWord(word);
                    for (int j = 0; j < ConnectList[i].Length; j++)
                    {
                        // 重複を許さない
                        if (ConnectList[i][j] != null && !deleteList.Contains(ConnectList[i][j]))
                        {
                            deleteList.Add(ConnectList[i][j]);
                        }
                    }
                    // BountyHuntList内の単語を消すと残りターンが文字数＊5回復
                    if (BountyList.Contains(word))
                    {
                        for (int k = 0; k < word.Length * 10; k++)
                        {
                            RemainingTurn++;
                        }
                        BountyList.Remove(word);
                        // アニメーションを追加
                        processSystem.StrikeThroughBountyHuntWord(word);
                    }
                    if(gameMode == 1)
                    {
                        for (int k = 0; k < word.Length/1.5f; k++)
                        {
                            RemainingTurn++;
                        }
                    }
                }
            }
            if (isCombo)
            {
                processSystem.AddComboCount();
            }
            else
            {
                processSystem.ResetComboCount();
            }

            processSystem.DisappearWordBlockOnBoard(deleteList);

            // 削除対象のリストを削除
            for (int i = 0; i < deleteList.Count; i++)
            {
                if (deleteList[i] != null) // null チェックを追加
                {
                    deleteList[i].SetWord(null);
                    // score加算
                    processSystem.AddWordCountFactor();
                    processSystem.AddScore();

                    // 削除のアニメーションなどを入れる
                }
            }
            processSystem.ResetWordCountFactor();
        }

        public int GetGameMode()
        {
            return gameMode;
        }

        public int GetRemainingTurn()
        {
            return RemainingTurn;
        }

        public List<string> GetBountyList()
        {
            return BountyList;
        }
    }
}