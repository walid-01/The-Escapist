using System;
using UnityEngine;

[Serializable]
public class Item
{
    public bool isActive = false;

    public enum ItemType
    {
        Default,
        RedKey,
        GreenKey,
        BlueKey,
        Shovel,
        Axe,
        Pickaxe,
        Coal,
        Cable,
        Stick,
        TorchOn,
        TorchOff
    }
    public ItemType itemType;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.RedKey:   return ItemAssets.Instance.redKeySprite;
            case ItemType.GreenKey: return ItemAssets.Instance.greenKeySprite;
            case ItemType.BlueKey:  return ItemAssets.Instance.blueKeySprite;
            case ItemType.Shovel:   return ItemAssets.Instance.shovelSprite;
            case ItemType.Axe:      return ItemAssets.Instance.axeSprite;
            case ItemType.Pickaxe:  return ItemAssets.Instance.pickaxeSprite;
            case ItemType.Coal:  return ItemAssets.Instance.coal;
            case ItemType.Cable:  return ItemAssets.Instance.cable;
            case ItemType.Stick:  return ItemAssets.Instance.stick;
            case ItemType.TorchOn:  return ItemAssets.Instance.torchOn;
            case ItemType.TorchOff:  return ItemAssets.Instance.torchOff;
        }
    }
}
