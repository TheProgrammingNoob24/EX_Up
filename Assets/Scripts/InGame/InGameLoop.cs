using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
using R3.Triggers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;


public class InGameLoop : IStartable, ITickable
{
    private readonly CompositeDisposable _disposable = new();
    private CancellationTokenSource _cancellationTokenSource = new();
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


        //試しに一個とってみる

        //, cancellationToken: cancellation
        if (Input.GetMouseButtonUp(0))
        {
            anyClickedObject();// こっちはforeachしてるから複数個取れている？
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log($"ミギクリ！");
            //これは一個だけ
        }

        //_selectedCardCombination.Up

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

    private void anyClickedObject()
    {
        foreach (GameObject i in _selectedCardCombination)
        {
            var card = i.GetComponent<Card>();

            card.OnMouseUpAsObservable().TakeLast(1)
                .Subscribe(_ =>
            {
                Debug.Log($"{card.name}のクリック検知したヨ");
            });
        }


        /* for (int i = 0; i < _selectedCardCombination.Length; i++)
         {
             var a = _selectedCardCombination[i].GetComponent<Card>();

             a.OnMouseUpAsObservable().Subscribe(_ =>
             {
                 Debug.Log($"{a.name}のクリック検知したヨ");
             });
         }*/

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
    public void Dispose()
    {
        _disposable?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
