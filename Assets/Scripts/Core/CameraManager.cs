using UnityEngine;

namespace Core
{
    public class CameraManager : MonoBehaviour
    {
        private int _oldSizeOfGrid;
        private float _spacing;
        private float _gridChangeRatio;
        void Start()
        {
            GameEventSystem.Instance.OnResetGrid += OnResetGrid;
            GameEventSystem.Instance.OnGameStarted += OnGameStarted;
        }
    
        #region events
        private void OnGameStarted(int defaultTileSize, float spacing)
        {
            //Debug.Log(defaultTileSize);
            _oldSizeOfGrid = defaultTileSize;
            _spacing = spacing;
            if (Camera.main is { }) Camera.main.orthographicSize = defaultTileSize*GetScreenRatio()+_spacing;
        }
        private void OnResetGrid(int newGridSize)
        {
            _gridChangeRatio = (float)newGridSize / _oldSizeOfGrid;
            _oldSizeOfGrid = newGridSize;
           // Debug.Log(_gridChangeRatio);
            if (Camera.main is { }) Camera.main.orthographicSize *=_gridChangeRatio;
        }
        #endregion
        
        private float GetScreenRatio()
        {
            float h = Screen.height;
            float w = Screen.width;
            return Mathf.Max(h, w)/Mathf.Min(h, w);
        }

        private void OnDestroy()
        {
            GameEventSystem.Instance.OnResetGrid -= OnResetGrid;
            GameEventSystem.Instance.OnGameStarted -= OnGameStarted;
        }
    }
}
