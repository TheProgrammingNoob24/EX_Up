using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX_Up.InGame.CardBehaviour
{
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

        // 各周期に対するコンビネーション
        GameObject[] _twoCombinationType;
        GameObject[] _threeCombinationType;
        GameObject[] _fourCombinationType;
        GameObject[] _fiveCombinationType;
        GameObject[] _feverCombinationType;

        // 周期に対する変数
        private int[] _sequenceCycle = { 2, 3, 4, 5, 4, 3 };
        private int sequencePointer = 0;

        // 整列を行うときの向き
        private Quaternion generateRotation = Quaternion.Euler(90, 0, 180);

        //最初からオブジェクトは生成しておく、そして非表示に


        /// <summary>
        /// 各パターンのコンビネーションを設定
        /// </summary>
        public void InitCardInfometion()
        {

            // 各パターンの組み合わせを設定
            _twoCombinationType = new GameObject[] { _card_TwoTimes, _card_OneHalf };
            _threeCombinationType = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_OneHalf };
            _fourCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird };
            _fiveCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
            _feverCombinationType = new GameObject[] { _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };

        }


        /// <summary>
        /// カードを混ぜる
        /// </summary>
        public void TurnMovement()
        {
            bool isFever; //TO DO：GameLoopに変数を用意し格納
            int cardQuantity;
            GameObject[] cardCombination;

            // ランダムでフィーバータイムに突入するかを判定する
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
            //Debug.Log($" <color=yellow> Debug.Log{cardCombination.Length} </color>");
            cardCombination = CardShuffle(cardCombination);

            EvenlyArrange(cardCombination);
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
        /// <returns>周期のにより決定された現在のカード枚数を返す</returns>
        private int GetNextValue()
        {
            int returnValue = _sequenceCycle[sequencePointer % _sequenceCycle.Length];
            sequencePointer++;

            return returnValue;
        }

        /// <summary>
        /// カード配列の中身をシャッフル
        /// </summary>
        /// <param name="array"></param>
        static GameObject[] CardShuffle(GameObject[] cardCombination)
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
            float aspectRatioWidth = Screen.width / Screen.height;

            // カードの総幅を計算
            float totalWidth = cardCombination.Length * aspectRatioWidth;

            // 開始位置
            float startPosX = -totalWidth / 2f + aspectRatioWidth / 2f;

            for (int i = 0; i < cardCombination.Length; i++)
            {
                // カードの位置を設定
                Vector3 cardPosition = new Vector3(startPosX + aspectRatioWidth * i, 0f, -0.5f);

                // オブジェクトを配置
                cardCombination[i].transform.position = cardPosition;
                cardCombination[i].transform.rotation = generateRotation;

                // Debug.Log($"-配置完了=>i:{i} カード名{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
            }
        }
    }
}

