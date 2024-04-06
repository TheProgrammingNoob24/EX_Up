using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ICard
{
    // カードが持つスコアを変動させる値
    double UpdateScoreValue { get; }

}
