using System;
using UnityEngine;

namespace Core
{
    public class GameEventSystem : MonoBehaviour
    {
        //creating singleton object for event managing
        public static GameEventSystem Instance;

        #region events
        public event Action<int,int> OnCellClicked;
        public event Action<int,float> OnGameStarted;
        public event Action<int> OnResetGrid;
        public event Action OnResetMark;
        public event Action OnResetClick;
        #endregion

        private void Awake()
        {
            Instance = this;
        }
        
        public void CellClicked(int row,int col)
        {
            if (OnCellClicked != null)
                OnCellClicked(row,col);
        }
        
        public void ResetMark()
        {
            if (OnResetMark != null)
                OnResetMark();
        }
        
        public void ResetClick()
        {
            if (OnResetClick != null)
                OnResetClick();
        }
        
        public void ResetGrid(int newGridSize)
        {
            if (OnResetGrid != null)
                OnResetGrid(newGridSize);
        }
        
        public void GameStarted(int defaultTileSize,float spacing)
        {
            if (OnGameStarted != null)
                OnGameStarted(defaultTileSize, spacing);
        }
    }
}