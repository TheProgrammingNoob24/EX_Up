using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class ScoreModel
{
    private readonly ReactiveProperty<int> _score = new();
    public ReadOnlyReactiveProperty<int> Score => _score;
    public void UpdateScore(int multipleValue)
    {
        _score.Value+= multipleValue;
    }
}
