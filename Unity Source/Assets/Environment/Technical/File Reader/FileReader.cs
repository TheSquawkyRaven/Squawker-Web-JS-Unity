using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Concurrent;

public class FileReader : ICommand
{

    public ChatReceiver receiver;

    [Serializable]
    public class FileContainer
    {
        public Dictionary<string, string> remappedNames = new Dictionary<string, string>();
    }
    public FileContainer Save => SN.@this.save.fileContainer;

    public class Content
    {
        public string name;
        public string text;
        public int textNextLines;

        public string displayName;

        public string errors = string.Empty;
        public bool isCommand;
        /// <summary>
        /// Inclusive first command, ToLowered
        /// </summary>
        public string[] commands;
    }

    [HideInInspector] public List<Content> contents = new List<Content>();
    [HideInInspector] public readonly string[] splittingString = new string[] { "{:}" };
    [HideInInspector] public bool isRunning = false;

    public Action<Content> onContentChange = new Action<Content>((content)=> { });
    [HideInInspector] public int ioErrorCount = 0;

    private Dictionary<string, Action<Content>> mainCommand;
    private Dictionary<string, string> remappedNames = new Dictionary<string, string>();

    public FileReader()
    {
        receiver = new ChatReceiver();

        SN.@this.inputActions.Main.StartReading.performed += StartReading;
        SN.@this.inputActions.Main.StopReading.performed += StopReading;

        if (Save.remappedNames == null)
        {
            Save.remappedNames = new Dictionary<string, string>();
        }
        if (Save.remappedNames.Count > 0)
        {
            foreach (var item in Save.remappedNames)
            {
                remappedNames.Add(item.Key, item.Value);
            }
        }
        AddCommand();

        StartReading();
    }

