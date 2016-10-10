using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreTextController : MonoBehaviour
{

    private static GameObject _thisGameObject;

    void Awake()
    {
        _thisGameObject = gameObject;
    }

    public static void UpdateScoreText(int newScore)
    {
        _thisGameObject.GetComponent<Text>().text = "Score: " + newScore;
    }

}
