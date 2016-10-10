using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    public GameObject PanelInventoryCommonItems;
    public GameObject PanelInventoryCraftableItems;
    public static Inventory ItemInventory;
    public static int TotalMoney = 0;//pull from xml


    private static int[] _itemAmountsInInventory;
    private static bool _inventoryInitialized = false;

    public GameObject TextDebug;

    void Awake()
    {
        
    }

    void Start()
    {
        if (_inventoryInitialized)
        {
            ShowCommonInventory();
            ShowCraftableInventory();
        }
        else
        {
            InitializeItemAmountArray();

            List<CommonItem> CommonItems = new List<CommonItem>();
            List<CraftableItem> CraftableItems = new List<CraftableItem>();
            ItemInventory = new Inventory(CommonItems, CraftableItems);
            try
            {
                XMLInventoryToInventory();
                ShowCommonInventory();
                ShowCraftableInventory();
            }
            catch (Exception e)
            {
                TextDebug.GetComponent<Text>().text += "Couldn't load inventory. Created a new one" + e.ToString() + "\n";
                Debug.Log("Couldn't load inventory. Created a new one.");
            }



            _inventoryInitialized = true;
        }

    }

    private void XMLInventoryToInventory()
    {
#if UNITY_EDITOR
        string inventoryEncrypted = File.ReadAllText(Application.dataPath + "/Resources/InventoryData.xml");
        XDocument itemListDocument = XDocument.Parse(Decrypt(inventoryEncrypted));
#else
                //TextAsset inventoryEncrypted = Resources.Load("InventoryData") as TextAsset;
                string inventoryEncrypted = File.ReadAllText(Application.persistentDataPath + "/InventoryData.xml");
                XDocument itemListDocument = XDocument.Parse(Decrypt(inventoryEncrypted));

#endif




        //XDocument itemListDocument = XDocument.Load(Application.dataPath + "/Resources/InventoryData.xml", LoadOptions.None);
        foreach (XElement item in itemListDocument.Root.Elements("Item"))
        {
            int id = Convert.ToInt32((string)item.Element("Id"));
            int amount = Convert.ToInt32((string)item.Element("Amount"));

            _itemAmountsInInventory[id] = amount;
            if (IsItemCommon(id))
            {
                CommonItem newItem = new CommonItem(id, ItemManager.CommonItems[id].Name, ItemManager.CommonItems[id].Price, ItemManager.CommonItems[id].ItemTexture);
                _itemAmountsInInventory[id] = amount;
                AddCommonItemToInventory(newItem);
            }
            else
            {
                CraftableItem newItem = new CraftableItem(id, ItemManager.CraftableItems[id].Name, ItemManager.CraftableItems[id].Price, ItemManager.CraftableItems[id].ItemTexture, ItemManager.CraftableItems[id].IsPassiveItem, ItemManager.CraftableItems[id].Description);
                _itemAmountsInInventory[id] = amount;
                AddCraftableItemToInventory(newItem);
            }

        }
    }


    public static string Encrypt(string toEncrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("79123953724856384920584610502483");
        // 256-AES key
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string toDecrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("79123953724856384920584610502483");
        // AES-256 key
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }


    private void InitializeItemAmountArray()
    {
        _itemAmountsInInventory = new int[ItemManager.InGameTotalItemAmount];
        for (int i = 0; i < _itemAmountsInInventory.Length; i++)
        {
            _itemAmountsInInventory[i] = 0;
        }
    }

    private bool IsItemCommon(int id)
    {
        try
        {
            if (ItemManager.CommonItems[id].Id == id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void AddCommonItemToInventory(CommonItem item)
    {
        ItemInventory.CommonItems.Add(item);
    }

    private void AddCraftableItemToInventory(CraftableItem item)
    {
        ItemInventory.CraftableItems.Add(item);
    }

    public void ShowCommonInventory()
    {
        for (int i = 0; i < ItemInventory.CommonItems.Count; i++)
        {
            GameObject newCommonItem = Instantiate(ItemPrefab, transform.position, transform.rotation) as GameObject;
            newCommonItem.GetComponent<Transform>().SetParent(PanelInventoryCommonItems.GetComponent<Transform>(), false);
            newCommonItem.GetComponent<RawImage>().texture = ItemInventory.CommonItems[i].ItemTexture;
            newCommonItem.GetComponentInChildren<Text>().text = "x" + _itemAmountsInInventory[ItemInventory.CommonItems[i].Id];
        }
    }

    public void ShowCraftableInventory()
    {
        for (int i = 0; i < ItemInventory.CraftableItems.Count; i++)
        {
            GameObject newCraftableItem = Instantiate(ItemPrefab, transform.position, transform.rotation) as GameObject;
            newCraftableItem.GetComponent<Transform>().SetParent(PanelInventoryCraftableItems.GetComponent<Transform>(), false);
            newCraftableItem.GetComponent<RawImage>().texture = ItemInventory.CraftableItems[i].ItemTexture;
            newCraftableItem.GetComponentInChildren<Text>().text = "x" + _itemAmountsInInventory[ItemInventory.CraftableItems[i].Id];
        }
    }

    public void InventoryToXML()
    {
        try
        {
            XDocument inventoryXML = new XDocument(
        new XElement("InventoryData", ItemInventory.CommonItems.Select(x => new XElement("Item", new XElement("Id", x.Id), new XElement("Amount", _itemAmountsInInventory[x.Id]))),
        ItemInventory.CraftableItems.Select(x => new XElement("Item", new XElement("Id", x.Id), new XElement("Amount", _itemAmountsInInventory[x.Id])))
        )
            );

            string str = Encrypt(inventoryXML.ToString());
#if UNITY_EDITOR
            File.WriteAllText(Application.dataPath + "/Resources/InventoryData.xml", str);
#else
            StreamWriter fileWriter = File.CreateText(Application.persistentDataPath + "/InventoryData.xml");
            fileWriter.WriteLine(str);
            fileWriter.Close();
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public static void CommonDropAmountCheck(int id)
    {
        if (_itemAmountsInInventory[ItemManager.CommonItemDropList[id].Id] == 0)
        {
            ItemInventory.CommonItems.Add(ItemManager.CommonItemDropList[id]);
        }
        _itemAmountsInInventory[ItemManager.CommonItemDropList[id].Id]++;
    }

    public static void CraftableDropAmountCheck(int id)
    {
        if (_itemAmountsInInventory[ItemManager.CraftableItemDropList[id].Id] == 0)
        {
            ItemInventory.CraftableItems.Add(ItemManager.CraftableItemDropList[id]);
        }
        _itemAmountsInInventory[ItemManager.CraftableItemDropList[id].Id]++;
    }
}
