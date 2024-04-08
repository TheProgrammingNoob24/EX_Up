using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class CutInPresenter
{
    UIAnimation _uiAnimation;
    CutInView _cutInView;

    public CutInPresenter(
      CutInView cutInView,
      UIAnimation uiAnimation
      )
    {
        _uiAnimation = uiAnimation;
        _cutInView = cutInView;
    }

    public void CutIn(string changeText)
    {
        _uiAnimation.CutInAnimation();
        _cutInView.UpdateText(changeText);
        _uiAnimation.CutOutAnimation();
    }
}
