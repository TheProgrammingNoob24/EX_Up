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
    VerticalCutInPresenter _cutInPresenter;

    bool _gameOver;
    bool _cardSelected = false;
    bool _isFever;

    string _nowRoundCount;

    GameObject[] _allCardObjects;
    // カードの組み合わせを記憶する配列
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }

    [Inject]
    public InGameLoop(
       ScorePresenter scorePresenter,
       VerticalCutInPresenter cutInPresenter,
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

    }

    async void ITickable.Tick()
    {

        if (_gameOver)
        {
            // ターンを決定
            (_isFever, _selectedCardCombination) = _cardBehaviourSummary.DecideTurn(_isFever, SelectedCardCombination);


            var nowRoundText = SetRoundText();

            //ターン通知カットイン
            _cutInPresenter.CutIn(nowRoundText);

            // 決定されたカードの組み合わせをシャッフル
            _cardBehaviourSummary.CardShuffle(_selectedCardCombination);

            // シャッフルされたカードを配置
            _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);



            // カードが選択された TO DO:↓メソッド化できない？　試したが、難しそう
            await UniTask.WaitUntil(() => _cardSelected);

            // 横カットイン

            //スコア加算


            //カードポジションを裏面で0に集める
            //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

            //_selectedCardCombination.Up

        }


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
                 _cardSelected = true;
             });
        }

    }


    private string SetRoundText()
    {
        if (_isFever)
        {
            return "Fever!";
        }

        return "Round" + _nowRoundCount;
    }
    public void Dispose()
    {
        _disposable?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
