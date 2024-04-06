using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLoop : MonoBehaviour
{

    CardBehaviourSummary _cardBehaviourSummary;

    // �t�B�[�o��Ԃ�Flg
    bool isFever; 
    public bool IsFever { get => isFever; set => isFever = value; }

    // �J�[�h�̑g�ݍ��킹���L������z��
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }

    

    private void Awake()
    {
        _cardBehaviourSummary = this.GetComponent<CardBehaviourSummary>();
    }
    void Start()
    {

        _cardBehaviourSummary.configureCardCombination();

        _cardBehaviourSummary.DecideTurn();
        //TurnLoopProcessing();

    }

    private void Update()
    {
        //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);


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
        _cardBehaviourSummary.DecideTurn();

        //�J�[�h�|�W�V�����𗠖ʂ�0�ɏW�߂�
        _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        _cardBehaviourSummary.CardShuffle(_selectedCardCombination);
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //�J�[�h�z�z

    }

    //await click
    /*
     * Icard�����J�[�h���N���b�N���ꂽ��X�R�A����A
     * �|�W�V�����̃��Z�b�g
     * �J�[�h�z�z
     * �J�b�g�C��
     * 
     */

}