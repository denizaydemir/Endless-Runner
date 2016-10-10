using UnityEngine;
using System.Collections;

public class CommonItem : Item
{

    public CommonItem(int id, string name, int price, Texture2D itemTexture)
    {
        Id = id;
        Name = name;
        Price = price;
        ItemTexture = itemTexture;
    }



}
