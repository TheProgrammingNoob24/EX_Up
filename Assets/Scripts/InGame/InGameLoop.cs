using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class InGameLoop : IStartable, ITickable
{

    CardBehaviourSummary _cardBehaviourSummary;
    ScorePresenter _scorePresenter;
    CutInPresenter _cutInPresenter;

    // フィーバ状態のFlg
    bool _isFever;
    public bool IsFever { get => _isFever; set => _isFever = value; }

    // カードの組み合わせを記憶する配列
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }

    [Inject]
    public InGameLoop(
       ScorePresenter scorePresenter,
       CutInPresenter cutInPresenter,
       CardBehaviourSummary cardBehaviourSummary
       )
    {
        _scorePresenter = scorePresenter;
        _cutInPresenter = cutInPresenter;
        _cardBehaviourSummary = cardBehaviourSummary;
    }

    void IStartable.Start()
    {

        _cardBehaviourSummary.configureCardCombination();

        _scorePresenter.ResetScore();
        (_isFever, _selectedCardCombination) = _cardBehaviourSummary.DecideTurn(IsFever, SelectedCardCombination);

        var a = "Change!!";
        _cutInPresenter.CutIn(a);
        //TurnLoopProcessing();

    }

    void ITickable.Tick()
    {
        _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //カードポジションを裏面で0に集める
        //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        // await _card_FiveTimes.;
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        if (Input.GetKeyDown("space"))
        {
            TurnLoopProcessing();
        }
    }

    /// <summary>
    /// ターンで行う処理
    /// </summary>
    private void TurnLoopProcessing()
    {

        //ターン通知カットイン
        _cardBehaviourSummary.DecideTurn(IsFever, _selectedCardCombination);

        //カードポジションを裏面で0に集める
        _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        _cardBehaviourSummary.CardShuffle(_selectedCardCombination);
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //カード配布

    }

    /* partial async UniTaskVoid click() {


     }*/






    /*
     * Icardを持つカードがクリックされたらスコア判定、
     * ポジションのリセット
     * カード配布
     * カットイン
     * 
     */

}
