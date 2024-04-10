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

    /* partial async UniTaskVoid click() {


     }*/






    /*
     * Icard�����J�[�h���N���b�N���ꂽ��X�R�A����A
     * �|�W�V�����̃��Z�b�g
     * �J�[�h�z�z
     * �J�b�g�C��
     * 
     */

}
