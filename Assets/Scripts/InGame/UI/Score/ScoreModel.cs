using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class HighScoreModel
{
    // こっちが本体
    private readonly ReactiveProperty<int> _score = new();

    // 公開するプロパティ
    public ReadOnlyReactiveProperty<int> Score => _score;
}
