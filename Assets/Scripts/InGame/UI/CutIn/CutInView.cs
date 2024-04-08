using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutInView : MonoBehaviour
{

    [SerializeField] private GameObject _cutInText;

    public void UpdateText(string newText)
    {

        Debug.Log(_cutInText.GetComponent<RectTransform>().transform.position);
        Debug.Log(_cutInText.GetComponent<TextMeshProUGUI>());
        _cutInText.GetComponent<TextMeshProUGUI>().text = newText;
    }
}
