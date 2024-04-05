using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviourSummary : MonoBehaviour
{

    [SerializeField] private GameObject _card_TwoTimes;
    [SerializeField] private GameObject _card_ThreeTimes;
    [SerializeField] private GameObject _card_FiveTimes;
    [SerializeField] private GameObject _card_TenTimes;
    [SerializeField] private GameObject _card_OneHalf;
    [SerializeField] private GameObject _card_OneThird;
    [SerializeField] private GameObject _card_GameOver;

    GameObject[] _twoCombinationType;
    GameObject[] _threeCombinationType;
    GameObject[] _fourCombinationType;
    GameObject[] _fiveCombinationType;
    GameObject[] _feverCombinationType;

    private float waveFrequency = 1.0f; // SIN波の周波数
    private float currentTime = 0.0f;
    private float currentValue = 0.0f;

    private Vector3 generatePosition = new Vector3(0, 0, -0.5f);
    private Quaternion generateRotate = Quaternion.Euler(90, 0, 180);

    //最初からオブジェクトは生成しておく、そして非表示に


    public void InitCardInfometion()
    {

        // 各パターンの組み合わせを設定
        _twoCombinationType = new GameObject[] { _card_TwoTimes, _card_OneHalf };
        _threeCombinationType = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_OneHalf };
        _fourCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird };
        _fiveCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
        _feverCombinationType = new GameObject[] { _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };

    }

    public void CardShuffle()
    {
        bool isFever;
        int cardQuantity;
        GameObject[] cardCombination;

        isFever = IsFever();

        if (isFever)
        {
            cardCombination = _feverCombinationType;
        }
        else
        {
            cardQuantity = GetNextValue();

            switch (cardQuantity)
            {
                case 2:
                    cardCombination = _twoCombinationType;
                    break;

                case 3:
                    cardCombination = _threeCombinationType;
                    break;

                case 4:
                    cardCombination = _fourCombinationType;
                    break;

                case 5:
                    cardCombination = _fiveCombinationType;
                    break;

                default:
                    Debug.Log($" <color=blue> エラー </color>");
                    cardCombination = _feverCombinationType;
                    break;
            }

        }
        Debug.Log($" <color=yellow> Debug.Log{cardCombination.Length} </color>");
        cardCombination = ShuffleArray(cardCombination);

        EvenlyArrange(cardCombination);
    }

    /// <summary>
    /// ランダムフィーバータイムにする
    /// </summary>
    /// <returns>フィーバータイムか否か</returns>
    private bool IsFever()
    {
        if (Random.value < 0.2f)
        {
            Debug.Log($" <color=cyan> あたり！</color>");
            return true;
        }

        Debug.Log($" <color=red> はずれ！</color>");
        return false;
    }

    /// <summary>
    /// カードを2345432という周期を設定して返すイテレータ関数　IENUMU
    /// </summary>
    /// <returns>使用カードの枚数を返す</returns>
    private int GetNextValue()
    {
        float sinValue = Mathf.Sin(currentTime * waveFrequency * Mathf.PI * 2);
        currentValue = Mathf.FloorToInt(Mathf.Lerp(2f, 5f, sinValue / 2f)); // SIN波の値を2から5の範囲にマッピング
        currentTime += Time.deltaTime;
        Debug.Log($"<color=blue>GetNextValue()結果：{currentValue}</color>");
        return Mathf.RoundToInt(currentValue);
    }

    /// <summary>
    /// カード配列の中身をシャッフル
    /// </summary>
    /// <param name="array"></param>
    static GameObject[] ShuffleArray(GameObject[] cardCombination)
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
    void EvenlyArrange(GameObject[] cardCombination)
    {
        // 画面のアスペクト比の横の大きさを取得
        float aspectRatioWidth = Screen.width / (float)Screen.height;

        // 全体幅
        float totalWidth = cardCombination.Length * aspectRatioWidth;

        // 開始位置
        float startX = -totalWidth / 2f + aspectRatioWidth / 2f;

        for (int i = 0; i < cardCombination.Length; i++)
        {
            // カードの位置を設定
            Vector3 cardPosition = new Vector3(startX + aspectRatioWidth * i, 0f, -0.5f);

            // オブジェクトを配置
            cardCombination[i].transform.position = cardPosition;
            cardCombination[i].transform.rotation = generateRotate;

            Debug.Log($"-配置完了=>i:{i} カード名{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
        }
    }
}


