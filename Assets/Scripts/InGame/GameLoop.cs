using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EX_Up.InGame.CardBehaviour;

namespace EX_Up.InGame.GameLoop
{
    public class GameLoop : MonoBehaviour
    {

        CardBehaviourSummary _cardBehaviourSummary;

        // Start is called before the first frame update
        void Start()
        {
            _cardBehaviourSummary = this.GetComponent<CardBehaviourSummary>();


            _cardBehaviourSummary.InitCardInfometion();
            //�J�[�h�|�W�V������0�ɏW�߂�
            //�J�b�g�C��
            //�J�[�h�z�z
            _cardBehaviourSummary.TurnMovement();


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
}