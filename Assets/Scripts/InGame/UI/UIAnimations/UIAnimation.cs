using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using System;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _cutInImage;
    [SerializeField] private GameObject _cutInText;

    Vector3 _initPosition = new Vector3(965, 1224, 0);
    Vector3 _stopPosition = new Vector3(965, 304, 0);
    Vector3 _fadeOutPosition = new Vector3(965, -152, 0);

    /// <summary>
    /// カットイン演出
    /// </summary>
    public async UniTask CutInAnimation()
    {

        var rectTransform = _cutInImage.GetComponent<RectTransform>();
        await LMotion.Create(_initPosition, _stopPosition, 0.5f).BindToPosition(rectTransform);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        rectTransform = _cutInImage.GetComponent<RectTransform>();
        LMotion.Create(_stopPosition, _fadeOutPosition, 0.4f).BindToPosition(rectTransform);

    }

    public void Reset()
    {
        var rectTransform = _cutInImage.GetComponent<RectTransform>();
        rectTransform.position = _initPosition;
    }
}
