using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Speech.Synthesis;

[Serializable]
public class SColor
{
    public float r;
    public float g;
    public float b;
    public float a;
    public SColor(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
    public static implicit operator Color(SColor scolor)
    {
        return new Color(scolor.r, scolor.g, scolor.b, scolor.a);
    }
    public static implicit operator SColor(Color color)
    {
        return new SColor(color.r, color.g, color.b, color.a);
    }
}

public class Environment : MonoBehaviour, ICommand
{

    [Serializable]
    public class EnvironmentContainer
    {
        public string backgroundUrl = null;
        public SColor backgroundColor = null;

        public bool bloomEnabled = true;
        public float bloomThreshold = -1f;
        public float bloomIntensity = -1f;
        public SColor bloomColor = null;
    }

    [NonSerialized] public EnvironmentContainer container;
    public Image baseBackgroundImage;
    public Image backgroundImage;

    [HideInInspector] public bool environmentRunning = false;
    protected Dictionary<string, Action<FileReader.Content>> mainCommand;

    public void ExecuteCommand(FileReader.Content content)
    {
        if (mainCommand.Count > 0)
        {
            bool commandFound = false;
            foreach (KeyValuePair<string, Action<FileReader.Content>> command in mainCommand)
            {
                if (command.Key == content.commands[0])
                {
                    command.Value.Invoke(content);
                    commandFound = true;
                    break;
                }
            }
            if (!commandFound)
            {
                content.errors = "Command Not Found";
            }
        }
    }


    public static void Popup(string text, Popups.Data popupData)
    {
        SN.@this.popups.PutPopup(text, popupData);
    }

