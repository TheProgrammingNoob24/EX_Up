using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using VContainer.Unity;

public class ScoreModel
{
    private readonly ReactiveProperty<double> _score = new();
    public ReadOnlyReactiveProperty<double> Score => _score;


    public void UpdateScore(double multipleValue)
    {
        _score.Value *= multipleValue;
    }

    public double GetScoreValue()
    {
        return Score.CurrentValue;
    }
}
