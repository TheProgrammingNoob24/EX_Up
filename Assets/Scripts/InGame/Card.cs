using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VContainer;

public class Card : MonoBehaviour, ICard, IPointerClickHandler
{
    [SerializeField] private double _multipleScoreValue;
    public double UpdateScoreValue { get => _multipleScoreValue; }

    ScorePresenter _scorePresenter;

    [Inject]
    public Card(
        ScorePresenter scorePresenter
        )
    {
        _scorePresenter = scorePresenter;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        _scorePresenter.UpdateScore(UpdateScoreValue);
    }

}