    #region Initialization
    /// <summary>
    /// Make sure all is lower case
    /// </summary>
    protected virtual void AddCommands()
    {
        mainCommand = new Dictionary<string, Action<FileReader.Content>>()
        {
            //Make sure all is lower case
            //{ "clear", Clear },
            { "setbackground", SetBackground },
            { "backgroundset", SetBackground },
            { "background", SetBackground },
            { "bg", SetBackground },

            { "removebackground", RemoveBackground },
            { "backgroundremove", RemoveBackground },
            { "rebackground", RemoveBackground },
            { "backgroundre", RemoveBackground },
            { "removebg", RemoveBackground },
            { "bgremove", RemoveBackground },
            { "rebg", RemoveBackground },
            { "bgre", RemoveBackground },

            { "setpp", SetPP },
            { "ppset", SetPP },
            { "pp", SetPP },

            { "settimer", SetTimer },
            { "timer", SetTimer },
            { "time", SetTimer },

            { "announce", Announce },
            { "anc", Announce },
            { "ann", Announce },

            { "loadppl", List },

            { "attendancegetclear", AttendanceGetClear },
            { "attgetclear", AttendanceGetClear },
            { "attendancegetclr", AttendanceGetClear },
            { "attgetclr", AttendanceGetClear },
            { "agc", AttendanceGetClear },

            { "getatt", Attendance },
            { "attget", Attendance },
            { "att", Attendance },
            { "attendance", Attendance },
            { "attendanceget", Attendance },
            { "getattendance", Attendance },

            { "attendanceclear", ClearAttendance },
            { "clearattendance", ClearAttendance },
            { "attclear", ClearAttendance },
            { "clearatt", ClearAttendance },
            { "attendanceclr", ClearAttendance },
            { "clrattendance", ClearAttendance },
            { "attclr", ClearAttendance },
            { "clratt", ClearAttendance },

            { "pplclear", ClearPpl },
            { "pplclr", ClearPpl },
            { "clearppl", ClearPpl },
            { "clrppl", ClearPpl },

            { "clearpplatt", ClearBoth },
            { "clearpplattendance", ClearBoth },
            { "pplattendanceclear", ClearBoth },
            { "clrpplatt", ClearBoth },
            { "clrpplattendance", ClearBoth },
            { "pplattendanceclr", ClearBoth },
            { "allclear", ClearBoth },
            { "clearall", ClearBoth },
            { "allclr", ClearBoth },
            { "clrall", ClearBoth },
            { "bothclear", ClearBoth },
            { "clearboth", ClearBoth },
            { "bothclr", ClearBoth },
            { "clrboth", ClearBoth },

            { "randnum", RandomNumber },
            { "randomnum", RandomNumber },
            { "randnumber", RandomNumber },
            { "randomnumber", RandomNumber },
            { "numrand", RandomNumber },
            { "numrandom", RandomNumber },
            { "numberrand", RandomNumber },
            { "numberrandom", RandomNumber },
            { "rn", RandomNumber },
            { "nr", RandomNumber },

            { "randlist", RandomInList },
            { "randomlist", RandomInList },
            { "listrand", RandomInList },
            { "listrandom", RandomInList },
            { "randli", RandomInList },
            { "randomli", RandomInList },
            { "lirand", RandomInList },
            { "lirandom", RandomInList },
            { "rl", RandomInList },
            { "lr", RandomInList },
            { "rli", RandomInList },
            { "lir", RandomInList },

            { "randomshufflelist", RandomList },
            { "randshufflelist", RandomList },
            { "randomlistshuffle", RandomList },
            { "randlistshuffle", RandomList },
            { "randshuffle", RandomList },
            { "randomshuffle", RandomList },
            { "shufflerand", RandomList },
            { "shufflerandom", RandomList },
            { "rls", RandomList },
            { "rsl", RandomList },
            { "lrs", RandomList },
            { "lsr", RandomList },
            { "srl", RandomList },
            { "slr", RandomList },
            { "rs", RandomList },
            { "sr", RandomList },



            { "randppl", RandomPpl },
            { "randomppl", RandomPpl },
            { "pplrand", RandomPpl },
            { "pplrandom", RandomPpl },
            { "rp", RandomPpl },
            { "pr", RandomPpl },

            { "randpplex", RandomPplExclude },
            { "randompplex", RandomPplExclude },
            { "randpplexclude", RandomPplExclude },
            { "randompplexclude", RandomPplExclude },
            { "randexppl", RandomPplExclude },
            { "randomexppl", RandomPplExclude },
            { "randexcludeppl", RandomPplExclude },
            { "randomexcludeppl", RandomPplExclude },
            { "exrandppl", RandomPplExclude },
            { "excluderandppl", RandomPplExclude },
            { "exrandomppl", RandomPplExclude },
            { "excluderandomppl", RandomPplExclude },
            { "rpex", RandomPplExclude },
            { "rpe", RandomPplExclude },
            { "rpx", RandomPplExclude },
            { "rexp", RandomPplExclude },
            { "rep", RandomPplExclude },
            { "rxp", RandomPplExclude },
            { "exrp", RandomPplExclude },
            { "erp", RandomPplExclude },
            { "xrp", RandomPplExclude },
            { "expr", RandomPplExclude },
            { "epr", RandomPplExclude },
            { "xpr", RandomPplExclude },
            { "prex", RandomPplExclude },
            { "pre", RandomPplExclude },
            { "prx", RandomPplExclude },
            { "pexr", RandomPplExclude },
            { "per", RandomPplExclude },
            { "pxr", RandomPplExclude },


            { "rclear", RandomClear },
            { "rclr", RandomClear },
            { "randomclear", RandomClear },
            { "randomclr", RandomClear },
            { "randclear", RandomClear },
            { "randclr", RandomClear },
            { "clearr", RandomClear },
            { "clrr", RandomClear },
            { "clearrandom", RandomClear },
            { "clrrandom", RandomClear },
            { "clearrand", RandomClear },
            { "clrrand", RandomClear },



            { "experm", ExcludePermanent },
            { "permex", ExcludePermanent },
            { "excludepermanent", ExcludePermanent },
            { "permanentexclude", ExcludePermanent },

            { "inperm", IncludePermanent },
            { "permin", IncludePermanent },
            { "includepermanent", IncludePermanent },
            { "permanentinclude", IncludePermanent },

            { "ex", Exclude },
            { "exclude", Exclude },

            { "in", Include },
            { "include", Include },

            { "inlist", ShowInclusions },
            { "inl", ShowInclusions },
            { "inclusions", ShowInclusions },

            { "exlist", ShowExclusions },
            { "exl", ShowExclusions },
            { "exclusions", ShowExclusions },

            { "exre", ExcludeRemove },
            { "reex", ExcludeRemove },
            { "excludere", ExcludeRemove },
            { "reexclude", ExcludeRemove },
            { "excluderemove", ExcludeRemove },
            { "removeexclude", ExcludeRemove },

            { "inre", IncludeRemove },
            { "rein", IncludeRemove },
            { "includere", IncludeRemove },
            { "reinclude", IncludeRemove },
            { "includeremove", IncludeRemove },
            { "removeinclude", IncludeRemove },

            { "exclear", ExclusionClear },
            { "clearex", ExclusionClear },
            { "exclr", ExclusionClear },
            { "clrex", ExclusionClear },
            { "excludeclear", ExclusionClear },
            { "clearexclude", ExclusionClear },
            { "excludeclr", ExclusionClear },
            { "clrexclude", ExclusionClear },

            { "inclear", InclusionClear },
            { "clearin", InclusionClear },
            { "inclr", InclusionClear },
            { "clrin", InclusionClear },
            { "includeclear", InclusionClear },
            { "clearinclude", InclusionClear },
            { "includeclr", InclusionClear },
            { "clrinclude", InclusionClear },

            { "exinclear", ClearExcludeInclude },
            { "exinclr", ClearExcludeInclude },
            { "excludeincludeclear", ClearExcludeInclude },
            { "excludeincludeclr", ClearExcludeInclude },
            { "includeexcludeclear", ClearExcludeInclude },
            { "includeexcludeclr", ClearExcludeInclude },
            { "clearexcludeinclude", ClearExcludeInclude },
            { "clrexcludeinclude", ClearExcludeInclude },
            { "clearincludeexclude", ClearExcludeInclude },
            { "clrincludeexclude", ClearExcludeInclude },

            { "backgroundcolor", BackgroundColor },
            { "colorbackground", BackgroundColor },
            { "bgcolor", BackgroundColor },
            { "colorbg", BackgroundColor },


            { "tts", TTS },


        };
    }
    protected virtual void AddToCommandsDict()
    {
        SN.@this.COMMAND.AddToCommandDict(mainCommand.Keys.ToList(), this);
    }
    protected virtual void ClearCommands()
    {
        SN.@this.COMMAND.RemoveFromCommandDict(mainCommand.Keys.ToList());
    }
    #endregion

