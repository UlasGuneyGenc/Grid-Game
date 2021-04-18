using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField gridSize;
        [SerializeField] private Button rebuildButton;
        [SerializeField] private int defaultTileSize;
        [SerializeField] private TextMeshProUGUI scoreText;
    
        private GameObject[,] _gridCells2D;
        private readonly Queue <GameObject> _myQ = new Queue <GameObject> ();
        private int _sizeOfGrid;
        private static int _markedCount = 0;
        private static int _score = 0;
        private float _spacing = 2.2f;
        
        
        private void Start()
        {
            // Adding handlers
            GameEventSystem.Instance.OnCellClicked += OnCellClicked;
            rebuildButton.onClick.AddListener(ResetGrid);

            // Notifying that game is started
            GameEventSystem.Instance.GameStarted(defaultTileSize, _spacing);
            GenerateGrid();
        }

        private void ResetGrid()
        {
            for (int i = 0; i < _gridCells2D.GetLength(0); i++)
            {
                for (int j = 0; j < _gridCells2D.GetLength(1); j++)
                {
                    Destroy(_gridCells2D[i,j]); 
                }
            }
            Array.Clear(_gridCells2D, 0, _gridCells2D.Length);
            transform.position = Vector3.zero;

            if (!string.IsNullOrEmpty(gridSize.text))
            {
                _sizeOfGrid = int.Parse(gridSize.text);
                GameEventSystem.Instance.ResetGrid(_sizeOfGrid);
            }
            else
                GameEventSystem.Instance.ResetGrid(defaultTileSize);
            
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            GameObject referenceTile = (GameObject) Instantiate(Resources.Load("GridCell"));
            _sizeOfGrid = defaultTileSize;
            if (gridSize.text != "")
                _sizeOfGrid = int.Parse(gridSize.text);
            _gridCells2D = new GameObject[(int)_sizeOfGrid, (int)_sizeOfGrid];
            
            for (int row = 0; row < _sizeOfGrid; row++)
            {
                for (int col = 0; col < _sizeOfGrid; col++)
                {
                    GameObject tile = Instantiate(referenceTile, transform);
                    _gridCells2D[row, col] = tile;
                    tile.GetComponent<Cell>().MyRow = row;
                    tile.GetComponent<Cell>().MyCol = col;
                    
                    float posX = col * _spacing;
                    float posy = row * -_spacing;
                    tile.transform.position = new Vector2(posX, posy);
                }
            }
            Destroy(referenceTile);
            float gridW = _sizeOfGrid * _spacing;  
            float gridH = _sizeOfGrid * _spacing;
            
            transform.position = new Vector2(-gridW / 2 + _spacing / 2, (gridH/2  - _spacing/2) );
        }

        #region events
        private void OnCellClicked(int row, int col)
        {
            //Debug.Log("row "+row+" col "+col);
       
            _myQ.Enqueue(_gridCells2D[row,col]);
            while (_myQ.Count != 0)
            {
                Cell cell = _myQ.Peek().GetComponent<Cell>();
            
                //check left
                if (cell.MyCol > 0)
                {
                    Cell cellLeft = _gridCells2D[cell.MyRow, cell.MyCol - 1].GetComponent<Cell>();
                    if ( cellLeft.IsClicked && !cellLeft.IsMarked)
                    {
                        _myQ.Enqueue(cellLeft.gameObject);
                    }
                }
                //check top
                if (cell.MyRow > 0)
                {
                    Cell cellTop = _gridCells2D[cell.MyRow-1, cell.MyCol].GetComponent<Cell>();
                    if ( cellTop.IsClicked && !cellTop.IsMarked)
                    {
                        _myQ.Enqueue(cellTop.gameObject);
                    }
                }
                //check right
                if (cell.MyCol < _sizeOfGrid-1)
                {
                    Cell cellRight = _gridCells2D[cell.MyRow, cell.MyCol+1].GetComponent<Cell>();
                    if ( cellRight.IsClicked && !cellRight.IsMarked)
                    {
                        _myQ.Enqueue(cellRight.gameObject);
                    }
                }
                //check bottom
                if (cell.MyRow < _sizeOfGrid-1)
                {
                    Cell cellBottom = _gridCells2D[cell.MyRow+1, cell.MyCol].GetComponent<Cell>();
                    if ( cellBottom.IsClicked && !cellBottom.IsMarked)
                    {
                        _myQ.Enqueue(cellBottom.gameObject);
                    }
                }

                _myQ.Dequeue();
                cell.IsMarked = true;
                _markedCount++;
            }

            if (_markedCount>=3)
            {
                GameEventSystem.Instance.ResetClick();
                _score++;
                scoreText.text = _score.ToString();
            }
            _markedCount = 0;
            GameEventSystem.Instance.ResetMark();
        
        }
    
        #endregion
        private void OnDestroy()
        {
            GameEventSystem.Instance.OnCellClicked -= OnCellClicked;
            rebuildButton.onClick.RemoveListener(GenerateGrid);
        }
    }
}

