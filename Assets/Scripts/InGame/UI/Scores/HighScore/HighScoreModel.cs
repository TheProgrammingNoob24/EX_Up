using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class HighScoreModel
{
    private readonly ReactiveProperty<int> _highScore = new();
    public ReadOnlyReactiveProperty<int> HighScore => _highScore;

    public void UpdateHighScore(int UpdateValue)
    {
        _highScore.Value += UpdateValue;
    }
}