    public void ExecuteCommand(Content content)
    {
        if (mainCommand.Count > 0)
        {
            bool commandFound = false;
            foreach (KeyValuePair<string, Action<Content>> command in mainCommand)
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
    private void AddCommand()
    {
        mainCommand = new Dictionary<string, Action<Content>>()
        {
            { "r", RemapName },
            { "remap", RemapName },
            { "remapname", RemapName },
            { "rr", RemoveRemapName },
            { "removeremap", RemoveRemapName },
            { "remapre", RemoveRemapName },
            { "reremap", RemoveRemapName },
        };
        SN.@this.COMMAND.AddToCommandDict(mainCommand.Keys.ToList(), this);
    }
    private void RemapName(Content content)
    {
        if (content.commands.Length >= 2)
        {
            string displayName = "";
            for (int i = 1; i < content.commands.Length; i++)
            {
                displayName += $"{content.commands[i]}{((i + 1) < content.commands.Length ? " " : string.Empty)}";
            }
            if (remappedNames.TryGetValue(content.name, out string originalName))
            {
                remappedNames[content.name] = displayName;
                Save.remappedNames[content.name] = displayName;

                SN.@this.popups.PutPopup($"{originalName} has jumped to becoming {displayName}. Parkour!", SN.remapPopup);
                return;
            }
            remappedNames.Add(content.name, displayName);
            Save.remappedNames.Add(content.name, displayName);

            SN.@this.popups.PutPopup($"{content.name} is now known as {displayName}", SN.remapPopup);
            return;

        }

        SN.@this.popups.PutPopup("Syntax is /remap Batman with living parents", SN.notAllowedPopup);
    }
    private void RemoveRemapName(Content content)
    {
        if (remappedNames.TryGetValue(content.name, out string displayName))
        {
            remappedNames.Remove(content.name);
            Save.remappedNames.Remove(content.name);

            SN.@this.popups.PutPopup($"{displayName} is revealed to be {content.name} all along!", SN.remapPopup);
            return;
        }

        SN.@this.popups.PutPopup($"{content.name}:\"Wait, I'm {content.name}?\"\nSomeone:\"Always has been.\"", SN.infoPopup);
    }

    private void StartReading(InputAction.CallbackContext ctx)
    {
        StartReading();
    }
    public void StartReading()
    {
        if (!isRunning)
        {
            isRunning = true;
            SN.@this.StartCoroutine(ReadServer());
        }
    }
    private void StopReading(InputAction.CallbackContext ctx)
    {
        StopReading();
    }
    public void StopReading()
    {
        isRunning = false;
    }

    private IEnumerator ReadServer()
    {

        ConcurrentQueue<string> onConnectMessages = receiver.onConnectMessages;

        ConcurrentQueue<string> lines = receiver.messages;
        string[] split;

        while (isRunning)
        {

            if (onConnectMessages.Count > 0)
            {
                while (!onConnectMessages.IsEmpty)
                {
                    if (onConnectMessages.TryDequeue(out string connectText))
                    {
                        SN.@this.popups.PutPopup(connectText, SN.connectPopup);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (SN.@this.read)
            {
                if (lines.Count > 0)
                {
                    while (!lines.IsEmpty)
                    {
                        if (lines.TryDequeue(out string line))
                        {
                            if (line != "")
                            {
                                split = line.Split(splittingString, StringSplitOptions.None);
                                if (split.Length == 2)
                                {
                                    //string replaced = split[1].Replace("{\\n}", "\n");
                                    string replaced = split[1];
                                    Content content = new Content()
                                    {
                                        name = split[0],
                                        text = replaced,
                                        textNextLines = (replaced.Length - replaced.Replace("\n", "").Length)
                                    };
                                    if (remappedNames.ContainsKey(content.name))
                                    {
                                        content.displayName = remappedNames[content.name];
                                    }
                                    else
                                    {
                                        content.displayName = content.name;
                                    }
                                    contents.Add(content);
                                    onContentChange.Invoke(content);
                                }
                                else
                                {
                                    Debug.Log("Wrong format... => " + line);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(SN.@this.readChatPulse);
        }
    }
    //private IEnumerator Read()
    //{

    //    int lastLine = 0;
    //    string[] lines;
    //    string[] split;

    //    while (isRunning)
    //    {
    //        if (SN.@this.read)
    //        {
    //            lines = null;
    //            try
    //            {
    //                lines = File.ReadAllLines(SN.outputTxtPath);
    //            }
    //            catch (Exception)
    //            {
    //                ioErrorCount++;
    //            }
    //            if (lines != null)
    //            {
    //                for (int i = lastLine; i < lines.Length; i++)
    //                {
    //                    if (lines[i] != "")
    //                    {
    //                        split = lines[i].Split(splittingString, StringSplitOptions.None);
    //                        if (split.Length == 2)
    //                        {
    //                            string replaced = split[1].Replace("{\\n}", "\n");
    //                            Content content = new Content()
    //                            {
    //                                name = split[0],
    //                                text = replaced,
    //                                textNextLines = (replaced.Length - replaced.Replace("\n", "").Length)
    //                            };
    //                            if (remappedNames.ContainsKey(content.name))
    //                            {
    //                                content.displayName = remappedNames[content.name];
    //                            }
    //                            else
    //                            {
    //                                content.displayName = content.name;
    //                            }
    //                            contents.Add(content);
    //                            onContentChange.Invoke(content);
    //                        }
    //                        else
    //                        {
    //                            Debug.Log("Wrong format...");
    //                        }
    //                    }
    //                    lastLine = i + 1;
    //                }
    //            }
    //        }
    //        yield return new WaitForSeconds(SN.@this.readChatPulse);
    //    }

    //}

    //public void ApplicationQuitCallback()
    //{
    //    //DateTime dateTime = DateTime.Now;
    //    //string directoryPath = $"{SN.@this.chatFolderPath}log";
    //    //string targetPath = $"{directoryPath}\\{dateTime:MM-dd-yyyy-HH-mm-ss}.txt";
    //    //if (!Directory.Exists(directoryPath))
    //    //{
    //    //    Directory.CreateDirectory(directoryPath);
    //    //}
    //    //File.Copy(SN.@this.chatFilePath, targetPath);
    //    File.WriteAllText(SN.outputTxtPath, string.Empty);
    //}

}
