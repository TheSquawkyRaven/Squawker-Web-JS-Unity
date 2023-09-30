using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Command : MonoBehaviour, ICommand
{

    public static Command @this;

    [System.Serializable]
    public class CommandContainer
    {
        public HashSet<string> admins = new HashSet<string>();
        public HashSet<string> listExclude = new HashSet<string>();
    }
    public CommandContainer Save => SN.@this.save.commandContainer;

    public bool active;

    [SerializeField] private HashSet<string> admins = new HashSet<string>();
    [SerializeField] private char commandSymbol = '/';

    private Dictionary<string, System.Action<FileReader.Content>> mainCommand;
    [System.NonSerialized] public bool adminMode = true;

    #region Commands Initialization
    private void Awake()
    {
        @this = this;
        SN.@this.FILEReader.onContentChange += CheckContent;
    }
    private void Start()
    {
        AddCommand();
        if (Save != null)
        {
            if (Save.admins == null)
            {
                Save.admins = new HashSet<string>();
            }
            if (Save.admins.Count > 0)
            {
                admins.Clear();
                foreach (string admin in Save.admins)
                {
                    admins.Add(admin);
                }
            }
        }
    }
    public void AddToCommandDict(List<string> commands, ICommand command)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (SN.@this.stringToCommand.ContainsKey(commands[i]))
            {
                Debug.LogError($"Command {commands[i]} already added");
            }
            else
            {
                SN.@this.stringToCommand.Add(commands[i], command);
            }
        }
    }
    public void RemoveFromCommandDict(List<string> commands)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (SN.@this.stringToCommand.ContainsKey(commands[i]))
            {
                SN.@this.stringToCommand.Remove(commands[i]);
            }
            else
            {
                Debug.LogWarning($"Command {commands[i]} is already removed/never added");
            }
        }
    }
    #endregion

    public void Refresh()
    {
        //SN.@this.stringToCommand.Clear();
    }

    private void AddCommand()
    {
        mainCommand = new Dictionary<string, System.Action<FileReader.Content>>()
        {
            { "setadminmode", SetAdminMode },
            { "adminmode", SetAdminMode },

            { "add", SetAdmin },
            { "setadmin", SetAdmin },
            { "admin", SetAdmin },

            { "adre", RemoveAdmin },
            { "remove", RemoveAdmin },
            { "removeadmin", RemoveAdmin },
        };
        AddToCommandDict(mainCommand.Keys.ToList(), this);
    }
    void ICommand.ExecuteCommand(FileReader.Content content)
    {
        if (mainCommand.Count > 0)
        {
            bool commandFound = false;
            foreach (KeyValuePair<string, System.Action<FileReader.Content>> command in mainCommand)
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
    private void SetAdminMode(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            switch (content.commands[1])
            {
                case "off":
                case "false":
                    adminMode = false;
                    SN.@this.popups.PutPopup("Admin mode turned off", SN.adminPopup);
                    return;
                case "on":
                case "true":
                    adminMode = true;
                    SN.@this.popups.PutPopup("Admin mode turned on", SN.adminPopup);
                    return;
            }
        }
        SN.@this.popups.PutPopup("Syntax is /adminmode on/off", SN.warningPopup);
    }

    private void SetAdmin(FileReader.Content content)
    {
        if (content.commands.Length > 1)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            if (admins.Contains(text))
            {
                SN.@this.popups.PutPopup($"{text} is already an admin!", SN.infoPopup);
                return;
            }
            admins.Add(text);
            Save.admins.Add(text);
            SN.@this.popups.PutPopup($"{text} is promoted as an admin", SN.adminPopup);
            return;
        }
        SN.@this.popups.PutPopup("Syntax is /admin Joe Momma", SN.warningPopup);

    }
    private void RemoveAdmin(FileReader.Content content)
    {
        if (content.commands.Length > 1)
        {
            string text = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                text += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            if (admins.Contains(text))
            {
                admins.Remove(text);
                Save.admins.Remove(text);
                SN.@this.popups.PutPopup($"{text} has been fired from the admin position", SN.adminPopup);
                return;
            }
            SN.@this.popups.PutPopup($"{text} is already fired! Stop firing fired people already!", SN.infoPopup);
            return;
        }
        SN.@this.popups.PutPopup($"Syntax is /removeadmin Joe Momma", SN.warningPopup);
    }

    public void CheckContent(FileReader.Content content)
    {
        if (active)
        {
            if (CheckHasCommandSymbol(content))
            {
                if (CheckIsCommand(content))
                {
                    if (adminMode)
                    {
                        if (CheckIsAdmin(content))
                        {
                            ExecuteCommand(content);
                        }
                    }
                    else
                    {
                        ExecuteCommand(content);
                    }
                }
            }
        }
    }

    private bool CheckIsAdmin(FileReader.Content content)
    {
        if (content.name == "You" || content.name == "RAVEN LIM ZHE XUAN")
        {
            return true;
        }
        if (admins.Contains(content.name))
        {
            return true;
        }
        return false;
    }

    private bool CheckIsCommand(FileReader.Content content)
    {
        content.commands = content.text.Substring(1).Split(' ', '\n', '\t');
        if (content.commands.Length > 0)
        {
            if (SN.@this.stringToCommand.ContainsKey(content.commands[0]))
            {
                content.isCommand = true;
                return true;
            }
        }
        return false;
    }

    private bool CheckHasCommandSymbol(FileReader.Content content)
    {
        if (content.text.Length > 0)
        {
            if (content.text[0] == commandSymbol)
            {
                return true;
            }
        }
        return false;
    }

    private void ExecuteCommand(FileReader.Content content)
    {
        if (content.commands.Length > 0)
        {
            if (SN.@this.stringToCommand.ContainsKey(content.commands[0]))
            {
                SN.@this.stringToCommand[content.commands[0]].ExecuteCommand(content);
            }
            else
            {
                content.errors = "Command Not Found";
                SN.@this.popups.PutPopup($"{content.commands[0]} is not a recognized command", SN.warningPopup);
            }
        }
    }

}

public interface ICommand
{
    /// <summary>
    /// Check length > 0 before calling
    /// </summary>
    /// <returns>Error, if not, null</returns>
    void ExecuteCommand(FileReader.Content content);
}