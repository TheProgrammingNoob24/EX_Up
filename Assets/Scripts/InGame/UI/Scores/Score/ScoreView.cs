using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private GameObject _scoreText;

    public GameObject ScoreText { get => _scoreText; set => _scoreText = value; }

    public void UpdateText(double multipleValue)
    {
        _scoreText.GetComponent<TextMeshPro>().text = multipleValue.ToString();
    }
}
