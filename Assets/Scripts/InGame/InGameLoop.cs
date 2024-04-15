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
using Cysharp.Threading.Tasks.Triggers;
using Cysharp.Threading.Tasks.Linq;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;


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
    bool _isTurnEnd = false;
    // public bool _isAlignment = false;
    // public bool _isInitialSet = false;

    string _nowRoundCount;
    double _updateScoreValue;
    public double UpdateScoreValue { get => _updateScoreValue; set => _updateScoreValue = value; }

    GameObject[] _allCardObjects;
    // �J�[�h�̑g�ݍ��킹���L������z��
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }
    public bool CardSelected { get => _cardSelected; set => _cardSelected = value; }

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

        _gameOver = false;
        _allCardObjects = _cardBehaviourSummary.configureAllCardCombination();
        _cardBehaviourSummary.configureCardCombination();

        _scorePresenter.ResetScore();

        //
        _isTurnEnd = false;

        while (!_gameOver)
        {

            _isTurnEnd = false;

            Debug.Log($"_gameOver����Ȃ���");

            while (!_isTurnEnd)
            {
               /* if (!_cardBehaviourSummary.InitialSet) 
                { 
                    _isTurnEnd = true; 
                    Debug.Log($"TurnEnd0");
                    
                }*/
                //_cardBehaviourSummary.InitialSet = false;
                await TurnLoopProcess();
                //_isTurnEnd = true;
            }
        }
    }

    async void ITickable.Tick()
    {

        if (_gameOver) { return; }

        if (_cardBehaviourSummary.Alignment) { _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination); }
        if (_cardBehaviourSummary.InitialSet) { _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination); }



    }

    private async UniTask TurnLoopProcess()
    {

        // �^�[��������
        (_isFever, _selectedCardCombination) = await _cardBehaviourSummary.DecideTurn(_isFever, SelectedCardCombination);

        // ���݂̃^�[����ݒ�
        var nowRoundText = await SetRoundText();

        //�^�[���ʒm�J�b�g�C��
        await _cutInPresenter.CutIn(nowRoundText);

        // ���肳�ꂽ�J�[�h�̑g�ݍ��킹���V���b�t��
        await _cardBehaviourSummary.CardShuffle(_selectedCardCombination);

        // Flg���I���ɂ��ăV���b�t�����ꂽ�J�[�h��z�u
        _cardBehaviourSummary.Alignment = true;

        // �J�[�h���I�����ꂽ
        await UniTask.WaitUntil(() => _cardSelected);

        // GameOver�J�[�h���I�����ꂽ��
        if (IsGameOver()) { GameOverProcess(); }
        // ���J�b�g�C��

        // _updateScoreValue���ύX����邱�Ƃɂ��AGameOver�ȊO�̂����ꂩ�̃J�[�h���I�����ꂽ�Ƃ݂Ȃ�
        await UniTask.WaitUntil(() => _updateScoreValue != 0);

        // �X�R�A���Z�@
        await _scorePresenter.UpdateScore(_updateScoreValue);

        _cardBehaviourSummary.Alignment = false;

        // �J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        _cardBehaviourSummary.InitialSet = true;


        //
        // if (!_cardBehaviourSummary.InitialSet) { _isTurnEnd = true; Debug.Log($"TurnEnd1"); }
        _isTurnEnd = true;
        Debug.Log($"<color=yellow>������</color>");

    }

    private bool IsGameOver()
    {
        if (_updateScoreValue == 0) { return true; }
        return false;
    }

    private void GameOverProcess()
    {
        _gameOver = true;

        // �J�b�g�C�����Đ�
        var gameOverText = "GameOver!";
        _cutInPresenter.CutIn(gameOverText);
        Debug.Log($"<color=cyan>GO</color>");
    }

    private async UniTask<string> SetRoundText()
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
