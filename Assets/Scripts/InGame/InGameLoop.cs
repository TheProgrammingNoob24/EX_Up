using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class InGameLoop : MonoBehaviour
{

    CardBehaviourSummary _cardBehaviourSummary;
    ScorePresenter _scorePresenter;
    // フィーバ状態のFlg
    bool isFever; 
    public bool IsFever { get => isFever; set => isFever = value; }

    // カードの組み合わせを記憶する配列
    GameObject[] _selectedCardCombination;
    public GameObject[] SelectedCardCombination { get => _selectedCardCombination; set => _selectedCardCombination = value; }

    [Inject]
    public void Inject(
       ScorePresenter scorePresenter
       )
    {
        _scorePresenter = scorePresenter;
    }

        private void Awake()
    {
        _cardBehaviourSummary = this.GetComponent<CardBehaviourSummary>();
    }
    void Start()
    {

        _cardBehaviourSummary.configureCardCombination();

        _scorePresenter.ResetScore();
        _cardBehaviourSummary.DecideTurn();
        //TurnLoopProcessing();

    }

    private void Update()
    {

        _cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //カードポジションを裏面で0に集める
        //_cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);


        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        if (Input.GetKeyDown("space"))
        {
            TurnLoopProcessing();
        }
    }
    /// <summary>
    /// ターンで行う処理
    /// </summary>
    private void TurnLoopProcessing()
    {

        //ターン通知カットイン
        _cardBehaviourSummary.DecideTurn();

        //カードポジションを裏面で0に集める
        _cardBehaviourSummary.ResetCardPosion(_selectedCardCombination);

        _cardBehaviourSummary.CardShuffle(_selectedCardCombination);
        //_cardBehaviourSummary.EvenlyArrange(_selectedCardCombination);
        //カード配布

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
