using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    [SerializeField]private int _updateScoreValue;

    public int UpdateScoreValue { get => _updateScoreValue; }


    public void updateScore() 
    {

    }


    //await void OnClick
}
