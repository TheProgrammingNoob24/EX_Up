using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class HighScoreModel
{
    private readonly ReactiveProperty<double> _highScore = new();
    public ReadOnlyReactiveProperty<double> HighScore => _highScore;

    public void UpdateHighScore(double UpdateValue)
    {
        _highScore.Value += UpdateValue;
    }
}
