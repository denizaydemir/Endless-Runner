using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreTextController : MonoBehaviour
{

	
    private static GameObject _thisGameObject;

    void Awake()
    {
        _thisGameObject = gameObject;
    }

    public static void UpdateHighScoreText(int newScore)
    {
        _thisGameObject.GetComponent<Text>().text = "High Score: " + newScore;
    }


}
