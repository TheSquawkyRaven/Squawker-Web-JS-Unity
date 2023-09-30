using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnnouncementEnvironment : Environment
{

    [System.Serializable]
    public class AnnouncementContainer : EnvironmentContainer
    {
        public SColor textColor = null;

        public bool animate = true;
        public bool haveX = true;
        public bool haveY = true;
        public bool haveZ = true;
    }
    public AnnouncementContainer Save => (AnnouncementContainer)container;

    public CameraAnimationTransition transition;
    public TextMeshPro announcementText;
    public ParticleSystem particles;
    public ParticleSystem.MainModule mainModule;


    public List<int> integers = new List<int>()
    {
        0, 1, 2
    };
    public bool animate = true;

    public override void StartEnvironment()
    {
        environmentName = "announce";
        base.StartEnvironment();
        mainModule = particles.main;
        announcementText.SetText(SN.@this.announcementMessage);
        ManageSave(SN.@this.save.announcementContainer);
    }

    protected override void AddCommands()
    {
        base.AddCommands();
        mainCommand.Add("animate", Animate);
        mainCommand.Add("flow", Animate);
        mainCommand.Add("play", Animate);

        mainCommand.Add("static", Static);
        mainCommand.Add("freeze", Static);
        mainCommand.Add("deanimate", Static);
        mainCommand.Add("unanimate", Static);
        mainCommand.Add("pause", Static);
        mainCommand.Add("stop", Static);

        mainCommand.Add("usex", AddX);
        mainCommand.Add("addx", AddX);
        mainCommand.Add("x", AddX);

        mainCommand.Add("usey", AddY);
        mainCommand.Add("addy", AddY);
        mainCommand.Add("y", AddY);

        mainCommand.Add("usez", AddZ);
        mainCommand.Add("addz", AddZ);
        mainCommand.Add("z", AddZ);

        mainCommand.Add("dropx", RemoveX);
        mainCommand.Add("removex", RemoveX);
        mainCommand.Add("stopx", RemoveX);

        mainCommand.Add("dropy", RemoveY);
        mainCommand.Add("removey", RemoveY);
        mainCommand.Add("stopy", RemoveY);

        mainCommand.Add("dropz", RemoveZ);
        mainCommand.Add("removez", RemoveZ);
        mainCommand.Add("stopz", RemoveZ);

        mainCommand.Add("color", TextColor);
        mainCommand.Add("textcolor", TextColor);
        mainCommand.Add("colortext", TextColor);

        mainCommand.Add("text", Announce);

    }

    protected virtual void Animate(FileReader.Content content)
    {
        Save.animate = true;
        Animate();
        SN.@this.popups.PutPopup("Animation active", SN.playPopup);
    }
    protected virtual void Static(FileReader.Content content)
    {
        Save.animate = false;
        Static();
        SN.@this.popups.PutPopup("Animation deactivated", SN.stopPopup);
    }
    protected virtual void Animate()
    {
        animate = true;
        AddX();
        AddY();
        AddZ();
    }
    protected virtual void Static()
    {
        animate = false;
    }


    protected virtual void AddX(FileReader.Content content)
    {
        Save.haveX = true;
        AddX();
        SN.@this.popups.PutPopup("X-axis animation active", SN.optionsPopup);
    }
    protected virtual void AddY(FileReader.Content content)
    {
        Save.haveY = true;
        AddY();
        SN.@this.popups.PutPopup("Y-axis animation active", SN.optionsPopup);
    }
    protected virtual void AddZ(FileReader.Content content)
    {
        Save.haveZ = true;
        AddZ();
        SN.@this.popups.PutPopup("Z-axis animation active", SN.optionsPopup);
    }
    protected virtual void RemoveX(FileReader.Content content)
    {
        Save.haveX = false;
        RemoveX();
        SN.@this.popups.PutPopup("X-axis animation deactivate", SN.optionsPopup);
    }
    protected virtual void RemoveY(FileReader.Content content)
    {
        Save.haveY = false;
        RemoveY();
        SN.@this.popups.PutPopup("Y-axis animation deactivate", SN.optionsPopup);
    }
    protected virtual void RemoveZ(FileReader.Content content)
    {
        Save.haveZ = false;
        RemoveZ();
        SN.@this.popups.PutPopup("Z-axis animation deactivate", SN.optionsPopup);
    }
    public void AddX()
    {
        if (integers.Contains(0))
        {
            return;
        }
        integers.Add(0);
    }
    public void AddY()
    {
        if (integers.Contains(1))
        {
            return;
        }
        integers.Add(1);
    }
    public void AddZ()
    {
        if (integers.Contains(2))
        {
            return;
        }
        integers.Add(2);
    }
    public void RemoveX()
    {
        if (integers.Contains(0))
        {
            integers.Remove(0);
        }
    }
    public void RemoveY()
    {
        if (integers.Contains(1))
        {
            integers.Remove(1);
        }
    }
    public void RemoveZ()
    {
        if (integers.Contains(2))
        {
            integers.Remove(2);
        }
    }

    protected override void Announce(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            SN.@this.announcementMessage = text;
            announcementText.SetText(text);
            SN.@this.popups.PutPopup("Announcement Updated", SN.announcePopup);
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /announce Hello World", SN.notAllowedPopup);
    }

    protected virtual void TextColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            Save.textColor = color;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(color);
            announcementText.color = color;
            SN.@this.popups.PutPopup($"Text & particles colors set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "textcolor", "red");
    }

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.textColor != null)
            {
                announcementText.color = Save.textColor;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(Save.textColor);
            }
            if (Save.animate)
                Animate();
            else
                Static();
            if (Save.haveX)
                AddX();
            else
                RemoveX();
            if (Save.haveY)
                AddY();
            else
                RemoveY();
            if (Save.haveZ)
                AddZ();
            else
                RemoveZ();
        }
    }

}
