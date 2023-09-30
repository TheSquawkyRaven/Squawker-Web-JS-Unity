using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DVDEnvironment : Environment
{

    [System.Serializable]
    public class DVDContainer : EnvironmentContainer
    {
        public SColor nameColor = null;
        public SColor textColor = null;
        public float speed = -1;
        public List<SColor> bounceNameColors = new List<SColor>();
        public List<SColor> bounceTextColors = new List<SColor>();
        public bool bounceChange = true;
    }

    public DVDContainer Save => (DVDContainer)container;

    public Transform dvdTransform;
    public TextMeshPro nameText;
    public TextMeshPro textText;

    public Vector3 currentDirection;
    public float speed;
    public List<Color> bounceNameColors = new List<Color>();
    public List<Color> bounceTextColors = new List<Color>();
    private int bounceNameIndex = 0;
    private int bounceTextIndex = 0;
    public bool bounceChange = true;

    protected override void AddCommands()
    {
        base.AddCommands();
        //Make sure all is lower case
        mainCommand.Add("resetindex", ResetIndex);

        mainCommand.Add("namecolor", NameColor);
        mainCommand.Add("colorname", NameColor);

        mainCommand.Add("textcolor", TextColor);
        mainCommand.Add("colortext", TextColor);
        mainCommand.Add("color", TextColor);

        mainCommand.Add("namecolors", NameColors);
        mainCommand.Add("colorsname", NameColors);

        mainCommand.Add("textcolors", TextColors);
        mainCommand.Add("colorstext", TextColors);

        mainCommand.Add("colors", BothColors);
        mainCommand.Add("colorsall", BothColors);
        mainCommand.Add("allcolors", BothColors);

        mainCommand.Add("speed", Speed);

        mainCommand.Add("bouncechange", BounceChange);
        mainCommand.Add("bouncedontchange", BounceChangeFalse);

        mainCommand.Add("clear", Clear);
        mainCommand.Add("clr", Clear);
    }
    protected virtual void ResetIndex(FileReader.Content content)
    {
        bounceNameIndex = 0;
        bounceTextIndex = 0;
        SN.@this.popups.PutPopup("Index reset", SN.infoPopup);
    }
    protected void NameColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            nameText.color = color;
            Save.nameColor = color;
            SN.@this.popups.PutPopup($"Color of name set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "namecolor", "orange");
    }
    protected void TextColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            textText.color = color;
            Save.textColor = color;
            SN.@this.popups.PutPopup($"Color of text set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "textcolor", "blue");
    }
    protected void NameColors(FileReader.Content content)
    {
        ColorsHelper(content, bounceNameColors, Save.bounceNameColors, "namecolors", "name colors pool");
    }
    protected void TextColors(FileReader.Content content)
    {
        ColorsHelper(content, bounceTextColors, Save.bounceTextColors, "textcolors", "text colors pool");
    }
    protected void BothColors(FileReader.Content content)
    {
        ColorsHelper(content, bounceNameColors, Save.bounceNameColors, "colors", "name colors pool", false);
        ColorsHelper(content, bounceTextColors, Save.bounceTextColors, "colors", "text and name colors pool");
    }

    protected void Speed(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            speed = Mathf.Max(value, 99);
            Save.speed = Mathf.Max(value, 99);
            SN.@this.popups.PutPopup($"Text movement speed set to {speed}", SN.optionsPopup);
            return;
        }, "speed", 2f);
    }
    protected void BounceChange(FileReader.Content content)
    {
        bounceChange = true;
        Save.bounceChange = true;
        SN.@this.popups.PutPopup($"Text will now change color on each bounce", SN.optionsPopup);
    }
    protected void BounceChangeFalse(FileReader.Content content)
    {
        bounceChange = false;
        Save.bounceChange = false;
        SN.@this.popups.PutPopup($"Text will stay the same color", SN.optionsPopup);
    }

    protected void Clear(FileReader.Content content)
    {
        nameText.SetText(string.Empty);
        textText.SetText(string.Empty);
        SN.@this.popups.PutPopup($"Cleared texts", SN.infoPopup);
    }

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.nameColor != null)
            {
                nameText.color = Save.nameColor;
            }
            if (Save.textColor != null)
            {
                textText.color = Save.textColor;
            }
            if (Save.speed > 0)
            {
                speed = Save.speed;
            }
            if (Save.bounceNameColors.Count > 0)
            {
                bounceNameColors.Clear();
                for (int i = 0; i < Save.bounceNameColors.Count; i++)
                {
                    bounceNameColors.Add(Save.bounceNameColors[i]);
                }
            }
            if (Save.bounceTextColors.Count > 0)
            {
                bounceTextColors.Clear();
                for (int i = 0; i < Save.bounceTextColors.Count; i++)
                {
                    bounceTextColors.Add(Save.bounceTextColors[i]);
                }
            }
            bounceChange = Save.bounceChange;
        }
    }

    public override void StartEnvironment()
    {
        environmentName = "dvd";
        base.StartEnvironment();
        ManageSave(SN.@this.save.dvdContainer);
        currentDirection = Random.insideUnitCircle.normalized;
        SN.@this.FILEReader.onContentChange += Write;
    }
    public override void StopEnvironment()
    {
        base.StopEnvironment();
        SN.@this.FILEReader.onContentChange -= Write;
    }

    private void Write(FileReader.Content content)
    {
        if (!content.isCommand)
        {
            nameText.SetText(content.name);
            textText.SetText(content.text);
        }
    }


    private void FixedUpdate()
    {
        dvdTransform.localPosition += currentDirection * speed * Time.deltaTime * 10f;
    }

    public void Bounce()
    {
        if (bounceChange)
        {
            bounceNameIndex++;
            bounceTextIndex++;
            if (bounceNameIndex >= bounceNameColors.Count)
            {
                bounceNameIndex = 0;
            }
            if (bounceTextIndex >= bounceTextColors.Count)
            {
                bounceTextIndex = 0;
            }
            nameText.color = bounceNameColors[bounceNameIndex];
            textText.color = bounceTextColors[bounceTextIndex];
        }
    }

}
