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
        private List<WordBlockOnBoard> deleteList = new List<WordBlockOnBoard>();
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
            InitialBoard();

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
            }
        }

        private void DebugLogBoard()
        {
            string[] raw = new string[5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //raw[i] += Board[j, i].GetBoardCoordinate()[0] + "," + Board[i, j].GetBoardCoordinate()[1] + " ";
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

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j].SetWord(Board[i, j + 1].GetWord());
                        Board[i, j + 1].SetWord(null);

                        // ここで上にずれるアニメーションを入れる
                    }
                }
            }
            //最下段にランダムな文字を生成

            int RandomX = Random.Range(0, 5);
            GenerateRandomWordBlock(RandomX, 4, true);

            CheckConnect();
        }

        public void DownBoard()
        {

            //　左下から右上に向かって探索

            for (int j = 4; j > 0; j--)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j].SetWord(Board[i, j - 1].GetWord());
                        Board[i, j - 1].SetWord(null);

                        // ここで下にずれるアニメーションを入れる
                    }
                }
            }

            //最上段にランダムな文字を生成
            int RandomX = Random.Range(0, 5);
            GenerateRandomWordBlock(RandomX, 0, true);
            CheckConnect();
        }

        public void RightBoard()
        {
            // 右上から左下に向かって探索
            for (int i = 4; i > 0; i--)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j].SetWord(Board[i - 1, j].GetWord());
                        Board[i - 1, j].SetWord(null);

                        // ここで右にずれるアニメーションを入れる
                    }
                }
            }

            // 最左列にランダムな文字を生成
            int RandomY = Random.Range(0, 5);
            GenerateRandomWordBlock(0, RandomY, false);
            CheckConnect();
        }

        public void LeftBoard()
        {
            //左上から下に探索、右にずれて右下まで探索
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Board[i, j].GetWord() == null)
                    {
                        Board[i, j].SetWord(Board[i + 1, j].GetWord());
                        Board[i + 1, j].SetWord(null);

                        // ここで左にずれるアニメーションを入れる
                    }
                }
            }

            // 最右列にランダムな文字を生成
            // 置ける場所を探し続ける置けるとこがなければゲーム終了
            int RandomY = Random.Range(0, 5);
            GenerateRandomWordBlock(4, RandomY, false);

            CheckConnect();
        }

        private void GenerateRandomWordBlock(int x, int y, bool isRow)
        {
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
            int wordId = Random.Range(0, 25);
            string word = ((char)(wordId + 97)).ToString();
            Board[x, y].SetWord(word);
        }

        private void CheckGameEnd()
        {
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

            for (int i = 0; i < 5; i++)
            {
                CheckConnectHorizontal(i);
                CheckConnectVertical(i);
            }
            // ConnectListをログに表示
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
            }


            DeleteWords();
            processSystem.SetProcessStateToRunning();
            CheckGameEnd();
            DebugLogBoard();

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
                    for (int j = 0; j < ConnectList[i].Length; j++)
                    {
                        // 重複を許さない
                        if (ConnectList[i][j] != null && !deleteList.Contains(ConnectList[i][j]))
                        {
                            deleteList.Add(ConnectList[i][j]);
                        }
                    }
                }
            }

            // 削除対象のリストを削除
            for (int i = 0; i < deleteList.Count; i++)
            {
                if (deleteList[i] != null) // null チェックを追加
                {
                    deleteList[i].SetWord(null);
                    // 削除のアニメーションなどを入れる
                }
            }
        }

    }
}