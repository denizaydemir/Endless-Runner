using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Linq;

public class ItemManager : MonoBehaviour
{


    public static int InGameTotalItemAmount = 1024;

    public static CommonItem[] CommonItems;
    public static CraftableItem[] CraftableItems;
    public static List<Texture2D> TexturesList;
    public static List<CommonItem> CommonItemDropList;
    public static List<CraftableItem> CraftableItemDropList;

    private static bool _gameInitialized = false;

    public GameObject TextDebug;

    void Awake()
    {
        if (_gameInitialized)
        {
            //PrintAllInv();//
        }
        else
        {
            TexturesList = new List<Texture2D>();
            CommonItems = new CommonItem[InGameTotalItemAmount];
            CraftableItems = new CraftableItem[InGameTotalItemAmount];
            CommonItemDropList = new List<CommonItem>();
            CraftableItemDropList = new List<CraftableItem>();
            _gameInitialized = true;
            try
            {
                LoadTexturesAndAddToList();
                SortTextureListIndexes();
                XMLCommonItemsToList();
                XMLCraftableItemsToList();
                //PrintAllInv();//


            }
            catch (Exception e)
            {
                TextDebug.GetComponent<Text>().text += e.ToString() + "\n";
                Debug.Log(e);
            }
        }


    }

    void Start()
    {

    }


    private void XMLCommonItemsToList()
    {
#if UNITY_EDITOR
        XDocument itemListDocument = XDocument.Load(Application.dataPath + "/Resources/CommonItems.xml", LoadOptions.None);
#else
                TextAsset commonItemsTextAsset = Resources.Load("CommonItems") as TextAsset;
                XDocument itemListDocument = XDocument.Parse(commonItemsTextAsset.text);

#endif

        foreach (XElement item in itemListDocument.Root.Elements("CommonItem"))
        {
            int id = Convert.ToInt32((string)item.Element("Id"));
            string name = (string)item.Element("Name");
            int price = Convert.ToInt32((string)item.Element("Price"));
            Texture2D itemTexture = GetTextureOfItemId(id);
            CommonItem newItem = new CommonItem(id, name, price, itemTexture);
            CommonItems[id] = newItem;
            CommonItemDropList.Add(newItem);
        }
    }

    private void XMLCraftableItemsToList()
    {
#if UNITY_EDITOR
        XDocument itemListDocument = XDocument.Load(Application.dataPath + "/Resources/CraftableItems.xml", LoadOptions.None);
#else
                TextAsset craftableItemsTextAsset = Resources.Load("CraftableItems") as TextAsset;
                XDocument itemListDocument = XDocument.Parse(craftableItemsTextAsset.text);

#endif
         
        foreach (XElement item in itemListDocument.Root.Elements("CraftableItem"))
        {
            int id = Convert.ToInt32((string)item.Element("Id"));
            string name = (string)item.Element("Name");
            int price = Convert.ToInt32((string)item.Element("Price"));
            Texture2D itemTexture = GetTextureOfItemId(id);
            bool isPassiveItem = Convert.ToBoolean((string)item.Element("IsPassiveItem"));
            string description = (string)item.Element("Description");
            CraftableItem newItem = new CraftableItem(id, name, price, itemTexture, isPassiveItem, description);
            CraftableItems[id] = newItem;
            CraftableItemDropList.Add(newItem);
        }
    }

    private static Texture2D GetTextureOfItemId(int id)
    {
        Texture2D currentItemTexture;
        if (TexturesList[id].name == id.ToString())
        {
            currentItemTexture = TexturesList[id];
        }
        else
        {
            currentItemTexture = TexturesList.Find(t => t.name == id.ToString());
        }

        //GameObject.Find("Item").GetComponent<RawImage>().texture = currentItemTexture;
        return currentItemTexture;
    }

    private void SortTextureListIndexes()
    {
        TexturesList.Sort((x, y) => Convert.ToInt32(x.name).CompareTo(Convert.ToInt32(y.name)));
    }

    private void LoadTexturesAndAddToList()
    {
        var texturesArray = Resources.LoadAll("Textures", typeof(Texture2D)).Cast<Texture2D>();
        for (int i = 0; i < texturesArray.Count(); i++)
        {
            TexturesList.Add(texturesArray.ElementAt(i));
        }
    }


    //private void PrintAllInv()
    //{
    //    for (int i = 0; i < CommonItems.Count(); i++)
    //    {
    //        GameObject newItem = Instantiate(ItemPrefab, transform.position, transform.rotation) as GameObject;
    //        newItem.GetComponent<Transform>().SetParent(PanelInventoryItems.GetComponent<Transform>(), false);
    //        newItem.GetComponent<RawImage>().texture = TexturesList[i];
    //    }
    //}

    //public void ShowInventoryCommonItems(List<CommonItem> inventoryCommonItems)
    //{
    //    for (int i = 0; i < inventoryCommonItems.Count(); i++)
    //    {
    //        GameObject newItem = Instantiate(ItemPrefab, transform.position, transform.rotation) as GameObject;
    //        newItem.GetComponent<Transform>().SetParent(PanelInventoryItems.GetComponent<Transform>(), false);
    //        newItem.GetComponent<RawImage>().texture = TexturesList[i];
    //    }
    //}

}
