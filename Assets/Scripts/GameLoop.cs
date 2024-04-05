using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{

    CardBehaviourSummary _cardBehaviourSummary;

    // Start is called before the first frame update
    void Start()
    {
        _cardBehaviourSummary = this.GetComponent<CardBehaviourSummary>();


        _cardBehaviourSummary.InitCardInfometion();
        _cardBehaviourSummary.TurnMovement();
        

    }

    //await 
    /*
     * カードがクリックされたらスコア判定、
     * ポジションのリセット
     * 
     * 
     * 
     */
    
}
