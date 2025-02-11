using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.WordBlock
{
    public class WordBlockOnBoard
    {
        private int[] BoardCoordinate = new int[2];
        private string Word;

        public WordBlockOnBoard(int x, int y, string word)
        {
            BoardCoordinate[0] = x;
            BoardCoordinate[1] = y;
            Word = word;
        }

        public void SetWord(string word)
        {
            Word = word;
        }

        

        public int[] GetBoardCoordinate()
        {
            return BoardCoordinate;
        }

        public string GetWord()
        {
            return Word;
        }


    }
}