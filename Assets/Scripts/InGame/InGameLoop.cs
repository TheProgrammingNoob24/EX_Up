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
    // �J�[�h�̑g�ݍ��킹���L������z��
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
            // �^�[��������
            (_isFever, _selectedCardCombination) = _cardBehaviourSummary.DecideTurn(_isFever, SelectedCardCombination);


            var nowRoundText = SetRoundText();

            //�^�[���ʒm�J�b�g�C��
            _cutInPresenter.CutIn(nowRoundText);

            // ���肳�ꂽ�J�[�h�̑g�ݍ��킹���V���b�t��
            _cardBehaviourSummary.CardShuffle(_selectedCardCombination);

            // �V���b�t�����ꂽ�J�[�h��z�u
            _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);



            // �J�[�h���I�����ꂽ TO DO:�����\�b�h���ł��Ȃ��H�@���������A�����
            await UniTask.WaitUntil(() => _cardSelected);

            // ���J�b�g�C��

            //�X�R�A���Z


            //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
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
                 Debug.Log($"{card.name}�̃N���b�N���m������");
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
