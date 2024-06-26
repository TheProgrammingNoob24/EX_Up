using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;
using R3;
using R3.Triggers;
using Cysharp.Threading.Tasks;

public class CardBehaviourSummary : MonoBehaviour
{

    // カードオブジェクト群
    [SerializeField] private GameObject _card_TwoTimes;
    [SerializeField] private GameObject _card_ThreeTimes;
    [SerializeField] private GameObject _card_FiveTimes;
    [SerializeField] private GameObject _card_TenTimes;
    [SerializeField] private GameObject _card_OneHalf;
    [SerializeField] private GameObject _card_OneThird;
    [SerializeField] private GameObject _card_GameOver;


    // 全カードオブジェクトの組み合わせ
    GameObject[] _allCardCombination;

    // 各周期に対する組み合わせ
    GameObject[] _twoCombinationType;
    GameObject[] _threeCombinationType;
    GameObject[] _fourCombinationType;
    GameObject[] _fiveCombinationType;
    GameObject[] _feverCombinationType;

    // 周期に関する変数
    private int[] _sequenceCycle = { 2, 3, 4, 5, 4, 3 };
    private int _sequencePointer = 0;

    Vector3 _appearPosition = new Vector3(-0.962395608f, 27.3865585f, -7.06599998f);
    Vector3 _hidePosition = new Vector3(-0.962395608f, 27.3865585f, -4);

    // 整列を行うときのオブジェクトの向き
    private Quaternion _frontRotation = Quaternion.Euler(90, 0, -180);
    private Quaternion _backRotation = Quaternion.Euler(90, 0, 0);

    // アスペクト比の大きさ(※比率)
    private float aspectRatio = Screen.width / Screen.height;
    // アスペクト比の中央
    private float center = Screen.width / Screen.height / 2f + 0.5f;


    private bool _isAlignment;
    private bool _isInitialSet;
    // 移動処理の共通変数
    // 目標値に到達するまでのおおよその時間[s]
    private float _smoothTime = 0.5f;
    // 最高速度
    private float _maxSpeed = 100f;
    // 現在速度(SmoothDampの計算のために必要)
    private Vector3 _currentVelocity = Vector3.zero;

    public bool Alignment { get => _isAlignment; set => _isAlignment = value; }
    public bool InitialSet { get => _isInitialSet; set => _isInitialSet = value; }

    public GameObject[] configureAllCardCombination()
    {
        _allCardCombination = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
        return _allCardCombination;
    }


