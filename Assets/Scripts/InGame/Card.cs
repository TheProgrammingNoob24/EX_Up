using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VContainer;
using LitMotion;
using LitMotion.Extensions;

public class Card : MonoBehaviour, ICard, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{

    [SerializeField] private double _multipleScoreValue;
    public double UpdateScoreValue { get => _multipleScoreValue; }

    ScorePresenter _scorePresenter;

    Quaternion _shakeAnimationRoteto = Quaternion.Euler(70, -90, 90);
    Quaternion _shakeAnimationRotetoReset = Quaternion.Euler(90, -90, 90);

    Vector3 _enlargementAnimationScale = new Vector3(1.5f, 1.5f, 1.5f);
    Vector3 _reductionAnimationScale = new Vector3(1, 1, 1);

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
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterCardAnimation();
    }

    private void OnPointerEnterCardAnimation()
    {
        var transform = this.GetComponent<Transform>();
        LMotion.Create(transform.rotation, _shakeAnimationRoteto, 0.1f).BindToRotation(transform);
    }

    /// <summary>
    /// ポインターが離れたらカードが垂直になる
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitCardAnimation();
    }
    private void OnPointerExitCardAnimation()
    {
        var transform = this.GetComponent<Transform>();
        LMotion.Create(transform.rotation, _shakeAnimationRotetoReset, 0.1f).BindToRotation(transform);
    }


    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpCardAnimation();
        _scorePresenter.UpdateScore(UpdateScoreValue);
    }

    private void OnPointerUpCardAnimation()
    {
        var transform = this.GetComponent<Transform>();
        LMotion.Create(transform.localScale, _reductionAnimationScale, 0.1f).BindToLocalScale(transform);
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownCardAnimation();
    }

    private void OnPointerDownCardAnimation()
    {
        var transform = this.GetComponent<Transform>();
        LMotion.Create(transform.localScale, _enlargementAnimationScale, 0.1f).BindToLocalScale(transform);
    }

}
