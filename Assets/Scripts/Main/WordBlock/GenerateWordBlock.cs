using UnityEngine;
using System.Collections.Generic;
using Assets.Rule;
using DG.Tweening;

namespace Assets.WordBlock
{
    public class GenerateWordBlock : MonoBehaviour
    {
        [SerializeField] public GameObject wordBlockPrefab;
        [SerializeField] public GameObject wordBlockParent;
        private GameObject[,] wordBlockOnBoard = new GameObject[5,5];
        private Vector2[,] wordBlockPosition = new Vector2[5,5];

        public void Awake()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    wordBlockPosition[x, y] = new Vector2(x * 77.5f -155, 155 - y * 77.5f);
                    // 親のローカル座標で用いる
                }
            }
        } 

        public void GenerateWordBlockOnBoard(int x, int y, string word,int direction)
        {
            GameObject wordBlock = Instantiate(wordBlockPrefab, wordBlockParent.transform);
            wordBlock.GetComponent<WordBlockPrefab>().Initialize(word);
            wordBlock.transform.localPosition = wordBlockPosition[x, y];
            wordBlockOnBoard[x, y] = wordBlock;
            wordBlock.GetComponent<WordBlockPrefab>().GenerateAnimation(direction, wordBlockPosition[x, y]);
        }

        private void SelectWordBlockOnBoard(int x, int y)
        {
            wordBlockOnBoard[x, y].GetComponent<WordBlockPrefab>().SelectAnimation();
        }

        public void DisappearWordBlockOnBoard(List<WordBlockOnBoard> deleteList)
        {
            for(int i = 0; i < deleteList.Count; i++)
            {
                int[] coordinate = deleteList[i].GetBoardCoordinate();
                SelectWordBlockOnBoard(coordinate[0], coordinate[1]);
            }

            for(int i = 0; i < deleteList.Count; i++)
            {
                int[] coordinate = deleteList[i].GetBoardCoordinate();
                wordBlockOnBoard[coordinate[0], coordinate[1]].GetComponent<WordBlockPrefab>().DisappearAnimation();
                wordBlockOnBoard[coordinate[0], coordinate[1]] = null;
            }
        }

        public Tween MoveWordBlockOnBoard(int x, int y, int direction)
        {
            if (wordBlockOnBoard[x, y] == null)
            {
                Debug.LogError($"No word block found at position ({x}, {y})");
                return null;
            }

            Tween moveTween = null;
            switch (direction)
            {
                // 左右上下
                case 0:
                    if (x > 0 && wordBlockOnBoard[x - 1, y] == null)
                    {
                        moveTween = wordBlockOnBoard[x, y].GetComponent<WordBlockPrefab>().MoveAnimation(wordBlockPosition[x - 1, y]);
                        wordBlockOnBoard[x - 1, y] = wordBlockOnBoard[x, y];
                        wordBlockOnBoard[x, y] = null;
                    }
                    break;
                case 1:
                    if (x < 4 && wordBlockOnBoard[x + 1, y] == null)
                    {
                        moveTween = wordBlockOnBoard[x, y].GetComponent<WordBlockPrefab>().MoveAnimation(wordBlockPosition[x + 1, y]);
                        wordBlockOnBoard[x + 1, y] = wordBlockOnBoard[x, y];
                        wordBlockOnBoard[x, y] = null;
                    }
                    break;
                case 2:
                    if (y < 4 && wordBlockOnBoard[x, y + 1] == null)
                    {
                        moveTween = wordBlockOnBoard[x, y].GetComponent<WordBlockPrefab>().MoveAnimation(wordBlockPosition[x, y + 1]);
                        wordBlockOnBoard[x, y + 1] = wordBlockOnBoard[x, y];
                        wordBlockOnBoard[x, y] = null;
                    }
                    break;
                case 3:
                    if (y > 0 && wordBlockOnBoard[x, y - 1] == null)
                    {
                        moveTween = wordBlockOnBoard[x, y].GetComponent<WordBlockPrefab>().MoveAnimation(wordBlockPosition[x, y - 1]);
                        wordBlockOnBoard[x, y - 1] = wordBlockOnBoard[x, y];
                        wordBlockOnBoard[x, y] = null;
                    }
                    break;
            }

            return moveTween;
        }
    }
}