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
    // �t�B�[�o��Ԃ�Flg
    bool _isFever;
    public bool IsFever { get => _isFever; set => _isFever = value; }

    GameObject[] _allCardObjects;
    // �J�[�h�̑g�ݍ��킹���L������z��
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

        // �J�[�h���I�����ꂽ TO DO:�����\�b�h���ł��Ȃ��H�@���������A�����
        await UniTask.WaitUntil(() => _selected);

        // ���J�b�g�C��

        //�X�R�A���Z

        //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        //_selectedCardCombination.Up

    }

    /// <summary>
    /// �^�[���ōs������
    /// </summary>
    private void TurnLoopProcessing()
    {

        //�^�[���ʒm�J�b�g�C��
        _cardBehaviourSummary.DecideTurn(IsFever, _selectedCardCombination);

        //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        _cardBehaviourSummary.CardShuffle(_selectedCardCombination);
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //�J�[�h�z�z

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
                _selected = true;
            });
        }

    }
   


    /* partial async UniTaskVoid click() {


     }*/






    /*
     * Icard�����J�[�h���N���b�N���ꂽ��X�R�A����A
     * �|�W�V�����̃��Z�b�g
     * �J�[�h�z�z
     * �J�b�g�C��
     * 
     */
    public void Dispose()
    {
        _disposable?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
