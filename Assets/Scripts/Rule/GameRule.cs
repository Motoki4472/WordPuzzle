using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.WordBlock;
using Random = UnityEngine.Random;

namespace Assets.Rule
{
    public class GameRule
    {
        private WordBlockOnBoard[,] Board = new WordBlockOnBoard[5, 5];
        private List<WordBlockOnBoard[]> ConnectList = new List<WordBlockOnBoard[]>();
        private WordList WordList = new WordList();
        private ProcessSystem processSystem;

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

            Board[0, 0].SetWord("a");
            //Debug.Log(Board[0, 0].GetWord() + " " + Board[0, 0].GetWordId());

            //初期配置
            // logに出力
            DebugLogBoard();
        }

        private void DebugLogBoard()
        {
            string[] raw = new string[5];
            for (int i = 0; i < 5; i++)
            {
                string row = "[";
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        row += " ][";
                    }
                    else
                    {
                        row += " " + Board[i, j].GetWord() + "][";
                    }
                }
                row = row.TrimEnd('['); // 最後の '[' を削除
                raw[i] = row;
            }
            Debug.Log(raw[0] + "\n" + raw[1] + "\n" + raw[2] + "\n" + raw[3] + "\n" + raw[4]);
        }

        //盤面の操作
        public void UpBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Board[j, i].GetWord() == null)
                    {
                        Board[j, i].SetWord(Board[j + 1, i].GetWord());
                        Board[j + 1, i].SetWord(null);

                        // ここで上にずれるアニメーションを入れる
                    }
                }
            }
            //最下段にランダムな文字を生成

            int RandomX = Random.Range(0, 5);
            if (Board[RandomX, 4].GetWord() == null)
            {
                GenerateRandomWordBlock(RandomX, 4);
            }

            CheckConnect();
        }

        public void DownBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 4; j > 0; j--)
                {
                    if (Board[j, i].GetWord() == null)
                    {
                        Board[j, i].SetWord(Board[j - 1, i].GetWord());
                        Board[j - 1, i].SetWord(null);

                        // ここで下にずれるアニメーションを入れる
                    }
                }
            }

            //最上段にランダムな文字を生成
            int RandomX = Random.Range(0, 5);
            if (Board[RandomX, 0].GetWord() == null)
            {
                GenerateRandomWordBlock(RandomX, 0);
            }
            CheckConnect();
        }

        public void RightBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 4; j > 0; j--)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j] = Board[i, j - 1];
                        Board[i, j - 1].SetWord(null);

                        // ここで右にずれるアニメーションを入れる
                    }
                }
            }

            // 最左列にランダムな文字を生成
            int RandomY = Random.Range(0, 5);
            if (Board[0, RandomY].GetWord() == null)
            {
                GenerateRandomWordBlock(0, RandomY);
            }
            CheckConnect();
        }

        public void LeftBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j] = Board[i, j + 1];
                        Board[i, j + 1].SetWord(null);

                        // ここで左にずれるアニメーションを入れる
                    }
                }
            }

            // 最右列にランダムな文字を生成
            int RandomY = Random.Range(0, 5);
            if (Board[4, RandomY].GetWord() == null)
            {
                GenerateRandomWordBlock(4, RandomY);
            }
            CheckConnect();
        }

        private void GenerateRandomWordBlock(int x, int y)
        {
            int wordId = Random.Range(0, 25);
            string word = ((char)(wordId + 97)).ToString();
            //Debug.Log(word);
            Board[x, y].SetWord(word);
            //Debug.Log(Board[x, y].GetWord() + " " + Board[x, y].GetBoardCoordinate()[0] + " " + Board[x, y].GetBoardCoordinate()[1]);
        }

        //盤面内の文字のつながりを検査
        private void CheckConnect()
        {
            ConnectList.Clear();

            for (int i = 0; i < 5; i++)
            {
                CheckConnectHorizontal(i);
                CheckConnectVertical(i);
            }

            DeleteWords();
        }

        // 横方向のつながりの検査
        private void CheckConnectHorizontal(int y)
        {
            int connectCount = 0;
            WordBlockOnBoard[] connect = new WordBlockOnBoard[5];
            for (int i = 0; i < 5; i++)
            {
                connectCount++;
                if (Board[i, y].GetWord() == null)
                {
                    connectCount = 0;
                    connect = new WordBlockOnBoard[5];
                }
                if (connectCount >= 2)
                {
                    for (int j = 0; j < connectCount; j++)
                    {
                        connect[j] = Board[i - connectCount + j + 1, y];
                    }
                    ConnectList.Add(connect);
                }
            }
        }

        // 縦方向のつながりの検査
        private void CheckConnectVertical(int x)
        {
            int connectCount = 0;
            WordBlockOnBoard[] connect = new WordBlockOnBoard[5];
            for (int i = 0; i < 5; i++)
            {
                connectCount++;
                if (Board[x, i].GetWord() == null)
                {
                    connectCount = 0;
                    connect = new WordBlockOnBoard[5];
                }
                if (connectCount >= 2)
                {
                    for (int j = 0; j < connectCount; j++)
                    {
                        connect[j] = Board[x, i - connectCount + j + 1];
                    }
                    ConnectList.Add(connect);
                }
            }
        }

        private void DeleteWords()
        {
            // ConnectListを文字数多い順にソート
            ConnectList.Sort((a, b) => b.Length - a.Length);
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
                    for (int j = 0; j < ConnectList[i].Length; j++)
                    {
                        if (ConnectList[i][j] != null)
                        {
                            ConnectList[i][j].SetWord(null);
                            // ここで消えるアニメーションを入れる
                        }
                    }
                }
            }
            DebugLogBoard();
            processSystem.SetProcessStateToRunning();
        }

    }
}