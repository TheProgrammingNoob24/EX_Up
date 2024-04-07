using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VContainer;
using LitMotion;
using UnityEngine.UIElements;
using LitMotion.Extensions;

public class Card : MonoBehaviour, ICard, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private double _multipleScoreValue;
    public double UpdateScoreValue { get => _multipleScoreValue; }

    ScorePresenter _scorePresenter;


    Quaternion _shakeAnimationRoteto = Quaternion.Euler(70, -90, 90);
    Quaternion _shakeAnimationRotetoReset = Quaternion.Euler(90, -90, 90);

    [Inject]
    public void Inject(
        ScorePresenter scorePresenter
        )
    {
        _scorePresenter = scorePresenter;
    }

    
    /// <summary>
    /// ポインターが触れたらカードが傾く
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAnimation();
    }

    private void OnPointerEnterAnimation()
    {
        var transform = this.GetComponent<Transform>();
        Debug.Log($" <color=red> C1</color>");
        LMotion.Create(transform.rotation, _shakeAnimationRoteto, 0.1f).BindToRotation(transform);
    }

    /// <summary>
    /// ポインターが離れたらカードが垂直になる
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAnimation();
    }
    private void OnPointerExitAnimation()
    {
        var transform = this.GetComponent<Transform>();
        Debug.Log($" <color=red> C2</color>");
        LMotion.Create(transform.rotation, _shakeAnimationRotetoReset, 0.1f).BindToRotation(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // カード選択アニメーション　カードが浮く
        Debug.Log($" <color=red> {_multipleScoreValue}！</color>");
        // エフェクトアニメ
        _scorePresenter.UpdateScore(UpdateScoreValue);
    }

}
