using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        text.text = "| Win : " + IOManager.Instance.Win.ToString() + " | Lose : " + IOManager.Instance.Lose.ToString() + " |";
    }
}
