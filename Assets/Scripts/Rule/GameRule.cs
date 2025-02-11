using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rule
{
    public class GameRule
    {
        private int[,] Board = new int[5,5];
        private List<int[,,]> ConnectList = new List<int[,,]>(); // (ひらがなの番号,x,y)

        //盤面の操作
        public void UpBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Board[j, i] == 0)
                    {
                        Board[j, i] = Board[j + 1, i];
                        Board[j + 1, i] = 0;
                    }
                }
            }
        }

        public void DownBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 4; j > 0; j--)
                {
                    if (Board[j, i] == 0)
                    {
                        Board[j, i] = Board[j - 1, i];
                        Board[j - 1, i] = 0;
                    }
                }
            }
        }

        public void RightBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 4; j > 0; j--)
                {
                    if (Board[i, j] == 0)
                    {
                        Board[i, j] = Board[i, j - 1];
                        Board[i, j - 1] = 0;
                    }
                }
            }
        }

        public void LeftBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Board[i, j] == 0)
                    {
                        Board[i, j] = Board[i, j + 1];
                        Board[i, j + 1] = 0;
                    }
                }
            }
        }

        //盤面内の文字のつながりを検査
        private void CheckConnect()
        {
            ConnectList.Clear();

        }


    }
}