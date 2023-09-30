using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerEnvironment : Environment
{
    [System.Serializable]
    public class TimerContainer : EnvironmentContainer
    {
        public SColor color = null;
    }
    public TimerContainer Save => (TimerContainer)container;

    public TextMeshProUGUI timerText;
    public AudioSource timerEndAudioSource;

    private bool ended = false;
    private bool hasHour;

    private float timeCountdown;
    private bool isPaused = false;

    protected override void AddCommands()
    {
        base.AddCommands();
        mainCommand.Add("start", StartTimer);
        mainCommand.Add("resume", StartTimer);

        mainCommand.Add("pause", PauseTimer);

        mainCommand.Add("stop", StopTimer);
        mainCommand.Add("back", StopTimer);

        mainCommand.Add("reset", ResetTimer);

        mainCommand.Add("color", TextColor);
        mainCommand.Add("textcolor", TextColor);
        mainCommand.Add("colortext", TextColor);
        mainCommand.Add("timercolor", TextColor);
        mainCommand.Add("colortimer", TextColor);
        mainCommand.Add("timecolor", TextColor);
        mainCommand.Add("colortime", TextColor);
    }
    private void StartTimer(FileReader.Content content)
    {
        isPaused = false;
        SN.@this.popups.PutPopup($"Timer started", SN.playPopup);
    }
    private void PauseTimer(FileReader.Content content)
    {
        isPaused = true;
        SN.@this.popups.PutPopup($"Timer paused", SN.pausePopup);
    }
    private void StopTimer(FileReader.Content content)
    {
        timerEndAudioSource.Stop();
        isPaused = true;
        if (SN.@this.timerStopReturnEnvironment == null || SN.@this.timerStopReturnEnvironment == string.Empty)
        {
            SN.@this.timerStopReturnEnvironment = SN.@this.defaultEnvironment.environmentName;
        }
        SN.@this.ChangeEnvironment(SN.@this.timerStopReturnEnvironment);
        SN.@this.popups.PutPopup($"Timer stopped", SN.stopPopup);
    }
    private void ResetTimer(FileReader.Content content)
    {
        timerEndAudioSource.Stop();
        timeCountdown = SN.@this.timerSetTimeCountdown;
        isPaused = true;
        ended = false;
        hasHour = timeCountdown >= 3600f;
        SetTime();
        SN.@this.popups.PutPopup($"Timer reset", SN.timerPopup);
    }

    protected void TextColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            timerText.color = color;
            Save.color = color;
            SN.@this.popups.PutPopup($"Timer's color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "color", "green");
    }

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.color != null)
            {
                timerText.color = Save.color;
            }
        }
    }

    public override void StartEnvironment()
    {
        environmentName = "timer";
        base.StartEnvironment();
        ManageSave(SN.@this.save.timerContainer);
        SN.@this.updateAction += UpdateTime;

        timeCountdown = SN.@this.timerSetTimeCountdown;
        isPaused = SN.@this.timerStartPaused;
        hasHour = timeCountdown >= 3600f;
        timerEndAudioSource.Stop();

        SetTime(true);
    }

    public override void StopEnvironment()
    {
        base.StopEnvironment();
        SN.@this.updateAction -= UpdateTime;
    }

    private void UpdateTime()
    {
        if (!ended)
        {
            if (!isPaused)
            {
                timeCountdown -= Time.deltaTime;
                SetTime();
            }
        }
    }

    private void SetTime(bool isInit = false)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeCountdown);

        if (!isInit)
        {
            if (timeCountdown <= 0)
            {
                timeSpan = new System.TimeSpan();
                ended = true;
                timerEndAudioSource.Play();
            }
        }
        string text;
        bool roundUp = timeSpan.Milliseconds > 0;
        if (roundUp)
        {
            timeSpan = System.TimeSpan.FromSeconds(timeCountdown + 1);
        }
        if (hasHour)
        {
            text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
        timerText.SetText(text);
    }

}
