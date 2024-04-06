using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class Card : MonoBehaviour, ICard, IPointerClickHandler
{
    [SerializeField]private int _multipleScoreValue;
    public  int UpdateScoreValue { get => _multipleScoreValue; }

    ScoreModel _scoreModel;

    public Card(
        ScoreModel scoreModel
        )
    {
        _scoreModel = scoreModel;
    }

    private void updateScore() 
    {
        _scoreModel.UpdateScore(_multipleScoreValue);
        Debug.Log($"AddScore:{_multipleScoreValue})");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        updateScore();
    }
    //await void OnClick
}
