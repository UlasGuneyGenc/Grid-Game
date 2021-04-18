using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject clickedSprite;

    private void Start()
    {
        GameEventSystem.Instance.OnResetMark += ResetMark;
        GameEventSystem.Instance.OnResetClick += ResetClick;
    }
    #region events
    private void ResetClick()
    {
        if (IsMarked)
        {
            clickedSprite.gameObject.SetActive(false);
            IsClicked = false;
        }
    }

    private void ResetMark()
    {
        IsMarked = false;
    }
    private void OnMouseDown()
    {
        if (IsClicked != true)
        {
            clickedSprite.gameObject.SetActive(true);
            IsClicked = true;
            GameEventSystem.Instance.CellClicked(MyRow,MyCol);
        }
        else
        {
            clickedSprite.gameObject.SetActive(false);
            IsClicked = false;
        }
    }
    #endregion
    
    #region publicVariables
    public int MyRow { get; set; }

    public int MyCol { get; set; }

    public bool IsClicked { get; private set; }

    public bool IsMarked { get; set; }

    #endregion
    
   private void OnDestroy()
   {
       GameEventSystem.Instance.OnResetMark -= ResetMark;
       GameEventSystem.Instance.OnResetClick -= ResetClick;
   }
}
