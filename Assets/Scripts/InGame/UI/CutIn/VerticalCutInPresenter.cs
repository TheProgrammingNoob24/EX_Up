using Cysharp.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

public class VerticalCutInPresenter
{
    UIAnimation _uiAnimation;
    VerticalCutInView _cutInView;

    public VerticalCutInPresenter(
      VerticalCutInView cutInView,
      UIAnimation uiAnimation
      )
    {
        _uiAnimation = uiAnimation;
        _cutInView = cutInView;
    }

    public async UniTask CutIn(string changeText)
    {
        await _uiAnimation.CutInAnimation();
        //フィーバーの時演出を入れたい
        _cutInView.UpdateText(changeText);
       
    }
}
