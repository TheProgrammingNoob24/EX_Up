using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.SocialPlatforms.Impl;

public class HighScoreModel
{
    private readonly ReactiveProperty<double> _highScore = new();
    public ReadOnlyReactiveProperty<double> HighScore => _highScore;

    public double GetHighScoreValue()
    {
        return HighScore.CurrentValue;
    }

    public void UpdateHighScore(double UpdateValue)
    {
        _highScore.Value += UpdateValue;
    }
}
