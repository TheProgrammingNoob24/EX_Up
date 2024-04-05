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
            //カードポジションを0に集める
            //カットイン
            //カード配布
            _cardBehaviourSummary.TurnMovement();


        }

        //await click
        /*
         * Icardを持つカードがクリックされたらスコア判定、
         * ポジションのリセット
         * カード配布
         * カットイン
         * 
         */

    }
}