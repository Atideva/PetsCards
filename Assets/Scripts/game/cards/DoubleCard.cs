
using EPOOutline;
using UnityEngine;

public class DoubleCard : MonoBehaviour
{

    public SpriteRenderer topImage;
    public SpriteRenderer botImage;
    public SpriteRenderer frame;
    public Outlinable topOutline;
    public Outlinable botOutline;
    public void SetOutlineColor(Color clr)
    {
        topOutline.OutlineParameters.Color = clr;
        botOutline.OutlineParameters.Color = clr;
    }
    public void EnableSprites()
    {
        topImage.enabled = true;
        botImage.enabled = true;
    }
    public void EnableOutline()
    {
        topOutline.enabled = true;
        botOutline.enabled = true;
    }
    public void DisableOutline()
    {
        topOutline.enabled = false;
        botOutline.enabled = false;
    }
    public void Init(Sprite sprite)
    {
        topImage.sprite = sprite;
        botImage.sprite = sprite;
        frame.enabled = true;
        Hide();
    }
    public void Show()
    {
        topImage.sortingOrder = 1;
        botImage.sortingOrder = 1;
        EnableSprites();
    }
    public void Hide()
    {
        topImage.sortingOrder = 0;
        botImage.sortingOrder = 0;
        topImage.enabled = false;
        botImage.enabled = false;
    }
}