    public static string[] CommaSeparatedWithSpacesIntoArray(string text)
    {
        string[] texts = text.Split(',');
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i] = texts[i].Trim();
        }
        return texts;
    }
    public static List<string> GetNamesFrom(FileReader.Content content, string listnames)
    {
        List<string> names = new List<string>();
        if (content.commands.Length == 1)
        {
            names.Add(content.name);
        }
        else if (content.commands.Length >= 2)
        {
            string[] persons = CommaSeparatedWithSpacesIntoArray(listnames);
            for (int i = 0; i < persons.Length; i++)
            {
                //Debug.Log(persons[i]);
                names.Add(persons[i]);
            }
        }
        return names;
    }
    public static int GetCommandLength(FileReader.Content content)
    {
        return content.commands[0].Length + 2;
    }
    public static void PopupCheckEmpty(string text, string beforeText, Popups.Data popupData, bool substr = true, int substrLength = 2)
    {
        if (text != string.Empty)
        {
            if (substr)
            {
                text = TrimComma(text, substrLength);
            }
            SN.@this.popups.PutPopup(beforeText + text, popupData);
            return;
        }

    }
    public static string TrimComma(string text, int substrLength = 2)
    {
        if (text.Length > substrLength)
        {
            return text.Substring(0, text.Length - substrLength);
        }
        return text;
    }

    protected virtual void ClearExcludeInclude(FileReader.Content content)
    {
        ExclusionClear(content);
        InclusionClear(content);
    }
    protected virtual void ExclusionClear(FileReader.Content content)
    {
        if (SN.@this.randomExclusion.Count > 0)
        {
            SN.@this.randomExclusion.Clear();
            SN.@this.popups.PutPopup("Exclusions Cleared", SN.excludePopup);
            return;
        }
        SN.@this.popups.PutPopup("No Exclusions to Clear", SN.infoPopup);
    }
    protected virtual void InclusionClear(FileReader.Content content)
    {
        if (SN.@this.randomInclusion.Count > 0)
        {
            SN.@this.randomInclusion.Clear();
            SN.@this.popups.PutPopup("Inclusions Cleared", SN.includePopup);
            return;
        }
        SN.@this.popups.PutPopup("No Inclusions to Clear", SN.infoPopup);
    }

    protected virtual void RemoveFromHashset(FileReader.Content content, HashSet<string> set, out string clearing, out string notFound, bool displaySyntaxError = false, string command = null)
    {
        List<string> names = GetNamesFrom(content, content.text.Substring(GetCommandLength(content)));
        clearing = string.Empty;
        notFound = string.Empty;
        if (names.Count == 0 && displaySyntaxError)
        {
            SN.@this.popups.PutPopup($"Syntax is /{command} Person 1, Person2", SN.notAllowedPopup);
            return;
        }
        for (int i = 0; i < names.Count; i++)
        {
            //Debug.Log(names[i]);
            if (set.Contains(names[i]))
            {
                clearing += $"{names[i]}, ";
                set.Remove(names[i]);
                continue;
            }
            notFound += $"{names[i]}, ";

        }
    }
    protected virtual void AddToHashSet(FileReader.Content content, HashSet<string> set, out string adding, out string alreadyExist, bool displaySyntaxError = false, string command = null)
    {
        List<string> names = GetNamesFrom(content, content.text.Substring(GetCommandLength(content)));
        adding = string.Empty;
        alreadyExist = string.Empty;
        if (names.Count == 0 && displaySyntaxError)
        {
            SN.@this.popups.PutPopup($"Syntax is /{command} Person 1, Person2", SN.notAllowedPopup);
            return;
        }
        for (int i = 0; i < names.Count; i++)
        {
            //Debug.Log(names[i]);
            if (set.Contains(names[i]))
            {
                alreadyExist += $"{names[i]}, ";
                continue;
            }
            set.Add(names[i]);
            adding += $"{names[i]}, ";
            //Debug.Log(adding);
        }
    }
    protected virtual void AddToHashSet(FileReader.Content content, HashSet<string> set, HashSet<string> existRemoveSet, out string adding, out string removedOther, out string alreadyExist, bool displaySyntaxError = false, string command = null)
    {
        List<string> names = GetNamesFrom(content, content.text.Substring(GetCommandLength(content)));
        adding = string.Empty;
        removedOther = string.Empty;
        alreadyExist = string.Empty;
        if (names.Count == 0 && displaySyntaxError)
        {
            SN.@this.popups.PutPopup($"Syntax is /{command} Person 1, Person2", SN.notAllowedPopup);
            return;
        }
        for (int i = 0; i < names.Count; i++)
        {
            //Debug.Log(names[i]);
            if (set.Contains(names[i]))
            {
                alreadyExist += $"{names[i]}, ";
                continue;
            }
            set.Add(names[i]);
            adding += $"{names[i]}, ";
            //Debug.Log(adding);
            if (existRemoveSet.Contains(names[i]))
            {
                removedOther += $"{names[i]}, ";
                existRemoveSet.Remove(names[i]);
            }
        }
    }

    protected virtual void ExcludeRemove(FileReader.Content content)
    {
        RemoveFromHashset(content, SN.@this.randomExclusion, out string clearing, out string notFound, true, "exre");
        PopupCheckEmpty(notFound, "Exclusion Not Found: ", SN.infoPopup);
        PopupCheckEmpty(clearing, "Exclusion Removed: ", SN.excludePopup);
    }
    protected virtual void IncludeRemove(FileReader.Content content)
    {
        RemoveFromHashset(content, SN.@this.randomExclusion, out string clearing, out string notFound, true, "inre");
        PopupCheckEmpty(notFound, "Inclusion Not Found: ", SN.infoPopup);
        PopupCheckEmpty(clearing, "Inclusion Removed: ", SN.includePopup);
    }

    protected void Exclude(FileReader.Content content)
    {
        AddToHashSet(content, SN.@this.randomExclusion, SN.@this.randomInclusion, out string adding, out string removedOther, out string alreadyExist, true, "ex");
        //Debug.Log(adding);
        PopupCheckEmpty(adding, "Excluding: ", SN.excludePopup);
        PopupCheckEmpty(removedOther, "Inclusion Removed: ", SN.includePopup);
        PopupCheckEmpty(alreadyExist, "Exclusion Already Exist: ", SN.infoPopup);
    }
    protected void Include(FileReader.Content content)
    {
        AddToHashSet(content, SN.@this.randomInclusion, SN.@this.randomExclusion, out string adding, out string removedOther, out string alreadyExist, true, "in");
        //Debug.Log(adding);
        PopupCheckEmpty(adding, "Including: ", SN.includePopup);
        PopupCheckEmpty(removedOther, "Exclusion Removed: ", SN.excludePopup);
        PopupCheckEmpty(alreadyExist, "Inclusion Already Exist: ", SN.infoPopup);
    }

    protected virtual void ExcludePermanent(FileReader.Content content)
    {
        AddToHashSet(content, Command.@this.Save.listExclude, out string adding, out string alreadyExist, true, "experm");
        PopupCheckEmpty(adding, "Permanently Excluding: ", SN.excludePopup);
        PopupCheckEmpty(alreadyExist, "Permanent Exclusion Already Exist: ", SN.infoPopup);
    }
    protected virtual void IncludePermanent(FileReader.Content content)
    {
        RemoveFromHashset(content, Command.@this.Save.listExclude, out string clearing, out string notFound, true, "inperm");
        PopupCheckEmpty(clearing, "Permanent Exclusion Removed: ", SN.excludePopup);
        PopupCheckEmpty(notFound, "Permanent Exclusion Not Found: ", SN.infoPopup);
    }

    protected void ShowExclusions(FileReader.Content content)
    {
        if (SN.@this.randomExclusion.Count > 0)
        {
            string text = string.Empty;
            foreach (string person in SN.@this.randomExclusion)
            {
                text += $"{person}, ";
            }
            text = TrimComma(text);

            SN.@this.popups.PutPopup($"Exclusions: {text}", SN.excludePopup);
            return;
        }
        SN.@this.popups.PutPopup($"No exclusions currently", SN.infoPopup);
    }
    protected void ShowInclusions(FileReader.Content content)
    {
        if (SN.@this.randomInclusion.Count > 0)
        {
            string text = string.Empty;
            foreach (string person in SN.@this.randomInclusion)
            {
                text += $"{person}, ";
            }
            text = TrimComma(text);

            SN.@this.popups.PutPopup($"Inclusions: {text}", SN.includePopup);
            return;
        }
        SN.@this.popups.PutPopup($"No inclusions currently", SN.infoPopup);
    }

    protected virtual void RandomClear(FileReader.Content content)
    {
        SN.@this.Random.ClearRandomPopup();
    }

    protected virtual void RandomPpl(FileReader.Content content)
    {
        List<string> list = SN.@this.FilteredNames();
        if (list.Count == 0)
        {
            SN.@this.popups.PutPopup("Please use \"/loadppl\" to add ppl in first", SN.warningPopup);
            return;
        }
        string randomPerson = list[UnityEngine.Random.Range(0, list.Count)];
        SN.@this.Random.NewRandomPopup($"<size=36>Person:</size>\n<b>{randomPerson}</b>");
    }

    protected virtual void RandomPplExclude(FileReader.Content content)
    {
        List<string> list = SN.@this.FilteredNames();
        if (list.Count == 0)
        {
            SN.@this.popups.PutPopup("Not enough ppl to pick from. Please use \"/loadppl\" to add ppl in, or use \"/in\" to include more ppl", SN.warningPopup);
            return;
        }
        string randomPerson = list[UnityEngine.Random.Range(0, list.Count)];
        SN.@this.Random.NewRandomPopup($"<size=36>Person (E):</size>\n<b>{randomPerson}</b>");
        SN.@this.randomInclusion.Remove(randomPerson);
        SN.@this.randomExclusion.Add(randomPerson);
    }

    protected virtual void RandomInList(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = content.text.Substring(GetCommandLength(content));
            string[] texts = CommaSeparatedWithSpacesIntoArray(text);
            if (texts.Length == 0)
            {
                SN.@this.popups.PutPopup("No text detected", SN.warningPopup);
                return;
            }
            string randomText = texts[UnityEngine.Random.Range(0, texts.Length)];
            SN.@this.Random.NewRandomPopup($"<size=36>Element:</size>\n<b>{randomText}</b>");
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /randlist x, yy y, zzz", SN.notAllowedPopup);
    }

    protected virtual void RandomList(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = content.text.Substring(GetCommandLength(content));
            string[] texts = CommaSeparatedWithSpacesIntoArray(text);
            if (texts.Length == 0)
            {
                SN.@this.popups.PutPopup("No text detected", SN.warningPopup);
                return;
            }
            //string randomText = texts[UnityEngine.Random.Range(0, texts.Length)];
            System.Random r = new System.Random();
            texts = texts.OrderBy(x => r.Next()).ToArray();
            string finalText = texts[0];
            for (int i = 1; i < texts.Length; i++)
            {
                finalText += $", {texts[i]}";
            }
            SN.@this.Random.NewRandomPopup($"<size=36>List:</size>\n<b>{finalText}</b>");
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /rls x, yy y, zzz", SN.notAllowedPopup);
    }

    protected virtual void RandomNumber(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string[] text = content.commands[1].Split('-');
            if (text.Length >= 2)
            {
                if (int.TryParse(text[0], out int firstNum))
                {
                    if (int.TryParse(text[1], out int secondNum))
                    {
                        int randomNumber = UnityEngine.Random.Range(firstNum, secondNum + 1);
                        SN.@this.Random.NewRandomPopup($"<size=36>{firstNum}-{secondNum}:</size>\n<b>{randomNumber}</b>");
                        return;
                    }
                }
            }
        }
        SN.@this.popups.PutPopup("Syntax is /randnum 20-50", SN.notAllowedPopup);
        return;

    }

    protected virtual void ClearBoth(FileReader.Content content)
    {
        ClearPpl(content);
        ClearAttendance(content);
    }
    protected virtual void ClearPpl(FileReader.Content content)
    {
        if (SN.@this.listNames.Count > 0)
        {
            SN.@this.listNames.Clear();
            SN.@this.popups.PutPopup("Load Ppl List Cleared", SN.loadPplPopup);
            return;
        }
        SN.@this.popups.PutPopup("No Ppl to Clear", SN.infoPopup);
    }
    protected virtual void ClearAttendance(FileReader.Content content)
    {
        if (SN.@this.attendees.Count > 0)
        {
            SN.@this.attendees.Clear();
            SN.@this.popups.PutPopup("Attendance List Cleared", SN.loadPplPopup);
            return;
        }
        SN.@this.popups.PutPopup("No Ppl in Attendnace to Clear", SN.infoPopup);
    }
    protected virtual void AttendanceGetClear(FileReader.Content content)
    {
        Attendance(content);
        ClearAttendance(content);
    }
    protected virtual void Attendance(FileReader.Content content)
    {
        int attendees = SN.@this.attendees.Count;
        string filename = WriteAttendance();

        SN.@this.popups.PutPopup($"Attendance Exported. Total: {attendees}. File: {filename}", SN.loadPplPopup);
    }
    protected virtual string WriteAttendance()
    {
        string path = DateTime.Now.ToString("dd-MM-yyyy H-mm-ss-ff");
        path = $"Attendance {path}.txt";
        //File.Create(path);
        using StreamWriter sw = File.CreateText(path);
        
        foreach (string person in SN.@this.attendees)
        {
            sw.WriteLine(person);
        }
        return path;
    }
    protected virtual void List(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string[] persons = content.text.Substring(GetCommandLength(content)).Split('\t');

            int originalAttendees = SN.@this.attendees.Count;
            SN.@this.listNames.Clear();
            for (int i = 0; i < persons.Length; i++)
            {
                if (Command.@this.Save.listExclude.Contains(persons[i]))
                {
                    continue;
                }
                SN.@this.listNames.Add(persons[i]);
                SN.@this.attendees.Add(persons[i]);
            }
            int additionalAttendees = SN.@this.attendees.Count - originalAttendees;

            //for (int i = 0; i < persons.Length; i++)
            //{
            //    Debug.Log(persons[i]);
            //}

            SN.@this.popups.PutPopup($"Ppl Loaded. Total: {SN.@this.listNames.Count}. Additional Ppl since last Load: {additionalAttendees})", SN.loadPplPopup);
            return;
        }
        SN.@this.popups.PutPopup($"INTERNAL ERROR. Send Bug Report! (Might have too many people)", SN.notAllowedPopup);
    }

    protected virtual void Announce(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            SN.@this.announcementMessage = text;
            SN.@this.ChangeEnvironment("announce");
            SN.@this.popups.PutPopup("Announcement!", SN.announcePopup);
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /announce Hello World", SN.notAllowedPopup);
    }

    protected virtual void SetTimer(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string[] times = content.commands[1].Split(':', '-', '.');
            if (times.Length > 0 && times.Length <= 3)
            {
                bool isProper = false;
                List<int> numbers = new List<int>();
                for (int i = 0; i < times.Length; i++)
                {
                    if (times[i] != string.Empty)
                    {
                        bool isNumber = int.TryParse(times[i], out int value);
                        if (isNumber)
                        {
                            numbers.Add(value);
                            isProper = true;
                        }
                        else
                        {
                            isProper = false;
                            break;
                        }
                    }
                }
                if (isProper)
                {
                    string text = string.Empty;
                    int totalSeconds = 0;
                    for (int i = numbers.Count - 1; i >= 0; i--)
                    {
                        text = string.Format("{0:D2}", numbers[i]) + text;
                        totalSeconds += numbers[i] * (int)Mathf.Pow(60, numbers.Count - 1 - i);
                    }
                    if (totalSeconds >= 0)
                    {
                        SN.@this.timerSetTimeCountdown = totalSeconds;
                        if (content.commands.Length >= 3)
                        {
                            bool isBool = bool.TryParse(content.commands[2], out bool value);
                            if (!isBool)
                            {
                                value = true;
                            }
                            SN.@this.timerStartPaused = value;
                        }
                        else
                        {
                            SN.@this.timerStartPaused = true;
                        }
                        if (environmentName != "timer")
                        {
                            SN.@this.timerStopReturnEnvironment = environmentName;
                        }
                        SN.@this.ChangeEnvironment("timer");
                        SN.@this.popups.PutPopup("Timer Started: " + text, SN.timerPopup);
                        return;
                    }
                }
            }
        }
        SN.@this.popups.PutPopup("Syntax is /timer 0:5:00", SN.notAllowedPopup);
    }

    protected string ShowColor(Color color)
    {
        string htmlColor = ColorUtility.ToHtmlStringRGB(color);
        return $"><color=#{htmlColor}>●</color><";

    }
    protected virtual void ColorHelper(FileReader.Content content, Action<Color> validSetColor, string command, string exampleColor)
    {
        if (content.commands.Length >= 2)
        {
            bool isColor = ColorUtility.TryParseHtmlString(content.commands[1], out Color color);
            if (isColor)
            {
                validSetColor.Invoke(color);
                return;
            }
            SN.@this.popups.PutPopup($"{content.commands[1]} is an unrecognized color", SN.warningPopup);
            return;
        }

        SN.@this.popups.PutPopup($"Syntax is /{command} {exampleColor}", SN.notAllowedPopup);
    }
    protected virtual void FloatHelper(FileReader.Content content, Action<float> validSetFloat, string command, float exampleValue)
    {
        if (content.commands.Length >= 2)
        {
            bool isFloat = float.TryParse(content.commands[1], out float value);
            if (isFloat)
            {
                validSetFloat(value);
                return;
            }
            SN.@this.popups.PutPopup($"{content.commands[1]} is not a number", SN.warningPopup);
            return;
        }

        SN.@this.popups.PutPopup($"Syntax is /{command} {exampleValue}", SN.notAllowedPopup);
    }
    protected virtual void IntHelper(FileReader.Content content, Action<int> validSetInt, string command, int exampleValue)
    {
        if (content.commands.Length >= 2)
        {
            bool isFloat = int.TryParse(content.commands[1], out int value);
            if (isFloat)
            {
                validSetInt(value);
                return;
            }
            SN.@this.popups.PutPopup($"{content.commands[1]} is not a integer", SN.warningPopup);
            return;
        }

        SN.@this.popups.PutPopup($"Syntax is /{command} {exampleValue}", SN.notAllowedPopup);
    }
    protected virtual void BoolHelper(FileReader.Content content, Action<bool> validSetBool, string command, string exampleBool)
    {
        if (content.commands.Length >= 2)
        {
            bool isBool = bool.TryParse(content.commands[1], out bool value);
            if (isBool)
            {
                validSetBool(value);
                return;
            }
            SN.@this.popups.PutPopup($"{content.commands[1]} is not on/off/true/false", SN.warningPopup);
        }

        SN.@this.popups.PutPopup($"Syntax is /{command} {exampleBool}", SN.notAllowedPopup);
    }
    protected virtual void ColorsHelper(FileReader.Content content, List<Color> targetColors, List<SColor> saveColors, string command, string addingGroup, bool shout = true)
    {
        if (content.commands.Length >= 2)
        {
            bool clearedColors = false;
            for (int i = 1; i < content.commands.Length; i++)
            {
                bool isColor = ColorUtility.TryParseHtmlString(content.commands[i], out Color color);
                if (isColor)
                {
                    if (!clearedColors)
                    {
                        targetColors.Clear();
                        saveColors.Clear();
                        clearedColors = true;
                    }
                    targetColors.Add(color);
                    saveColors.Add(color);
                    if (shout)
                    {
                        SN.@this.popups.PutPopup($"{ShowColor(color)} added into {addingGroup}", SN.BrushPopup(color));
                    }
                }
                else
                {
                    if (shout)
                    {
                        SN.@this.popups.PutPopup($"{content.commands[i]} is an unrecognized color", SN.warningPopup);
                    }
                }
            }
        }
        if (shout)
        {
            SN.@this.popups.PutPopup($"Syntax is /{command} red green blue", SN.notAllowedPopup);
        }
    }

    protected virtual void SetBackground(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            StartCoroutine(FetchTexture(content.commands[1]));
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /setbackground https://upload.wikimedia.org/wikipedia/commons/thumb/9/9f/Pitta_versicolor_-_Kembla_Heights.jpg/350px-Pitta_versicolor_-_Kembla_Heights.jpg", SN.notAllowedPopup);
    }
    protected IEnumerator FetchTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        SetBackground(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)), url);
        SN.@this.popups.PutPopup("Background Changed", SN.imagePopup);
    }
    protected virtual void SetBackground(Sprite sprite, string urlReference)
    {
        container.backgroundUrl = urlReference;
        backgroundImage.enabled = true;
        backgroundImage.sprite = sprite;
    }
    protected virtual void RemoveBackground(FileReader.Content content)
    {
        backgroundImage.sprite = null;
        backgroundImage.enabled = false;
        container.backgroundUrl = null;
        SN.@this.popups.PutPopup("Background Removed", SN.imagePopup);
    }
    protected virtual void BackgroundColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            baseBackgroundImage.color = color;
            container.backgroundColor = color;
            container.backgroundUrl = null;
            SN.@this.popups.PutPopup($"Background color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "backgroundcolor", "blue");
    }

    protected virtual void SetPP(FileReader.Content content)
    {
        if (content.commands.Length >= 3)
        {
            switch (content.commands[1])
            {
                case "bloom":
                    bool isFloat = false;
                    bool isColor = false;
                    float value = -1f;
                    Color tint = Color.white;
                    if (content.commands.Length >= 4)
                    {
                        isFloat = float.TryParse(content.commands[3], out value);
                        if (!isFloat)
                        {
                            isColor = ColorUtility.TryParseHtmlString(content.commands[3], out tint);
                        }
                    }
                    switch (content.commands[2])
                    {
                        case "threshold":
                            if (isFloat && value > 0f)
                            {
                                SN.@this.bloom.threshold.value = value;
                                container.bloomThreshold = value;
                                SN.@this.popups.PutPopup($"Threshold set to {value}", SN.bloomPopup);
                                return;
                            }
                            SN.@this.popups.PutPopup($"{content.commands[2]} is not a number", SN.warningPopup);
                            return;

                        case "intensity":
                            if (isFloat && value > 0f)
                            {
                                SN.@this.bloom.intensity.value = value;
                                container.bloomIntensity = value;
                                SN.@this.popups.PutPopup($"Intensity set to {value}", SN.bloomPopup);
                                return;
                            }
                            SN.@this.popups.PutPopup($"{content.commands[2]} is not a number", SN.warningPopup);
                            return;

                        case "tint":
                            if (isColor)
                            {
                                SN.@this.bloom.tint.value = tint;
                                container.bloomColor = tint;
                                SN.@this.popups.PutPopup($"Tint set to {ShowColor(tint)}", SN.bloomPopup);
                                return;
                            }
                            SN.@this.popups.PutPopup($"{content.commands[2]} is an unrecognized color", SN.warningPopup);
                            return;

                        case "true":
                        case "on":
                            SN.@this.bloom.active = true;
                            container.bloomEnabled = true;
                            SN.@this.popups.PutPopup("Bloom turned on", SN.bloomPopup);
                            return;

                        case "false":
                        case "off":
                            SN.@this.bloom.active = false;
                            container.bloomEnabled = false;
                            SN.@this.popups.PutPopup("Bloom turned off", SN.bloomPopup);
                            return;

                        default:
                            SN.@this.popups.PutPopup($"{content.commands[2]} Unrecognized. Use /setpp bloom [threshold/intensity/tint/on/off] [optional]", SN.warningPopup);
                            return;
                    }
            }
            SN.@this.popups.PutPopup("Syntax is /setpp bloom [parameters]", SN.notAllowedPopup);
        }
    }
    protected virtual void TTS(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            using (MemoryStream audioStream = new MemoryStream())
            {
                synth.SetOutputToWaveStream(audioStream);
                System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer();

                // Configure the synthesizer to output to an audio stream.  
                synth.SetOutputToWaveStream(audioStream);

                // Speak a phrase.  
                synth.Speak("This is sample text-to-speech output.");
                audioStream.Position = 0;
                m_SoundPlayer.Stream = audioStream;
                m_SoundPlayer.Play();

                // Set the synthesizer output to null to release the stream.   
                synth.SetOutputToNull();
                synth.Speak(text);
            }
            return;
        }
    }

    protected void Start()
    {
        StartEnvironment();
    }
    protected void OnDisable()
    {
        StopEnvironment();
    }

    protected void ManageSave(EnvironmentContainer container)
    {
        this.container = container;
        ManageSave();
    }
    protected virtual void ManageSave()
    {
        if (container != null)
        {
            if (container.backgroundUrl != null)
            {
                StartCoroutine(FetchTexture(container.backgroundUrl));
            }
            if (container.backgroundColor != null)
            {
                baseBackgroundImage.color = container.backgroundColor;
            }
            SN.@this.bloom.active = container.bloomEnabled;
            if (container.bloomThreshold > 0f)
            {
                SN.@this.bloom.threshold.value = container.bloomThreshold;
            }
            if (container.bloomIntensity > 0f)
            {
                SN.@this.bloom.intensity.value = container.bloomIntensity;
            }
            if (container.bloomColor != null)
            {
                SN.@this.bloom.tint.value = container.bloomColor;
            }
        }
    }

    /// <summary>
    /// Will be called automatically in Start
    /// </summary>
    public virtual void StartEnvironment()
    {
        if (!environmentRunning)
        {
            SN.@this.inputActions.Main.RefreshEnvironment.performed += RefreshEnvironment;
            AddCommands();
            AddToCommandsDict();
            environmentRunning = true;
        }
    }

    public virtual void StopEnvironment()
    {
        if (environmentRunning)
        {
            SN.@this.inputActions.Main.RefreshEnvironment.performed -= RefreshEnvironment;
            ClearCommands();
            environmentRunning = false;
        }
    }

    protected string environmentName;
    public virtual void RefreshEnvironment(InputAction.CallbackContext ctx)
    {
        SN.@this.ChangeEnvironment(environmentName);
    }


}
