using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainCanvasManager : MonoBehaviour
{
    public GameObject PanelMenu;
    public GameObject PanelInventory;

    void Awake()
    {
        Application.targetFrameRate = 300;
    }

    void Start()
    {
        ShowMenuPanel();
    }



    public void ShowInventoryPanel()
    {
        PanelInventory.GetComponent<CanvasGroup>().alpha = 1;
        PanelInventory.GetComponent<CanvasGroup>().interactable = true;
        PanelInventory.GetComponent<CanvasGroup>().blocksRaycasts = true;
        HideMenuPanel();
    }

    private void HideInventoryPanel()
    {
        PanelInventory.GetComponent<CanvasGroup>().alpha = 0;
        PanelInventory.GetComponent<CanvasGroup>().interactable = false;
        PanelInventory.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }



    public void ShowMenuPanel()
    {
        PanelMenu.GetComponent<CanvasGroup>().alpha = 1;
        PanelMenu.GetComponent<CanvasGroup>().interactable = true;
        PanelMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        HideInventoryPanel();
    }

    private void HideMenuPanel()
    {
        PanelMenu.GetComponent<CanvasGroup>().alpha = 0;
        PanelMenu.GetComponent<CanvasGroup>().interactable = false;
        PanelMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void StartRunGame()
    {
        SceneManager.LoadScene("GameScene");
    }


}


