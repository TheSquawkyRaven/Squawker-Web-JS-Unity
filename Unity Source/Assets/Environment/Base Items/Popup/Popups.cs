using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popups : MonoBehaviour
{

    [System.Serializable]
    public class Data
    {
        public Color outlineColor = Color.white;
        public Color backgroundColor = Color.black;
        public Color textColor = Color.white;

        public Color iconBackgroundColor = Color.white;
        public Sprite iconSprite = null;
        public Color iconColor = Color.black;

        public Data()
        {

        }
        public Data(Sprite icon)
        {
            iconSprite = icon;
        }

        /// <summary>
        /// Set Colors
        /// </summary>
        /// <param name="mainColor">Icon</param>
        /// <param name="subColor">Text</param>
        public void SetColor(Color mainColor, Color subColor)
        {
            iconColor = mainColor;
            iconBackgroundColor = subColor;
            backgroundColor = mainColor;
            outlineColor = subColor;
            textColor = subColor;
        }
        /// <summary>
        /// Set Colors
        /// </summary>
        /// <param name="mainColor">Icon</param>
        /// <param name="subColor">Icon Background</param>
        /// <param name="textColor">Text</param>
        public void SetColor(Color mainColor, Color subColor, Color textColor)
        {
            iconColor = mainColor;
            iconBackgroundColor = subColor;
            backgroundColor = mainColor;
            outlineColor = subColor;
            this.textColor = textColor;
        }
        public void SetColorBW(Color iconColor, Color iconBackgroundColor)
        {
            this.iconColor = iconColor;
            this.iconBackgroundColor = iconBackgroundColor;
            outlineColor = iconColor;
            backgroundColor = Color.black;
            textColor = Color.white;
        }
    }

    public static Data defaultData = new Data();

    public Transform popupCanvasTransform;
    public Vector3 defaultEnterPopupPosition;

    public float yDifference;

    public GameObject popupPrefab;
    public Queue<Popup> popupQueue = new Queue<Popup>();

    public Sprite loadIcon;
    public Sprite excludeIcon;
    public Sprite includeIcon;
    public Sprite randomIcon;
    public Sprite warningIcon;
    public Sprite infoIcon;
    public Sprite notAllowedIcon;
    public Sprite brushColorIcon;
    public Sprite brushIcon;
    public Sprite imageIcon;
    public Sprite envSwitchIcon;
    public Sprite timerIcon;
    public Sprite textIcon;
    public Sprite announceIcon;
    public Sprite muteIcon;
    public Sprite unmuteIcon;
    public Sprite catIcon;
    public Sprite optionsIcon;
    public Sprite playIcon;
    public Sprite pauseIcon;
    public Sprite stopIcon;
    public Sprite remapIcon;
    public Sprite bloomIcon;
    public Sprite adminIcon;
    public Sprite connectIcon;

    [ContextMenu("Put Popup")]
    public void PutPopup()
    {
        PutPopup("UI Placed", null);
    }

    public void PutPopup(string text, Data data)
    {
        if (data == null)
        {
            data = defaultData;
        }
        Popup popup = Instantiate(popupPrefab, popupCanvasTransform).GetComponent<Popup>();
        popup.thisTransform.anchoredPosition = defaultEnterPopupPosition;
        popup.SetText(text);
        popup.SetData(data);
        popup.targetYPosition = defaultEnterPopupPosition.y;
        popup.PostSetSize();
        popupQueue.Enqueue(popup);
        PushAllPopups();
    }

    public void PushAllPopups()
    {
        int index = popupQueue.Count - 1;
        foreach (Popup popup in popupQueue)
        {
            popup.targetYPosition = defaultEnterPopupPosition.y + yDifference * index;

            index--;
        }
    }

    public void EndPopup()
    {
        Popup popup = popupQueue.Dequeue();
        Destroy(popup.gameObject);
    }

}
