using Cysharp.Threading.Tasks;

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
        _uiAnimation.CutInAnimation();
        _cutInView.UpdateText(changeText);
        _uiAnimation.CutOutAnimation();
    }
}
