using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class ScorePresenter
{
    private double _updatedScoreValue;

    ScoreModel _scoreModel;
    ScoreView _scoreView;

    public ScorePresenter(
        ScoreModel scoreModel,
        ScoreView scoreView
        )
    {
        _scoreModel = scoreModel;
        _scoreView = scoreView;
    }

    public void UpdateScore(double UpdateScoreValue)
    {
        _scoreModel.UpdateScore(UpdateScoreValue);
        _updatedScoreValue = _scoreModel.GetScoreValue();
        _scoreView.UpdateText(_updatedScoreValue);
    }

    public void ResetScore()
    {
        _scoreModel.ResetScore();
        double scoreValue = _scoreModel.GetScoreValue();
        _scoreView.ReseScoreText(scoreValue);

    }
}
