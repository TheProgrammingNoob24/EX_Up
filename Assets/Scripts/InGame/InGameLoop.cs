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
using Unity.VisualScripting;


public class InGameLoop : IStartable, ITickable
{
    private readonly CompositeDisposable _disposable = new();
    private CancellationTokenSource _cancellationTokenSource = new();
    CardBehaviourSummary _cardBehaviourSummary;
    ScorePresenter _scorePresenter;
    CutInPresenter _cutInPresenter;

    bool _selected = false;
    // フィーバ状態のFlg
    bool _isFever;
    public bool IsFever { get => _isFever; set => _isFever = value; }

    GameObject[] _allCardObjects;
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
        _allCardObjects = _cardBehaviourSummary.configureAllCardCombination();
        InitRegistAllCardObjects();
        _cardBehaviourSummary.configureCardCombination();

        _scorePresenter.ResetScore();

        (_isFever, _selectedCardCombination) = _cardBehaviourSummary.DecideTurn(IsFever, SelectedCardCombination);

        var a = "Change!!";
        _cutInPresenter.CutIn(a);
        //TurnLoopProcessing();
        
    }

    async void ITickable.Tick()
    {
        _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        

        // await _card_FiveTimes.;
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        if (Input.GetKeyDown("space"))
        {
            TurnLoopProcessing();
        }

        // カードが選択された TO DO:↓メソッド化できない？　試したが、難しそう
        await UniTask.WaitUntil(() => _selected);

        // 横カットイン

        //スコア加算

        //カードポジションを裏面で0に集める
        //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

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

    private void InitRegistAllCardObjects()
    {
        foreach (GameObject i in _allCardObjects)
        {
            var card = i.GetComponent<Card>();

           var o = card.OnMouseUpAsObservable()
                .Subscribe(_ =>
            {
                Debug.Log($"{card.name}のクリック検知したヨ");
                _selected = true;
            });
        }

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
