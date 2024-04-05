using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    private int _updateScoreValue = 0;

    public int UpdateScoreValue { get => _updateScoreValue; set => _updateScoreValue = value; }


    public void updateScore() { }


    //await void OnClick
}