    /// <summary>
    /// 各パターンのコンビネーションを設定
    /// </summary>
    public void ConfigureCardCombination()
    {

        // 各パターンの組み合わせを設定
        _twoCombinationType = new GameObject[] { _card_TwoTimes, _card_OneHalf };
        _threeCombinationType = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_OneHalf };
        _fourCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird };
        _fiveCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
        _feverCombinationType = new GameObject[] { _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
    }

    /// <summary>
    /// 現在のターン数を決定
    /// </summary>
    public async UniTask<(bool, GameObject[])> DecideTurn(bool isFever, GameObject[] selectedCardCombination)
    {

        // ランダムでフィーバータイムに突入するかを判定する
        isFever = IsFever();

        if (isFever)
        {
            selectedCardCombination = _feverCombinationType;
        }
        else
        {
            switch (GetNextValue())
            {
                case 2:
                    selectedCardCombination = _twoCombinationType;
                    break;

                case 3:
                    selectedCardCombination = _threeCombinationType;
                    break;

                case 4:
                    selectedCardCombination = _fourCombinationType;
                    break;

                case 5:
                    selectedCardCombination = _fiveCombinationType;
                    break;

                default:
                    Debug.Log($" <color=blue> エラー </color>");
                    selectedCardCombination = _feverCombinationType;
                    break;
            }
        }
        return (isFever, selectedCardCombination);
    }

    /// <summary>
    /// ランダムでフィーバータイムに突入する
    /// </summary>
    /// <returns>フィーバータイムか否か</returns>
    private bool IsFever()
    {
        if (Random.value < 0.1f)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// カードを設定した周期で返す関数
    /// </summary>
    /// <returns>周期のなかから決定された現在のカード枚数を返す</returns>
    private int GetNextValue()
    {
        int returnValue = _sequenceCycle[_sequencePointer % _sequenceCycle.Length];
        _sequencePointer++;
        //Debug.Log($" <color=yellow> Debug.Log{returnValue} </color>");
        return returnValue;
    }

    /// <summary>
    /// カード配列の中身をシャッフル
    /// </summary>
    /// <param name="array"></param>
    public async UniTask<GameObject[]> CardShuffle(GameObject[] cardCombination)
    {
        System.Random rng = new System.Random();
        int n = cardCombination.Length;
        for (int i = 0; i < n; i++) { }
        {
            n--;
            //rng.Next()についてhttps://learn.microsoft.com/ja-jp/dotnet/api/system.random.next?view=net-8.0
            int k = rng.Next(n + 1);
            //スワップ
            GameObject temp = cardCombination[k];
            cardCombination[k] = cardCombination[n];
            cardCombination[n] = temp;
        }

        return cardCombination;
    }

    /// <summary>
    /// アスペクト比を基準にカードを水平且つ均等に並べる
    /// </summary>
    public void EvenlyArrange(GameObject[] cardCombination)
    {
        // カードの総幅を計算
        float totalWidth = cardCombination.Length * aspectRatio;

        // 開始位置
        float startPosX = -totalWidth / 2f + aspectRatio / 2f;

        for (int i = 0; i < cardCombination.Length; i++)
        {
            // 移動先のカード位置を設定
            Vector3 targetPosition = new Vector3((startPosX + aspectRatio * i) * 2, 0f, -0.5f);

            Vector3 nowPos = cardCombination[i].transform.position;
            // targetPosに居ない場合のみ移動処理
            if (nowPos != targetPosition)
            {
                _isAlignment = true;
                // オブジェクトを移動
                cardCombination[i].transform.position =
                    Vector3.SmoothDamp(
                        nowPos,
                        targetPosition,
                        ref _currentVelocity,
                        _smoothTime * Time.deltaTime,
                        _maxSpeed);
                cardCombination[i].transform.rotation = _frontRotation;

            }
            else if (nowPos == targetPosition)
            {
                _isAlignment = false;
            }
        }
    }

    /// <summary>
    /// カードを裏面状態で中央へ移動する
    /// </summary>
    public void ResetCardPosion(GameObject[] cardCombination)
    {

        // 移動先のカード位置を設定
        Vector3 targetPosition = new Vector3(center, center + 2.8f, -0.5f);

        for (int i = 0; i < cardCombination.Length; i++)
        {
            Vector3 nowPos = cardCombination[i].transform.position;

            // targetPosに居ない場合のみ移動処理
            if (nowPos != targetPosition)
            {
                _isInitialSet = true;
                // オブジェクトを移動
                cardCombination[i].transform.position =
                    Vector3.SmoothDamp(
                        cardCombination[i].transform.position,
                        targetPosition,
                        ref _currentVelocity,
                        _smoothTime * Time.deltaTime,
                        _maxSpeed);

                cardCombination[i].transform.rotation = _backRotation;
            }
            else if (nowPos == targetPosition)
            {
                _isInitialSet = false;
            }
        }
    }

    /// <summary>
    /// ボード裏のカードオブジェクトを出現させる
    /// </summary>
    public void AppearCardObjects()
    {

    }

    /// <summary>
    /// カードオブジェクトをボード裏に隠す
    /// </summary>
    public void HideCardObjects(GameObject[] HideObjects)
    {
        foreach (GameObject obj in HideObjects)
        {
            var oldPos = obj.GetComponent<Transform>();
            oldPos.position = _hidePosition;
            Debug.Log($"{obj.name}hideした POS{oldPos.position}");
        }
    }

}



