using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    public List<CommonItem> CommonItems;
    public List<CraftableItem> CraftableItems;


    public Inventory(List<CommonItem> CommonItems, List<CraftableItem> CraftableItems)
    {
        this.CommonItems = CommonItems;
        this.CraftableItems = CraftableItems;
    }


    


}
