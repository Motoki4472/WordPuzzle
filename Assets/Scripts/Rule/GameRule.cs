using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.WordBlock;

namespace Assets.Rule
{
    public class GameRule
    {
        private WordBlockOnBoard[,] Board = new WordBlockOnBoard[5, 5];
        private List<WordBlockOnBoard[]> ConnectList = new List<WordBlockOnBoard[]>();

        public GameRule()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Board[i, j] = new WordBlockOnBoard(i, j, null);
                }
            }

            //初期配置
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
                    }
                }
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
                    }
                }
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
                    }
                }
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
                    }
                }
            }
            CheckConnect();
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
                    for(int j = 0;j < connectCount;j++)
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

        // word list と照らし合わせて該当部分を消去

    }
}