using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class HighScoreModel
{
    // こっちが本体
    private readonly ReactiveProperty<int> _highScore = new();

    // 公開するプロパティ
    public ReadOnlyReactiveProperty<int> HighScore => _highScore;
}
