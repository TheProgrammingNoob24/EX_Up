using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ICard
{
    // カードが持つスコアを変動させる値
    int UpdateScoreValue { get; }

    /// <summary>
    /// updateScoreValueを基にスコアを変動させる
    /// </summary>
    public void updateScore();

}
