using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardBehaviourSummary : MonoBehaviour
{

    GameLoop _gameLoop;

    // �J�[�h�I�u�W�F�N�g�Q
    [SerializeField] private GameObject _card_TwoTimes;
    [SerializeField] private GameObject _card_ThreeTimes;
    [SerializeField] private GameObject _card_FiveTimes;
    [SerializeField] private GameObject _card_TenTimes;
    [SerializeField] private GameObject _card_OneHalf;
    [SerializeField] private GameObject _card_OneThird;
    [SerializeField] private GameObject _card_GameOver;

    // �e�����ɑ΂���R���r�l�[�V����
    GameObject[] _twoCombinationType;
    GameObject[] _threeCombinationType;
    GameObject[] _fourCombinationType;
    GameObject[] _fiveCombinationType;
    GameObject[] _feverCombinationType;

    // �����ɑ΂���ϐ�
    private int[] _sequenceCycle = { 2, 3, 4, 5, 4, 3 };
    private int _sequencePointer = 0;

    // ������s���Ƃ��̌���
    private Quaternion _frontRotation = Quaternion.Euler(90, 0, -180);
    private Quaternion _backRotation = Quaternion.Euler(90, 0, 0);




    // �A�X�y�N�g��̑傫��(���䗦)
    private float aspectRatio = Screen.width / Screen.height;
    // �A�X�y�N�g��̒���
    private float center = Screen.width / Screen.height / 2f + 0.5f;

    //�ŏ�����I�u�W�F�N�g�͐������Ă����A�����Ĕ�\����


    private void Awake()
    {
        _gameLoop = this.GetComponent<GameLoop>();
    }
    /// <summary>
    /// �e�p�^�[���̃R���r�l�[�V������ݒ�
    /// </summary>
    public void configureCardCombination()
    {

        // �e�p�^�[���̑g�ݍ��킹��ݒ�
        _twoCombinationType = new GameObject[] { _card_TwoTimes, _card_OneHalf };
        _threeCombinationType = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_OneHalf };
        _fourCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird };
        _fiveCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
        _feverCombinationType = new GameObject[] { _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };

    }

    /// <summary>
    /// ���݂̃^�[����������
    /// </summary>
    public void DecideTurn()
    {

        // �����_���Ńt�B�[�o�[�^�C���ɓ˓����邩�𔻒肷��
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
                    Debug.Log($" <color=blue> �G���[ </color>");
                    _gameLoop.SelectedCardCombination = _feverCombinationType;
                    break;
            }

        }

    }

    /// <summary>
    /// �����_���Ńt�B�[�o�[�^�C���ɓ˓�����
    /// </summary>
    /// <returns>�t�B�[�o�[�^�C�����ۂ�</returns>
    private bool IsFever()
    {
        if (Random.value < 0.2f)
        {
            //Debug.Log($" <color=cyan> ������I</color>");
            return true;
        }

        //Debug.Log($" <color=red> �͂���I</color>");
        return false;
    }

    /// <summary>
    /// �J�[�h��ݒ肵�������ŕԂ��֐�
    /// </summary>
    /// <returns>�����̂Ȃ����猈�肳�ꂽ���݂̃J�[�h������Ԃ�</returns>
    private int GetNextValue()
    {
        int returnValue = _sequenceCycle[_sequencePointer % _sequenceCycle.Length];
        _sequencePointer++;
        //Debug.Log($" <color=yellow> Debug.Log{returnValue} </color>");
        return returnValue;
    }

    /// <summary>
    /// �J�[�h�z��̒��g���V���b�t��
    /// </summary>
    /// <param name="array"></param>
    public GameObject[] CardShuffle(GameObject[] cardCombination)
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
    public void EvenlyArrange(GameObject[] cardCombination)
    {

        // �J�[�h�̑������v�Z
        float totalWidth = cardCombination.Length * aspectRatio;

        // �J�n�ʒu
        float startPosX = -totalWidth / 2f + aspectRatio / 2f;

        for (int i = 0; i < cardCombination.Length; i++)
        {
            // �J�[�h�̈ʒu��ݒ�
            Vector3 cardPosition = new Vector3(startPosX + aspectRatio * i, 0f, -0.5f);

            // �I�u�W�F�N�g��z�u
            cardCombination[i].transform.position = cardPosition;
            cardCombination[i].transform.rotation = _frontRotation;

            // Debug.Log($"-�z�u����=>i:{i} �J�[�h��{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
        }
    }


    /// <summary>
    /// �J�[�h�𗠖ʏ�ԂŒ����֋Ïk���� center
    /// </summary>
    public void ResetCardPosion(GameObject[] cardCombination)
    {

        // �ŏI�I�ȃJ�[�h�̈ʒu��ݒ�
        Vector3 targetPosition = new Vector3(center, center, -0.5f);

        // �ڕW�l�ɓ��B����܂ł̂����悻�̎���[s]
        float smoothTime = 0.5f;

        // �ō����x
        float maxSpeed = 100f;

        // ���ݑ��x(SmoothDamp�̌v�Z�̂��߂ɕK�v)
        Vector3 currentVelocity = new Vector3(0, 0);


        for (int i = 0; i < cardCombination.Length; i++)
        {
            Vector3 nowPos = cardCombination[i].transform.position;

            // targetPos�ɋ��Ȃ��ꍇ�݈̂ړ�����
            if (nowPos != targetPosition)
            {
                // �I�u�W�F�N�g���ړ�
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



