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
    // �J�[�h�̑g�ݍ��킹���L������z��
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

        // FIX:�v��Ȃ�����
        _allCardObjects = _cardBehaviourSummary.configureAllCardCombination();
        _cardBehaviourSummary.ConfigureCardCombination();

        _scorePresenter.ResetScore();

        // �ȉ����Z�b�g�����ň͂�
        _gameOver = false;

        while (!_gameOver)
        {
            if (_isTurnEnd)
            {
                _isTurnEnd = false;
                // �B����Ă���I�u�W�F�N�g��߂�
                await TurnLoopProcess();
            }

            // �J�[�h�̃|�W�V���������Z�b�g����Ă���^�[�����I��
            if (!_isTurnEnd && _cardBehaviourSummary.InitialSet)
            {
                // InitialSet(�����ʒu)�ɔz�u���ꂽ��I�u�W�F�N�g���B��
                

                //�J�[�h���{�[�h�̗��ɉB��
                //_cardBehaviourSummary.HideCardObjects(_unselectedObjects);

                _isTurnEnd = true;
            }
        }
    }


    void ITickable.Tick()
    {

        if (_gameOver) { return; }

        // Vector3.SmoothDamp���g�p���Ă���̂�Tick()�ɋL�q
        if (_cardBehaviourSummary.Alignment) { _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination); }
        if (_cardBehaviourSummary.InitialSet) { _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination); }

    }

    private async UniTask TurnLoopProcess()
    {

        // ���݂̃^�[���ƒ��I�Ō��肳�ꂽ�J�[�h�̑g�ݍ��킹���󂯎��
        (_isFever, _selectedCardCombination) = await _cardBehaviourSummary.DecideTurn(_isFever, SelectedCardCombination);

        Debug.Log("1");
        // ���I�őI�΂�Ă��Ȃ��I�u�W�F�N�g��T��
        _unselectedObjects = CheckUnselectedObjects(_allCardObjects, _selectedCardCombination);

        //�J�[�h���{�[�h�̗��ɉB��(��\���ɂ���)
        _cardBehaviourSummary.HideCardObjects(_unselectedObjects);

        // �J�b�g�C��UI�Ɍ��݂̃^�[���\�L���Z�b�g
        var nowRoundText = await SetRoundText();

        // �^�[���ʒm�J�b�g�C��
        await _cutInPresenter.CutIn(nowRoundText);

        // ���肳�ꂽ�J�[�h�̑g�ݍ��킹���V���b�t��
        await _cardBehaviourSummary.CardShuffle(_selectedCardCombination);

        // Flg���I���ɂ��ăV���b�t�����ꂽ�J�[�h��z�u
        _cardBehaviourSummary.Alignment = true;

        // �J�[�h���I�����ꂽ
        await UniTask.WaitUntil(() => _cardSelected);
        Debug.Log("2");
        // �J�[�h�����Z���ꂽ��z�uFlg��OFF
        _cardBehaviourSummary.Alignment = false;

        // GameOver�J�[�h���I�����ꂽ��
        if (IsGameOver()) { GameOverProcess(); }

        // ���J�b�g�C��

        // _updateScoreValue���ύX����邱�Ƃɂ��AGameOver�ȊO�̂����ꂩ�̃J�[�h���I�����ꂽ�Ƃ݂Ȃ�
        await UniTask.WaitUntil(() => _updateScoreValue != 0);

        // �X�R�A���Z
        await _scorePresenter.UpdateScore(_updateScoreValue);

        // --- 1�^�[���ɍs���������I��--- //

        // �J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�ׂ̃t���O��ON
        _cardBehaviourSummary.InitialSet = true;
        Debug.Log("3");
        /*//�J�[�h���{�[�h�̗��ɉB��(��\���ɂ���)
        _cardBehaviourSummary.HideCardObjects(_unselectedObjects);*/

        // �I���ς݃t���O��OFF
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

        // �J�b�g�C�����Đ�
        var gameOverText = "GameOver!";
        _cutInPresenter.CutIn(gameOverText);
        Debug.Log($"<color=cyan>GO</color>");
    }


    /// <summary>
    /// ���I����O�ꂽ�J�[�h���L�^���邽�߂ɁA�Q�[����ɂ���"���ׂẴJ�[�h"��"���I�œ��������J�[�h"��NAND����
    /// </summary>
    /// <param name="allCardObjects">���ׂẴJ�[�h���L�^���Ă���z��</param>
    /// <param name="selectedCardCombination">����̒��I�œ��������J�[�h���L�^�����z��</param>
    /// <returns></returns>
    private GameObject[] CheckUnselectedObjects(GameObject[] allCardObjects, GameObject[] selectedCardCombination)
    {
        var unselectedSelectedObjects = allCardObjects.Except(selectedCardCombination).ToArray();

        foreach (var obj in unselectedSelectedObjects) 
        {
            Debug.Log($"�I�΂�ĂȂ���̒��g{obj.name}");
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
