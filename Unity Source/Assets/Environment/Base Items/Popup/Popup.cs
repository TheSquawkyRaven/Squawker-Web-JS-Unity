using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{

    public RectTransform thisTransform;

    public Image containerBorderImage;
    public Image containerImage;

    public Image iconBackgroundImage;
    public Image iconImage;

    public RectTransform textTransform;
    public TextMeshProUGUI text;

    public float containerToTextWidth = 85f;
    public float additionalLength = 20;

    public float textLeftStretchWithIcon = 70f;
    public float textLeftStretchWithoutIcon = 30f;

    public float targetYPosition;
    public float lerpValue;

    private void Update()
    {
        if (thisTransform.anchoredPosition.y != targetYPosition)
        {
            thisTransform.anchoredPosition = Vector2.Lerp(thisTransform.anchoredPosition, new Vector2(thisTransform.anchoredPosition.x, targetYPosition), lerpValue);
        }
    }

    public void PostSetSize()
    {
        bool hasIcon = iconBackgroundImage.enabled;
        float containerToTextWidth = this.containerToTextWidth;
        float stretch = textLeftStretchWithIcon;
        if (!hasIcon)
        {
            containerToTextWidth = this.containerToTextWidth - (textLeftStretchWithIcon - textLeftStretchWithoutIcon);
            stretch = textLeftStretchWithoutIcon;
        }
        textTransform.offsetMin = new Vector2(stretch, textTransform.offsetMin.y);

        text.ForceMeshUpdate();
        Bounds textBounds = text.textBounds;
        float xSize = textBounds.size.x + containerToTextWidth + additionalLength;

        thisTransform.sizeDelta = new Vector2(xSize, thisTransform.sizeDelta.y);

    }

    public void SetData(Popups.Data data)
    {
        SetIcon(data.iconSprite, data.iconBackgroundColor, data.iconColor);
        SetColors(data.outlineColor, data.backgroundColor, data.textColor);
    }

    public void SetIcon(Sprite sprite, Color iconBackgroundColor, Color iconColor)
    {
        if (sprite == null)
        {
            iconBackgroundImage.enabled = false;
            iconImage.enabled = false;
            return;
        }
        if (sprite == SN.@this.popups.brushIcon)
        {
            Image brushOverlay = Instantiate(iconImage, iconImage.transform.parent).GetComponent<Image>();
            brushOverlay.sprite = SN.@this.popups.brushIcon;
            brushOverlay.color = Color.white;
            sprite = SN.@this.popups.brushColorIcon;
        }
        iconImage.sprite = sprite;
        iconBackgroundImage.color = iconBackgroundColor;
        iconImage.color = iconColor;
    }
    public void SetIcon(Sprite sprite)
    {
        SetIcon(sprite, Color.white, Color.black);
    }
    public void SetColors(Color containerBorderColor, Color containerColor, Color textColor)
    {
        containerBorderImage.color = containerBorderColor;
        containerImage.color = containerColor;
        text.color = textColor;
    }
    public void SetColors(Color backgroundColor, Color textColor)
    {
        SetColors(Color.white, backgroundColor, textColor);
    }

    public void SetText(string text)
    {
        this.text.SetText(text);
    }

    public void FinishPopup()
    {
        SN.@this.popups.EndPopup();
    }

}
