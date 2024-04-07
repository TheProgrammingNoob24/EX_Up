using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using VContainer;
using LitMotion;
using UnityEngine.UIElements;
using LitMotion.Extensions;

public class Card : MonoBehaviour, ICard, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private double _multipleScoreValue;
    public double UpdateScoreValue { get => _multipleScoreValue; }

    ScorePresenter _scorePresenter;


    Quaternion _shakeAnimationRoteto = Quaternion.Euler(70, -90, 90);
    Quaternion _shakeAnimationRotetoReset = Quaternion.Euler(90, -90, 90);

    [Inject]
    public void Inject(
        ScorePresenter scorePresenter
        )
    {
        _scorePresenter = scorePresenter;
    }

    
    /// <summary>
    /// �|�C���^�[���G�ꂽ��J�[�h���X��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAnimation();
    }

    private void OnPointerEnterAnimation()
    {
        var transform = this.GetComponent<Transform>();
        Debug.Log($" <color=red> C1</color>");
        LMotion.Create(transform.rotation, _shakeAnimationRoteto, 0.1f).BindToRotation(transform);
    }

    /// <summary>
    /// �|�C���^�[�����ꂽ��J�[�h�������ɂȂ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAnimation();
    }
    private void OnPointerExitAnimation()
    {
        var transform = this.GetComponent<Transform>();
        Debug.Log($" <color=red> C2</color>");
        LMotion.Create(transform.rotation, _shakeAnimationRotetoReset, 0.1f).BindToRotation(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �J�[�h�I���A�j���[�V�����@�J�[�h������
        Debug.Log($" <color=red> {_multipleScoreValue}�I</color>");
        // �G�t�F�N�g�A�j��
        _scorePresenter.UpdateScore(UpdateScoreValue);
    }

}
