using UnityEngine;
using System.Collections;

public class CraftableItem : Item
{

    public string Description;
    public bool IsPassiveItem;

    public CraftableItem(int id, string name, int price, Texture2D itemTexture, bool isPassiveItem, string description)
    {
        Id = id;
        Name = name;
        Price = price;
        ItemTexture = itemTexture;
        Description = description;
        IsPassiveItem = isPassiveItem;
    }


}
