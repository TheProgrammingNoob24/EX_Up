using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using VContainer.Unity;

public class ScoreModel
{
    private readonly ReactiveProperty<double> _score = new();
    public ReadOnlyReactiveProperty<double> Score => _score;

    public double GetScoreValue()
    {
        return Score.CurrentValue;
    }

    public void ResetScore(double multipleValue)
    {
        _score.Value = 100;
    }

    public void UpdateScore(double multipleValue)
    {
        _score.Value *= multipleValue;
    }

}
