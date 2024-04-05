using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardBehaviourSummary : MonoBehaviour
{

    GameLoop _gameLoop;

    // カードオブジェクト群
    [SerializeField] private GameObject _card_TwoTimes;
    [SerializeField] private GameObject _card_ThreeTimes;
    [SerializeField] private GameObject _card_FiveTimes;
    [SerializeField] private GameObject _card_TenTimes;
    [SerializeField] private GameObject _card_OneHalf;
    [SerializeField] private GameObject _card_OneThird;
    [SerializeField] private GameObject _card_GameOver;

    // 各周期に対するコンビネーション
    GameObject[] _twoCombinationType;
    GameObject[] _threeCombinationType;
    GameObject[] _fourCombinationType;
    GameObject[] _fiveCombinationType;
    GameObject[] _feverCombinationType;

    // 周期に対する変数
    private int[] _sequenceCycle = { 2, 3, 4, 5, 4, 3 };
    private int _sequencePointer = 0;

    // 整列を行うときの向き
    private Quaternion _frontRotation = Quaternion.Euler(90, 0, -180);
    private Quaternion _backRotation = Quaternion.Euler(90, 0, 0);




    // アスペクト比の大きさ(※比率)
    private float aspectRatio = Screen.width / Screen.height;
    // アスペクト比の中央
    private float center = Screen.width / Screen.height / 2f + 0.5f;

    //最初からオブジェクトは生成しておく、そして非表示に


    private void Awake()
    {
        _gameLoop = this.GetComponent<GameLoop>();
    }
    /// <summary>
    /// 各パターンのコンビネーションを設定
    /// </summary>
    public void configureCardCombination()
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
    public void DecideTurn()
    {

        // ランダムでフィーバータイムに突入するかを判定する
        _gameLoop.IsFever = IsFever();

        if (_gameLoop.IsFever)
        {
            _gameLoop.SelectedCardCombination = _feverCombinationType;
        }
        else
        {
            switch (GetNextValue())
            {
                case 2:
                    _gameLoop.SelectedCardCombination = _twoCombinationType;
                    break;

                case 3:
                    _gameLoop.SelectedCardCombination = _threeCombinationType;
                    break;

                case 4:
                    _gameLoop.SelectedCardCombination = _fourCombinationType;
                    break;

                case 5:
                    _gameLoop.SelectedCardCombination = _fiveCombinationType;
                    break;

                default:
                    Debug.Log($" <color=blue> エラー </color>");
                    _gameLoop.SelectedCardCombination = _feverCombinationType;
                    break;
            }

        }

    }

    /// <summary>
    /// ランダムでフィーバータイムに突入する
    /// </summary>
    /// <returns>フィーバータイムか否か</returns>
    private bool IsFever()
    {
        if (Random.value < 0.2f)
        {
            //Debug.Log($" <color=cyan> あたり！</color>");
            return true;
        }

        //Debug.Log($" <color=red> はずれ！</color>");
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
    public GameObject[] CardShuffle(GameObject[] cardCombination)
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
            // カードの位置を設定
            Vector3 cardPosition = new Vector3(startPosX + aspectRatio * i, 0f, -0.5f);

            // オブジェクトを配置
            cardCombination[i].transform.position = cardPosition;
            cardCombination[i].transform.rotation = _frontRotation;

            // Debug.Log($"-配置完了=>i:{i} カード名{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
        }
    }


    /// <summary>
    /// カードを裏面状態で中央へ凝縮する center
    /// </summary>
    public void ResetCardPosion(GameObject[] cardCombination)
    {

        // 最終的なカードの位置を設定
        Vector3 targetPosition = new Vector3(center, center, -0.5f);

        // 目標値に到達するまでのおおよその時間[s]
        float smoothTime = 0.5f;

        // 最高速度
        float maxSpeed = 100f;

        // 現在速度(SmoothDampの計算のために必要)
        Vector3 currentVelocity = new Vector3(0, 0);


        for (int i = 0; i < cardCombination.Length; i++)
        {
            Vector3 nowPos = cardCombination[i].transform.position;

            // targetPosに居ない場合のみ移動処理
            if (nowPos != targetPosition)
            {
                // オブジェクトを移動
                cardCombination[i].transform.position =
                    Vector3.SmoothDamp(
                        cardCombination[i].transform.position,
                        targetPosition,
                        ref currentVelocity,
                        smoothTime * Time.deltaTime,
                        maxSpeed);

                cardCombination[i].transform.rotation = _backRotation;
                Debug.Log($"{i}");
            }
        }
    }
}



