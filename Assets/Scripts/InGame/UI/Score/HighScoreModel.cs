using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class HighScoreModel
{
    // ���������{��
    private readonly ReactiveProperty<int> _highScore = new();

    // ���J����v���p�e�B
    public ReadOnlyReactiveProperty<int> HighScore => _highScore;
}
