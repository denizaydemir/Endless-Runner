using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RestartPanelController : MonoBehaviour
{
    private static GameObject _thisGameObject;
    private static bool _isPanelOn = false;

    void Awake()
    {
        _thisGameObject = gameObject;
        _thisGameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public static void ShowPanel()
    {
        _thisGameObject.GetComponent<CanvasGroup>().alpha = 1;
        _thisGameObject.GetComponent<CanvasGroup>().interactable = true;
        _thisGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }



    public void TogglePanel()
    {
        Debug.Log("paneled");
        if (!_isPanelOn)
        {
            _isPanelOn = true;
            _thisGameObject.GetComponent<CanvasGroup>().alpha = 1;
            _thisGameObject.GetComponent<CanvasGroup>().interactable = true;
            _thisGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            GameManager.DisablePlayerActions();
        }
        else
        {
            _isPanelOn = false;
            _thisGameObject.GetComponent<CanvasGroup>().alpha = 0;
            _thisGameObject.GetComponent<CanvasGroup>().interactable = false;
            _thisGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            GameManager.EnablePlayerActions();
        }
    }

    public static void ToggleStaticPanel()
    {
        Debug.Log("paneled");
        if (!GameManager.IsPlayerDead())
        {
            if (!_isPanelOn)
            {
                _isPanelOn = true;
                _thisGameObject.GetComponent<CanvasGroup>().alpha = 1;
                _thisGameObject.GetComponent<CanvasGroup>().interactable = true;
                _thisGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                GameManager.DisablePlayerActions();
            }
            else
            {
                _isPanelOn = false;
                _thisGameObject.GetComponent<CanvasGroup>().alpha = 0;
                _thisGameObject.GetComponent<CanvasGroup>().interactable = false;
                _thisGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                GameManager.EnablePlayerActions();
            }
        }
    }

}
