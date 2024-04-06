using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreView : MonoBehaviour
{
    [SerializeField] private GameObject _highScoreText;

    public GameObject HighScoreText { get => _highScoreText; set => _highScoreText = value; }

    public void UpdateText(double multipleValue)
    {
        _highScoreText.GetComponent<TextMeshPro>().text = multipleValue.ToString();
    }
}
