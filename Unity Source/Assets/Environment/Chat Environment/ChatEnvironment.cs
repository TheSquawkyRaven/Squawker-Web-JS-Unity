using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatEnvironment : Environment
{

    [System.Serializable]
    public class ChatContainer : EnvironmentContainer
    {
        public SColor textColor = null;
        public List<SColor> nameColors = new List<SColor>();
        public float scrollTime = -1f;

        public bool useParticles = true;
        public float emissionRate = -1f;

        public bool mute = false;
        public float volume = -1f;
    }
    public ChatContainer Save => (ChatContainer)container;

    public AudioSource newChatAudioSource;
    [System.NonSerialized] private bool isMute = false;

    public List<RectTransform> textTransforms;
    public List<TextMeshProUGUI> texts;

    public float from;
    public float to;

    public List<Vector2> textPositions;
    public float resetYPosition;
    public List<Color> nameColors;
    public Color textColor = Color.white;

    public ParticleSystem particles;
    public ParticleSystem.MainModule mainModule;
    public ParticleSystem.EmissionModule emissionModule;

    public float scrollTime = 0.25f;
    private float timeCount = 0;
    private bool isMoving = false;

    public override void StartEnvironment()
    {
        environmentName = "chat";
        base.StartEnvironment();
        mainModule = particles.main;
        emissionModule = particles.emission;
        ManageSave(SN.@this.save.willieContainer);
        SN.@this.FILEReader.onContentChange += Write;
        SN.@this.updateAction += UpdateText;
    }
    public override void StopEnvironment()
    {
        base.StopEnvironment();
        SN.@this.FILEReader.onContentChange -= Write;
    }

    protected override void AddCommands()
    {
        base.AddCommands();
        mainCommand.Add("mute", Mute);
        mainCommand.Add("unmute", Unmute);
        mainCommand.Add("setvolume", SetSoundVolume);
        mainCommand.Add("volume", SetSoundVolume);

        mainCommand.Add("color", TextColor);
        mainCommand.Add("textcolor", TextColor);
        mainCommand.Add("colortext", TextColor);

        mainCommand.Add("namecolors", NameColors);
        mainCommand.Add("colorsname", NameColors);
        mainCommand.Add("colors", NameColors);

        mainCommand.Add("scroll", ScrollSpeed);
        mainCommand.Add("scrollspeed", ScrollSpeed);

        mainCommand.Add("particle", Particles);
        mainCommand.Add("particles", Particles);
        mainCommand.Add("useparticle", Particles);
        mainCommand.Add("useparticles", Particles);
        mainCommand.Add("startparticle", Particles);
        mainCommand.Add("startparticles", Particles);

        mainCommand.Add("noparticle", ParticlesFalse);
        mainCommand.Add("noparticles", ParticlesFalse);
        mainCommand.Add("stopparticle", ParticlesFalse);
        mainCommand.Add("stopparticles", ParticlesFalse);

        mainCommand.Add("particleemission", ParticleEmission);
        mainCommand.Add("particlesemission", ParticleEmission);
        mainCommand.Add("emission", ParticleEmission);
        mainCommand.Add("emissionrate", ParticleEmission);

    }
    private void Mute(FileReader.Content content)
    {
        isMute = true;
        Save.mute = true;
        if (newChatAudioSource.isPlaying)
        {
            newChatAudioSource.Stop();
        }
        SN.@this.popups.PutPopup($"Muted", SN.mutePopup);

    }
    private void Unmute(FileReader.Content content)
    {
        isMute = false;
        Save.mute = false;
        SN.@this.popups.PutPopup($"Unmuted", SN.unmutePopup);
    }

    private void SetSoundVolume(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            value = Mathf.Min(0, value);
            value = Mathf.Max(1, value);
            newChatAudioSource.volume = value;
            Save.volume = value;
            isMute = false;
            Save.mute = false;
            SN.@this.popups.PutPopup($"Volume set to {value}", SN.unmutePopup);
            return;
        }, "volume", 1f);
    }

    protected void TextColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            Save.textColor = color;
            textColor = color;
            SN.@this.popups.PutPopup($"Text color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "textcolor", "white");
    }
    protected void NameColors(FileReader.Content content)
    {
        ColorsHelper(content, nameColors, Save.nameColors, "nameColors", "name colors pool");
    }
    protected void ScrollSpeed(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            scrollTime = value;
            Save.scrollTime = value;
            SN.@this.popups.PutPopup($"", SN.optionsPopup);
            return;
        }, "scrollspeed", 0.25f);
    }
    protected void Particles(FileReader.Content content)
    {
        emissionModule.enabled = true;
        Save.useParticles = true;
        SN.@this.popups.PutPopup($"Started emitting particles", SN.optionsPopup);
    }
    protected void ParticlesFalse(FileReader.Content content)
    {
        emissionModule.enabled = false;
        Save.useParticles = false;
        SN.@this.popups.PutPopup($"Stopped emitting particles", SN.optionsPopup);
    }
    protected void ParticleEmission(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            emissionModule.rateOverTime = value;
            Save.emissionRate = value;
            SN.@this.popups.PutPopup($"Emitting particles at a rate of {value} per second", SN.optionsPopup);
            return;
        }, "emission", 10f);
    }

    private void UpdateText()
    {
        if (isMoving)
        {
            timeCount += Time.deltaTime;
            float lerp = Mathf.Lerp(from, to, timeCount / scrollTime);
            for (int i = 0; i < textTransforms.Count; i++)
            {
                Vector2 position = textPositions[i + 1];
                position.y += lerp;
                textTransforms[i].anchoredPosition = position;
            }
            if (timeCount > scrollTime)
            {
                Snap();
                isMoving = false;
            }
        }
    }
    private void Snap()
    {
        for (int i = 0; i < textTransforms.Count; i++)
        {
            textTransforms[i].anchoredPosition = textPositions[i];
        }
        RectTransform rectTransform = textTransforms[0];
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, resetYPosition);
        textTransforms.RemoveAt(0);
        textTransforms.Add(rectTransform);
        TextMeshProUGUI text = texts[0];
        texts.RemoveAt(0);
        texts.Add(text);
    }

    private void Write(FileReader.Content content)
    {
        if (!content.isCommand)
        {
            if (!isMute)
            {
                if (newChatAudioSource.isPlaying)
                {
                    newChatAudioSource.Stop();
                }
                newChatAudioSource.Play();
            }
            if (isMoving)
            {
                Snap();
            }
            int index = (int)(content.displayName.GetHashCode() / ((float)2147483647 / nameColors.Count));
            index = Mathf.Abs(index);
            string name = $"<color=#{ColorUtility.ToHtmlStringRGBA(nameColors[index])}>{content.displayName}</color>";
            texts[texts.Count - 1].SetText($"{name}: <color=#{ColorUtility.ToHtmlStringRGBA(textColor)}>{content.text}</color>");

            mainModule.startColor = new ParticleSystem.MinMaxGradient(nameColors[index]);

            timeCount = 0;
            isMoving = true;
        }
    }

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.textColor == null)
            {
                Save.textColor = Color.white;
            }
            textColor = Save.textColor;
            if (Save.nameColors == null)
            {
                Save.nameColors = new List<SColor>();
            }
            if (Save.nameColors.Count > 0)
            {
                nameColors.Clear();
                for (int i = 0; i < Save.nameColors.Count; i++)
                {
                    nameColors.Add(Save.nameColors[i]);
                }
            }
            if (Save.scrollTime > 0f)
            {
                scrollTime = Save.scrollTime;
            }
            emissionModule.enabled = Save.useParticles;
            if (Save.emissionRate > 0f)
            {
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(Save.emissionRate);
            }
            isMute = Save.mute;
            if (Save.volume >= 0f)
            {
                newChatAudioSource.volume = Save.volume;
            }
            
        }
    }

}
