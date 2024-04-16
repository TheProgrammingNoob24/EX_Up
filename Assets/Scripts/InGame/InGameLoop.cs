using System;
using System.Linq;
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
using Cysharp.Threading.Tasks.Triggers;
using Cysharp.Threading.Tasks.Linq;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.ComponentModel;


public class InGameLoop : IStartable, ITickable
{
    private readonly CompositeDisposable _disposable = new();
    private CancellationTokenSource _cancellationTokenSource = new();
    CardBehaviourSummary _cardBehaviourSummary;
    ScorePresenter _scorePresenter;
    VerticalCutInPresenter _cutInPresenter;

    bool _gameOver;
    bool _isTurnEnd = true;
    bool _isFever;
    bool _cardSelected = false;
    public bool CardSelected { get => _cardSelected; set => _cardSelected = value; }


    int _nowRoundCount = 1;
    double _updateScoreValue;
    public double UpdateScoreValue { get => _updateScoreValue; set => _updateScoreValue = value; }

    GameObject[] _allCardObjects;
    // カードの組み合わせを記憶する配列
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }

    GameObject[] _unselectedObjects;

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

    async void IStartable.Start()
    {

        // FIX:要らないかも
        _allCardObjects = _cardBehaviourSummary.configureAllCardCombination();
        _cardBehaviourSummary.ConfigureCardCombination();

        _scorePresenter.ResetScore();

        // 以下リセット処理で囲う
        _gameOver = false;

        while (!_gameOver)
        {
            if (_isTurnEnd)
            {
                _isTurnEnd = false;
                // 隠されているオブジェクトを戻す
                await TurnLoopProcess();
            }

            // カードのポジションもリセットされてからターンを終了
            if (!_isTurnEnd && _cardBehaviourSummary.InitialSet)
            {
                // InitialSet(初期位置)に配置されたらオブジェクトを隠す
                

                //カードをボードの裏に隠す
                //_cardBehaviourSummary.HideCardObjects(_unselectedObjects);

                _isTurnEnd = true;
            }
        }
    }


    void ITickable.Tick()
    {

        if (_gameOver) { return; }

        // Vector3.SmoothDampを使用しているのでTick()に記述
        if (_cardBehaviourSummary.Alignment) { _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination); }
        if (_cardBehaviourSummary.InitialSet) { _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination); }

    }

    private async UniTask TurnLoopProcess()
    {

        // 現在のターンと抽選で決定されたカードの組み合わせを受け取る
        (_isFever, _selectedCardCombination) = await _cardBehaviourSummary.DecideTurn(_isFever, SelectedCardCombination);

        Debug.Log("1");
        // 抽選で選ばれていないオブジェクトを探索
        _unselectedObjects = CheckUnselectedObjects(_allCardObjects, _selectedCardCombination);

        //カードをボードの裏に隠す(非表示にする)
        _cardBehaviourSummary.HideCardObjects(_unselectedObjects);

        // カットインUIに現在のターン表記をセット
        var nowRoundText = await SetRoundText();

        // ターン通知カットイン
        await _cutInPresenter.CutIn(nowRoundText);

        // 決定されたカードの組み合わせをシャッフル
        await _cardBehaviourSummary.CardShuffle(_selectedCardCombination);

        // Flgをオンにしてシャッフルされたカードを配置
        _cardBehaviourSummary.Alignment = true;

        // カードが選択された
        await UniTask.WaitUntil(() => _cardSelected);
        Debug.Log("2");
        // カードが加算されたら配置FlgをOFF
        _cardBehaviourSummary.Alignment = false;

        // GameOverカードが選択されたか
        if (IsGameOver()) { GameOverProcess(); }

        // 横カットイン

        // _updateScoreValueが変更されることにより、GameOver以外のいずれかのカードが選択されたとみなす
        await UniTask.WaitUntil(() => _updateScoreValue != 0);

        // スコア加算
        await _scorePresenter.UpdateScore(_updateScoreValue);

        // --- 1ターンに行う処理が終了--- //

        // カードポジションを裏面で0に集める為のフラグをON
        _cardBehaviourSummary.InitialSet = true;
        Debug.Log("3");
        /*//カードをボードの裏に隠す(非表示にする)
        _cardBehaviourSummary.HideCardObjects(_unselectedObjects);*/

        // 選択済みフラグをOFF
        _cardSelected = false;

    }

    private bool IsGameOver()
    {
        if (_updateScoreValue == 0) { return true; }
        return false;
    }

    private void GameOverProcess()
    {
        _gameOver = true;

        // カットインを再生
        var gameOverText = "GameOver!";
        _cutInPresenter.CutIn(gameOverText);
        Debug.Log($"<color=cyan>GO</color>");
    }


    /// <summary>
    /// 抽選から外れたカードを記録するために、ゲーム上にある"すべてのカード"と"抽選で当たったカード"をNANDする
    /// </summary>
    /// <param name="allCardObjects">すべてのカードを記録している配列</param>
    /// <param name="selectedCardCombination">今回の抽選で当たったカードを記録した配列</param>
    /// <returns></returns>
    private GameObject[] CheckUnselectedObjects(GameObject[] allCardObjects, GameObject[] selectedCardCombination)
    {
        var unselectedSelectedObjects = allCardObjects.Except(selectedCardCombination).ToArray();

        foreach (var obj in unselectedSelectedObjects) 
        {
            Debug.Log($"選ばれてないやつの中身{obj.name}");
        }
        return unselectedSelectedObjects;
    }

    private async UniTask<string> SetRoundText()
    {
        if (_isFever)
        {
            return "Fever!";
        }

        var _nowRoundText = "ROUND" + _nowRoundCount.ToSafeString();

        _nowRoundCount++;

        return _nowRoundText;
    }
}
