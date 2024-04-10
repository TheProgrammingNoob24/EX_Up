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

    // �t�B�[�o��Ԃ�Flg
    bool _isFever;
    public bool IsFever { get => _isFever; set => _isFever = value; }

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
        //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        // await _card_FiveTimes.;
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        if (Input.GetKeyDown("space"))
        {
            TurnLoopProcessing();
        }


        //�����Ɉ�Ƃ��Ă݂�

        //, cancellationToken: cancellation
        if (Input.GetMouseButtonUp(0))
        {
            anyClickedObject();// ��������foreach���Ă邩�畡�����Ă���H
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log($"�~�M�N���I");
            //����͈����
        }

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

    private void anyClickedObject()
    {
        foreach (GameObject i in _selectedCardCombination)
        {
            var card = i.GetComponent<Card>();

            card.OnMouseUpAsObservable().TakeLast(1)
                .Subscribe(_ =>
            {
                Debug.Log($"{card.name}�̃N���b�N���m������");
            });
        }


        /* for (int i = 0; i < _selectedCardCombination.Length; i++)
         {
             var a = _selectedCardCombination[i].GetComponent<Card>();

             a.OnMouseUpAsObservable().Subscribe(_ =>
             {
                 Debug.Log($"{a.name}�̃N���b�N���m������");
             });
         }*/

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
