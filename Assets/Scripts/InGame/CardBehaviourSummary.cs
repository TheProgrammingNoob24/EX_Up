using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EX_Up.InGame.CardBehaviour
{
    public class CardBehaviourSummary : MonoBehaviour
    {

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
        private int sequencePointer = 0;

        // ������s���Ƃ��̌���
        private Quaternion generateRotation = Quaternion.Euler(90, 0, 180);

        //�ŏ�����I�u�W�F�N�g�͐������Ă����A�����Ĕ�\����


        /// <summary>
        /// �e�p�^�[���̃R���r�l�[�V������ݒ�
        /// </summary>
        public void InitCardInfometion()
        {

            // �e�p�^�[���̑g�ݍ��킹��ݒ�
            _twoCombinationType = new GameObject[] { _card_TwoTimes, _card_OneHalf };
            _threeCombinationType = new GameObject[] { _card_TwoTimes, _card_ThreeTimes, _card_OneHalf };
            _fourCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird };
            _fiveCombinationType = new GameObject[] { _card_ThreeTimes, _card_FiveTimes, _card_OneHalf, _card_OneThird, _card_GameOver };
            _feverCombinationType = new GameObject[] { _card_FiveTimes, _card_TenTimes, _card_OneHalf, _card_OneThird, _card_GameOver };

        }


        /// <summary>
        /// �J�[�h��������
        /// </summary>
        public void TurnMovement()
        {
            bool isFever; //TO DO�FGameLoop�ɕϐ���p�ӂ��i�[
            int cardQuantity;
            GameObject[] cardCombination;

            // �����_���Ńt�B�[�o�[�^�C���ɓ˓����邩�𔻒肷��
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
            //Debug.Log($" <color=yellow> Debug.Log{cardCombination.Length} </color>");
            cardCombination = CardShuffle(cardCombination);

            EvenlyArrange(cardCombination);
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
        /// <returns>�����̂ɂ�茈�肳�ꂽ���݂̃J�[�h������Ԃ�</returns>
        private int GetNextValue()
        {
            int returnValue = _sequenceCycle[sequencePointer % _sequenceCycle.Length];
            sequencePointer++;

            return returnValue;
        }

        /// <summary>
        /// �J�[�h�z��̒��g���V���b�t��
        /// </summary>
        /// <param name="array"></param>
        static GameObject[] CardShuffle(GameObject[] cardCombination)
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
            float aspectRatioWidth = Screen.width / Screen.height;

            // �J�[�h�̑������v�Z
            float totalWidth = cardCombination.Length * aspectRatioWidth;

            // �J�n�ʒu
            float startPosX = -totalWidth / 2f + aspectRatioWidth / 2f;

            for (int i = 0; i < cardCombination.Length; i++)
            {
                // �J�[�h�̈ʒu��ݒ�
                Vector3 cardPosition = new Vector3(startPosX + aspectRatioWidth * i, 0f, -0.5f);

                // �I�u�W�F�N�g��z�u
                cardCombination[i].transform.position = cardPosition;
                cardCombination[i].transform.rotation = generateRotation;

                // Debug.Log($"-�z�u����=>i:{i} �J�[�h��{cardCombination[i].name}:Pos{cardCombination[i].transform.position}-");
            }
        }
    }
}

