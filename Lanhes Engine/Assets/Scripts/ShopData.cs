using System;
using System.Collections.Generic;

[Serializable]
public struct ShopData {
    //TODO: this may be a good idea to turn into a ScriptableObject
    public List<ItemCost> buyPrices;
    public List<ItemCost> sellPrices;
}

