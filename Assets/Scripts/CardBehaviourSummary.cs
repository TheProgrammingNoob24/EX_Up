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

    private float waveFrequency = 1.0f; // SIN�g�̎��g��
    private float currentTime = 0.0f;
    private float currentValue = 0.0f;

    private Vector3 generatePosition = new Vector3(0, 0, -0.5f);
    private Quaternion generateRotate = Quaternion.Euler(90, 0, 180);

    //�ŏ�����I�u�W�F�N�g�͐������Ă����A�����Ĕ�\����


    public void InitCardInfometion()
    {

        // �e�p�^�[���̑g�ݍ��킹��ݒ�
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
                    Debug.Log($" <color=blue> �G���[ </color>");
                    cardCombination = _feverCombinationType;
                    break;
            }

        }
        Debug.Log($" <color=yellow> Debug.Log{cardCombination.Length} </color>");
        cardCombination = ShuffleArray(cardCombination);

        EvenlyArrange(cardCombination);
    }

    /// <summary>
    /// �����_���t�B�[�o�[�^�C���ɂ���
    /// </summary>
    /// <returns>�t�B�[�o�[�^�C�����ۂ�</returns>
    private bool IsFever()
    {
        if (Random.value < 0.2f)
        {
            Debug.Log($" <color=cyan> ������I</color>");
            return true;
        }

        Debug.Log($" <color=red> �͂���I</color>");
        return false;
    }

    /// <summary>
    /// �J�[�h��2345432�Ƃ���������ݒ肵�ĕԂ��C�e���[�^�֐��@IENUMU
    /// </summary>
    /// <returns>�g�p�J�[�h�̖�����Ԃ�</returns>
    private int GetNextValue()
    {
        float sinValue = Mathf.Sin(currentTime * waveFrequency * Mathf.PI * 2);
        currentValue = Mathf.FloorToInt(Mathf.Lerp(2f, 5f, sinValue / 2f)); // SIN�g�̒l��2����5�͈̔͂Ƀ}�b�s���O
        currentTime += Time.deltaTime;
        Debug.Log($"<color=blue>GetNextValue()���ʁF{currentValue}</color>");
        return Mathf.RoundToInt(currentValue);
    }

    /// <summary>
    /// �J�[�h�z��̒��g���V���b�t��
    /// </summary>
    /// <param name="array"></param>
    static GameObject[] ShuffleArray(GameObject[] cardCombination)
    {
        System.Random rng = new System.Random();
        int n = cardCombination.Length;
        for (int i = 0; i < n; i++) { }
        {
            n--;
            //rng.Next()�ɂ���https://learn.microsoft.com/ja-jp/dotnet/api/system.random.next?view=net-8.0
            int k = rng.Next(n + 1);
            //�X���b�v
            GameObject temp = cardCombination[k];
            cardCombination[k] = cardCombination[n];
            cardCombination[n] = temp;
        }

        return cardCombination;
    }

    /// <summary>
    /// �A�X�y�N�g�����ɃJ�[�h�𐅕����ϓ��ɕ��ׂ�
    /// </summary>
    void EvenlyArrange(GameObject[] cardCombination)
    {
        // ��ʂ̃A�X�y�N�g��̉��̑傫�����擾
        float aspectRatioWidth = Screen.width / (float)Screen.height;

        // �S�̕�
        float totalWidth = cardCombination.Length * aspectRatioWidth;

        // �J�n�ʒu
        float startX = -totalWidth / 2f + aspectRatioWidth / 2f;

        for (int i = 0; i < cardCombination.Length; i++)
        {
            // �J�[�h�̈ʒu��ݒ�
            Vector3 cardPosition = new Vector3(startX + aspectRatioWidth * i, 0f, -0.5f);

            // �I�u�W�F�N�g��z�u
            cardCombination[i].transform.position = cardPosition;
            cardCombination[i].transform.rotation = generateRotate;

            Debug.Log($"-�z�u����=>i:{i} �J�[�h��{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
        }
    }
}


