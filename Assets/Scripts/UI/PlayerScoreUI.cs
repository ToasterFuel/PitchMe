using UnityEngine.UI;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    public Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = "" + score;
    }
}