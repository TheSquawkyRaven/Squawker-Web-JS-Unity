using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CatEnvironment : Environment
{

    [System.Serializable]
    public class CatContainer : EnvironmentContainer
    {
        public SColor catNameColor = null;
        public SColor textColor = null;
        public List<SColor> textColors = new List<SColor>();

        public bool useSplits = false;
        public int splits = -1;

        public bool usePulseText = false;
        public bool pulseOverrideText = false;
        public string pulseText = null;

        public float textMovementSpeed = -1;

        public bool useFullText = true;

        public bool usePoppingText = true;
        public float poppingTextSpeed = -1;

        public string catName = null;
        public float catMouthOpenTime = -1;

        public float destroyTextTime = -1;

        public float shootingPulse = -1;
    }
    public CatContainer Save => (CatContainer)container;

    public TextMeshPro catNameText;
    public Transform spawnContainer;
    public Transform spawnPosition;

    public SpriteRenderer catSR;
    public Sprite catOpened;
    public Sprite catClosed;

    public GameObject textPrefab;
    public Queue<string> textQueue = new Queue<string>();

    public float shootingPulse = 0.1f;

    [Tooltip("Must be less than pulse")]
    public float catMouthOpenTime = 0.1f;

    public float randomizedAngleMin = 0;
    public float randomizedAngleMax = 45;

    public float textMovementSpeedMin;
    public float textMovementSpeedMax;

    public float rotationSpeedMin;
    public float rotationSpeedMax;

    public float radiusMin = 0.1f;
    public float radiusMax = 1f;

    public bool useSplits = false;
    public int splits = 5;

    public Color textColor;
    public List<Color> textColors;

    public float timeToDestroyText = 10f;

    public bool useFullText = false;
    public bool pulseOverrideText = false;
    public string pulseText;
    public bool usePulseText = false;

    public float poppingTextSpeed = 0.05f;
    public bool usePoppingText = false;

    protected override void AddCommands()
    {
        base.AddCommands();
        //Make sure lowercase
        mainCommand.Add("setpulse", SetPulse);
        mainCommand.Add("pulse", SetPulse);

        mainCommand.Add("startpulse", StartPulse);
        mainCommand.Add("resumepulse", StartPulse);

        mainCommand.Add("stoppulse", StopPulse);
        mainCommand.Add("pausepulse", StopPulse);

        mainCommand.Add("pulseoverride", PulseOverride);
        mainCommand.Add("override", PulseOverride);

        mainCommand.Add("name", SetName);
        mainCommand.Add("text", SetName);

        mainCommand.Add("textcolor", TextColor);
        mainCommand.Add("colortext", TextColor);

        mainCommand.Add("namecolor", NameColor);
        mainCommand.Add("colorname", NameColor);
        mainCommand.Add("color", NameColor);

        mainCommand.Add("textcolors", TextColors);
        mainCommand.Add("colorstext", TextColors);
        mainCommand.Add("colors", TextColors);

        mainCommand.Add("fulltext", FullText);
        mainCommand.Add("usefulltext", FullText);

        mainCommand.Add("nofulltext", FullTextFalse);
        mainCommand.Add("stopfulltext", FullTextFalse);

        mainCommand.Add("splits", Splits);
        mainCommand.Add("usesplits", Splits);

        mainCommand.Add("nosplits", SplitsFalse);
        mainCommand.Add("stopsplits", SplitsFalse);

        mainCommand.Add("pop", PoppingText);
        mainCommand.Add("usepop", PoppingText);

        mainCommand.Add("nopop", PoppingTextFalse);
        mainCommand.Add("stoppop", PoppingTextFalse);


        mainCommand.Add("shootpulse", ShootPulse);
        mainCommand.Add("mouthopen", MouthOpen);
        mainCommand.Add("speed", TextSpeed);
        mainCommand.Add("destroyin", DestroyTextIn);
        mainCommand.Add("popspeed", PoppingTextSpeed);

    }

    protected virtual void SetPulse(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            bool hasOverrideText = false;
            if (content.commands.Length >= 3)
            {
                switch (content.commands[content.commands.Length - 1])
                {
                    case "true":
                    case "yes":
                        pulseOverrideText = true;
                        Save.pulseOverrideText = true;
                        hasOverrideText = true;
                        SN.@this.popups.PutPopup("Overriding chat (chat text won't be used)", SN.catPopup);
                        break;
                    case "false":
                    case "no":
                        pulseOverrideText = false;
                        Save.pulseOverrideText = false;
                        hasOverrideText = true;
                        break;
                }
            }
            int max = hasOverrideText ? content.commands.Length - 1 : content.commands.Length;
            string text = string.Empty;
            for (int i = 1; i < max; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < max ? " " : string.Empty)}";
            }
            pulseText = text;
            usePulseText = true;
            SN.@this.popups.PutPopup($"{text} set as pulse. Pulse activated", SN.catPopup);
            return;
        }

        SN.@this.popups.PutPopup("Syntax is /pulse ---------- on", SN.notAllowedPopup);
    }
    protected virtual void PulseOverride(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            switch (content.commands[1])
            {
                case "true":
                case "yes":
                    SN.@this.popups.PutPopup("Overriding chat (chat text won't be used)", SN.catPopup);
                    return;
                case "false":
                case "no":
                    SN.@this.popups.PutPopup("Chat intercept allowed (chat text will be used)", SN.catPopup);
                    return;
            }
        }
        SN.@this.popups.PutPopup("Syntax is /override on", SN.notAllowedPopup);
    }
    protected virtual void StartPulse(FileReader.Content content)
    {
        usePulseText = true;
        SN.@this.popups.PutPopup("Pulse activated", SN.catPopup);
    }
    protected virtual void StopPulse(FileReader.Content content)
    {
        usePulseText = false;
        SN.@this.popups.PutPopup("Pulse deactivated", SN.catPopup);
    }

    protected virtual void SetName(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            catNameText.SetText(text);
            Save.catName = text;
            SN.@this.popups.PutPopup($"Hello {text}, meow uwu", SN.catPopup);
            return;
        }

        SN.@this.popups.PutPopup("Syntax is /name Noodles", SN.catPopup);
    }
    protected void TextColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            textColor = color;
            Save.textColor = color;
            SN.@this.popups.PutPopup($"Text color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "textcolor", "white");
    }
    protected void NameColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            catNameText.color = color;
            Save.catNameColor = color;
            SN.@this.popups.PutPopup($"Cat name color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "namecolor", "cyan");
    }

    protected void TextColors(FileReader.Content content)
    {
        ColorsHelper(content, textColors, Save.textColors, "textcolors", "Text Colors");
    }

    protected string GetCatName()
    {
        return catNameText.text;
    }
    protected string GetCatNameS()
    {
        string name = GetCatName();
        bool haveSAlready = name[name.Length - 1] == 's' || name[name.Length - 1] == 'S';
        name += haveSAlready ? "'" : "'s";
        return name;
    }
    protected void FullText(FileReader.Content content)
    {
        useFullText = true;
        Save.useFullText = true;
        SN.@this.popups.PutPopup($"{GetCatName()}'ll spit out text word by word. Bleh", SN.catPopup);
    }
    protected void FullTextFalse(FileReader.Content content)
    {
        useFullText = false;
        Save.useFullText = false;
        if (usePoppingText)
        {
            SN.@this.popups.PutPopup($"{GetCatName()}'ll gargle up texts until the screen fills. *burps", SN.catPopup);
        }
        else
        {
            SN.@this.popups.PutPopup($"{GetCatName()}'ll spit out lazers. pew pew", SN.catPopup);
        }
    }
    protected void PoppingText(FileReader.Content content)
    {
        usePoppingText = true;
        Save.usePoppingText = true;
        SN.@this.popups.PutPopup($"{GetCatNameS()} texts will be popping by characters. *hisses", SN.catPopup);
    }
    protected void PoppingTextFalse(FileReader.Content content)
    {
        usePoppingText = false;
        Save.usePoppingText = false;
        SN.@this.popups.PutPopup($"{GetCatName()}'ll spam the entire words/texts. *purrs", SN.catPopup);
    }

    protected void ShootPulse(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            shootingPulse = value;
            Save.shootingPulse = value;
            SN.@this.popups.PutPopup($"{GetCatName()}'ll be shooting text every {value} seconds. Mew", SN.catPopup);
            return;
        }, "shootpulse", 0.25f);
    }
    protected void MouthOpen(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            catMouthOpenTime = value;
            Save.catMouthOpenTime = value;
            SN.@this.popups.PutPopup($"{GetCatNameS()} mouth wil be hanging open for {value} seconds in a pop. *sneezes", SN.catPopup);
            return;
        }, "mouthopen", 0.1f);
    }
    protected void TextSpeed(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            textMovementSpeedMax = textMovementSpeedMin = value;
            Save.textMovementSpeed = value;
            SN.@this.popups.PutPopup($"Text will move at a speed of {value}. *snarls", SN.catPopup);
            return;
        }, "speed", 0.75f);
    }
    protected void DestroyTextIn(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            timeToDestroyText = value;
            Save.destroyTextTime = value;
            SN.@this.popups.PutPopup($"Text will be destroyed in {value} seconds after appearing. owo", SN.catPopup);
            return;
        }, "destroyin", 15f);
    }
    protected void PoppingTextSpeed(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            poppingTextSpeed = value;
            Save.poppingTextSpeed = value;
            SN.@this.popups.PutPopup($"{GetCatName()} prepares to do math. Popping letters at a rate of {1 / value} per second. Meow", SN.catPopup);
            return;
        }, "popspeed", 0.05f);
    }
    protected void SplitsFalse(FileReader.Content content)
    {
        useSplits = false;
        Save.useSplits = false;
        SN.@this.popups.PutPopup($"{GetCatName()}'ll pop text in random directions. uwu", SN.catPopup);
    }
    protected void Splits(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            bool isInt = int.TryParse(content.commands[1], out int value);
            if (isInt)
            {
                splits = Mathf.Max(1, value);
                Save.splits = value;
                string angles = string.Empty;
                for (int i = 1; i <= splits; i++)
                {
                    float zRotation = (1 * ((randomizedAngleMax - randomizedAngleMin) / splits)) + randomizedAngleMin;
                    angles += i == splits ? $"{zRotation}" : $"{zRotation}, ";
                }
                SN.@this.popups.PutPopup($"{GetCatName()} cheats in maths. Targeting at angles: {angles}. *Puts on sunglasses", SN.catPopup);
            }
        }
        useSplits = true;
        Save.useSplits = true;
        SN.@this.popups.PutPopup($"{GetCatName()}'ll pop text in oddly consistent sequential direction. :P", SN.catPopup);

    }

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.catNameColor != null)
            {
                catNameText.color = Save.catNameColor;
            }
            if (Save.textColor != null)
            {
                textColor = Save.textColor;
            }
            if (Save.textColors.Count > 0)
            {
                textColors.Clear();
                for (int i = 0; i < Save.textColors.Count; i++)
                {
                    textColors.Add(Save.textColors[i]);
                }
            }
            useSplits = Save.useSplits;
            if (Save.splits > 0)
            {
                splits = Save.splits;
            }
            usePulseText = Save.usePulseText;
            pulseOverrideText = Save.pulseOverrideText;
            if (Save.pulseText != null)
            {
                pulseText = Save.pulseText;
            }
            if (Save.textMovementSpeed > 0)
            {
                textMovementSpeedMax = textMovementSpeedMin = Save.textMovementSpeed;
            }
            useFullText = Save.useFullText;
            usePoppingText = Save.usePoppingText;
            if (Save.poppingTextSpeed > 0)
            {
                poppingTextSpeed = Save.poppingTextSpeed;
            }
            if (Save.catName != null)
            {
                catNameText.SetText(Save.catName);
            }
            if (Save.catMouthOpenTime > 0)
            {
                catMouthOpenTime = Save.catMouthOpenTime;
            }
            if (Save.destroyTextTime > 0)
            {
                timeToDestroyText = Save.destroyTextTime;
            }
            if (Save.shootingPulse > 0)
            {
                shootingPulse = Save.shootingPulse;
            }

        }
    }

    public override void StartEnvironment()
    {
        environmentName = "cat";
        base.StartEnvironment();
        ManageSave(SN.@this.save.catContainer);
        SN.@this.FILEReader.onContentChange += Write;
        SN.@this.updateAction += UpdateAction;
    }

    public override void StopEnvironment()
    {
        base.StopEnvironment();
        SN.@this.FILEReader.onContentChange -= Write;
        SN.@this.updateAction -= UpdateAction;
    }


    private void Write(FileReader.Content content)
    {
        if (!content.isCommand)
        {
            if (useFullText)
            {
                textQueue.Enqueue(content.text);
            }
            else
            {
                string[] splitString = content.text.Split(' ', '\n', '\t');
                for (int i = 0; i < splitString.Length; i++)
                {
                    textQueue.Enqueue(splitString[i]);
                }
            }
        }
    }

    private float timeCount = 0;
    private int colorsIndex = 0;
    private int splitIndex = 0;
    private bool coroutineRunning = false;
    private void UpdateAction()
    {
        timeCount += Time.deltaTime;
        if (!coroutineRunning)
        {
            if (timeCount > shootingPulse)
            {
                timeCount = 0;
                string text;
                if (usePulseText)
                {
                    if (pulseOverrideText)
                    {
                        text = pulseText;
                    }
                    else
                    {
                        if (textQueue.Count > 0)
                        {
                            text = textQueue.Dequeue();
                        }
                        else
                        {
                            text = pulseText;
                        }
                    }
                }
                else
                {
                    if (textQueue.Count > 0)
                    {
                        text = textQueue.Dequeue();
                    }
                    else
                    {
                        return;
                    }
                }
                float zRotation;
                if (useSplits)
                {
                    splitIndex++;
                    if (splitIndex > splits)
                    {
                        splitIndex = 1;
                    }
                    zRotation = (splitIndex * ((randomizedAngleMax - randomizedAngleMin) / splits)) + randomizedAngleMin;
                }
                else
                {
                    zRotation = Random.Range(randomizedAngleMin, randomizedAngleMax);
                }
                Rotater rotater = Instantiate(textPrefab, spawnPosition.position, Quaternion.Euler(0, 0/*Random.Range(-randomizedAngleMax, -randomizedAngleMin)*/, zRotation), spawnContainer).GetComponent<Rotater>();
                rotater.container = new Rotater.Container(
                    Random.Range(textMovementSpeedMin, textMovementSpeedMax),
                    Random.Range(radiusMin, radiusMax),
                    //Random.Range(radiusMin, radiusMax),
                    Random.Range(radiusMin, radiusMax),
                    Random.Range(rotationSpeedMin, rotationSpeedMax)
                );
                rotater.text = text;
                rotater.usePoppingText = usePoppingText;
                rotater.poppingTextSpeed = poppingTextSpeed;
                //rotater.textText.SetText(text);
                rotater.timeToDestroy = timeToDestroyText;
                colorsIndex++;
                if (colorsIndex >= textColors.Count)
                {
                    colorsIndex = 0;
                }
                rotater.textText.color = textColors.Count == 0 ? textColor : textColors[colorsIndex];
                StartCoroutine(OpenCloseCat(text.Length));
            }
        }
    }

    private IEnumerator OpenCloseCat(int textLength)
    {
        coroutineRunning = true;
        catSR.sprite = catOpened;
        if (usePoppingText)
        {
            yield return new WaitForSeconds(textLength * poppingTextSpeed);
            timeCount = 0;
        }
        else
        {
            yield return new WaitForSeconds(catMouthOpenTime < shootingPulse ? catMouthOpenTime : shootingPulse / 2f);
        }
        catSR.sprite = catClosed;
        coroutineRunning = false;
    }

}
