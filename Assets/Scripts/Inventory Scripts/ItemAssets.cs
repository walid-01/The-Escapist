using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public Sprite redKeySprite;
    public Sprite greenKeySprite;
    public Sprite blueKeySprite;
    public Sprite shovelSprite;
    public Sprite axeSprite;
    public Sprite pickaxeSprite;
    public Sprite coal;
    public Sprite cable;
    public Sprite stick;
    public Sprite torchOn;
    public Sprite torchOff;
}
