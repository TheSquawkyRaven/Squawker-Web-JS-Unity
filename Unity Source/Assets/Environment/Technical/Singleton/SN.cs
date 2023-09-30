using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SN : MonoBehaviour, ICommand
{
    public static SN @this;

    #region Technical
    public InputActions inputActions;
    #endregion

    #region File Reading
    public bool read = true;
    public FileReader FILEReader;
    public float readChatPulse;

    //public const string outputTxtPath = "../output.txt";

    
#if UNITY_EDITOR
    public const string savePath = "C:/Users/pc/Desktop/Squawker/save.save";
    //public const string serverPath = "C:/Users/pc/Desktop/Squawker/";           //No need /Save/
#else
    public const string savePath = "../save.save";
    public const string serverPath = "";
#endif
    #endregion



    #region Graphics
    public Volume postProcessVolume;
    public Bloom bloom;
    #endregion

    #region Save File
    [Serializable]
    public class SaveContainer
    {

        public MatrixEnvironment.MatrixContainer matrixContainer = new MatrixEnvironment.MatrixContainer();
        public DVDEnvironment.DVDContainer dvdContainer = new DVDEnvironment.DVDContainer();
        public CatEnvironment.CatContainer catContainer = new CatEnvironment.CatContainer();
        public FileReader.FileContainer fileContainer = new FileReader.FileContainer();
        public ChatEnvironment.ChatContainer willieContainer = new ChatEnvironment.ChatContainer();
        public TimerEnvironment.TimerContainer timerContainer = new TimerEnvironment.TimerContainer();
        public AnnouncementEnvironment.AnnouncementContainer announcementContainer = new AnnouncementEnvironment.AnnouncementContainer();
        public Command.CommandContainer commandContainer = new Command.CommandContainer();
        public void Check()
        {

            if (matrixContainer == null)
            {
                matrixContainer = new MatrixEnvironment.MatrixContainer();
            }
            if (dvdContainer == null)
            {
                dvdContainer = new DVDEnvironment.DVDContainer();
            }
            if (catContainer == null)
            {
                catContainer = new CatEnvironment.CatContainer();
            }
            if (fileContainer == null)
            {
                fileContainer = new FileReader.FileContainer();
            }
            if (willieContainer == null)
            {
                willieContainer = new ChatEnvironment.ChatContainer();
            }
            if (timerContainer == null)
            {
                timerContainer = new TimerEnvironment.TimerContainer();
            }
            if (announcementContainer == null)
            {
                announcementContainer = new AnnouncementEnvironment.AnnouncementContainer();
            }
            if (commandContainer == null)
            {
                commandContainer = new Command.CommandContainer();
            }
        }
    }
    [NonSerialized] public SaveContainer save = new SaveContainer();
#endregion

    #region Environment
    [Serializable]
    public class EnvironmentReference
    {
        public string environmentName;
        [Tooltip("PREFAB")]
        public GameObject environment;
        public Transform parent;
    }
    public Environment ENVIRONMENT;
    public EnvironmentReference defaultEnvironment = new EnvironmentReference();
    public List<EnvironmentReference> environmentReferences = new List<EnvironmentReference>();

    public Popups popups;
    public RandomPopup Random;

    public HashSet<string> randomInclusion = new HashSet<string>();
    public HashSet<string> randomExclusion = new HashSet<string>();
    public List<string> listNames = new List<string>();
    public HashSet<string> attendees = new HashSet<string>();

    public List<string> FilteredNames()
    {
        HashSet<string> filteredNames = new HashSet<string>(listNames);
        HashSet<string> addingNames = new HashSet<string>();
        HashSet<string> removingNames = new HashSet<string>();

        foreach (string person in randomInclusion)
        {
            if (!filteredNames.Contains(person))
            {
                addingNames.Add(person);
            }
        }
        foreach (string person in randomExclusion)
        {
            if (filteredNames.Contains(person))
            {
                removingNames.Add(person);
            }
        }

        foreach (string person in addingNames)
        {
            filteredNames.Add(person);
        }
        foreach (string person in removingNames)
        {
            filteredNames.Remove(person);
        }

        return filteredNames.ToList();
    }



    [NonSerialized] public float timerSetTimeCountdown = -1f;
    [NonSerialized] public bool timerStartPaused = false;
    [NonSerialized] public string timerStopReturnEnvironment = string.Empty;

    [NonSerialized] public string announcementMessage = string.Empty;
    #endregion

    #region Command
    public Command COMMAND;
    public Dictionary<string, ICommand> stringToCommand = new Dictionary<string, ICommand>();
    private Dictionary<string, Action<FileReader.Content>> mainCommand = new Dictionary<string, Action<FileReader.Content>>(); //local command
    #endregion


    public static Popups.Data loadPplPopup;
    public static Popups.Data excludePopup;
    public static Popups.Data includePopup;
    public static Popups.Data randomPopup;
    public static Popups.Data warningPopup;
    public static Popups.Data infoPopup;
    public static Popups.Data notAllowedPopup;
    private static Popups.Data brushPopup;
    public static Popups.Data imagePopup;
    public static Popups.Data envSwitchPopup;
    public static Popups.Data timerPopup;
    public static Popups.Data textPopup;
    public static Popups.Data announcePopup;
    public static Popups.Data mutePopup;
    public static Popups.Data unmutePopup;
    public static Popups.Data catPopup;
    public static Popups.Data optionsPopup;
    public static Popups.Data playPopup;
    public static Popups.Data pausePopup;
    public static Popups.Data stopPopup;
    public static Popups.Data remapPopup;
    public static Popups.Data bloomPopup;
    public static Popups.Data adminPopup;
    public static Popups.Data connectPopup;

    public static Popups.Data BrushPopup(Color color)
    {
        brushPopup.iconColor = color;
        brushPopup.outlineColor = color;
        return brushPopup;
    }

    public static Color orange = new Color(1f, 0.5f, 0f);
    public static Color pinkred = new Color(1f, 0f, 0.5f);
    public static Color pink = new Color(1f, 0f, 1f);
    public static Color darkblue = new Color(0f, 0f, 0.5f);
    public static Color lightblue = new Color(0.5f, 0.5f, 1f);
    public static Color brown = new Color(0.5f, 0.25f, 0f);
    public static Color bluecyan = new Color(0f, 0.5f, 1f);

    public Action updateAction = new Action(()=> { });

    public void InitializePopupData()
    {
        loadPplPopup = new Popups.Data(popups.loadIcon)
        {
            textColor = lightblue,
            iconColor = lightblue,
            outlineColor = darkblue,
            iconBackgroundColor = darkblue,
            backgroundColor = Color.black,
        };

        excludePopup = new Popups.Data(popups.excludeIcon)
        {

        };
        excludePopup.SetColorBW(Color.red, Color.cyan);
        includePopup = new Popups.Data(popups.includeIcon)
        {

        };
        includePopup.SetColorBW(Color.blue, Color.yellow);
        randomPopup = new Popups.Data(popups.randomIcon)
        {

        };
        randomPopup.SetColorBW(Color.white, Color.blue);
        warningPopup = new Popups.Data(popups.warningIcon)
        {

        };
        warningPopup.SetColorBW(Color.red, Color.white);
        infoPopup = new Popups.Data(popups.infoIcon)
        {

        };
        infoPopup.SetColorBW(Color.cyan, Color.blue);
        notAllowedPopup = new Popups.Data(popups.notAllowedIcon)
        {
            
        };
        notAllowedPopup.SetColorBW(Color.white, Color.red);
        brushPopup = new Popups.Data(popups.brushIcon)
        {
            
        };
        brushPopup.SetColorBW(Color.black, Color.black);    //Initialize with black & white
        imagePopup = new Popups.Data(popups.imageIcon)
        {
            
        };
        imagePopup.SetColorBW(Color.yellow, brown);
        envSwitchPopup = new Popups.Data(popups.envSwitchIcon)
        {

        };
        envSwitchPopup.SetColorBW(Color.green, brown);
        timerPopup = new Popups.Data(popups.timerIcon)
        {

        };
        timerPopup.SetColorBW(Color.white, bluecyan);
        textPopup = new Popups.Data(popups.textIcon)
        {
            outlineColor = orange,
            textColor = Color.white,
            iconBackgroundColor = Color.black,
            iconColor = Color.white,
            backgroundColor = Color.black,
        };

        announcePopup = new Popups.Data(popups.announceIcon)
        {

        };
        announcePopup.SetColorBW(orange, Color.blue);
        mutePopup = new Popups.Data(popups.muteIcon)
        {

        };
        mutePopup.SetColorBW(Color.white, pinkred);
        unmutePopup = new Popups.Data(popups.unmuteIcon)
        {

        };
        unmutePopup.SetColorBW(Color.white, pinkred);
        catPopup = new Popups.Data(popups.catIcon)
        {

        };
        catPopup.SetColorBW(orange, darkblue);
        optionsPopup = new Popups.Data(popups.optionsIcon)
        {

        };
        optionsPopup.SetColorBW(Color.black, Color.green);
        playPopup = new Popups.Data(popups.playIcon)
        {

        };
        playPopup.SetColorBW(Color.green, Color.blue);
        pausePopup = new Popups.Data(popups.pauseIcon)
        {

        };
        pausePopup.SetColorBW(Color.yellow, Color.blue);
        stopPopup = new Popups.Data(popups.stopIcon)
        {

        };
        stopPopup.SetColorBW(Color.red, Color.blue);
        remapPopup = new Popups.Data(popups.remapIcon)
        {

        };
        remapPopup.SetColorBW(orange, Color.blue);
        bloomPopup = new Popups.Data(popups.bloomIcon)
        {

        };
        bloomPopup.SetColor(Color.yellow, Color.red);
        adminPopup = new Popups.Data(popups.adminIcon)
        {

        };
        adminPopup.SetColorBW(Color.black, Color.white);
        connectPopup = new Popups.Data(popups.connectIcon)
        {

        };
        connectPopup.SetColorBW(Color.blue, Color.white);
    }

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        @this = this;

        //StartNodeJSServer();
        Load();
        inputActions = new InputActions();
        inputActions.Enable();
        inputActions.Main.Enable();
        DontDestroyOnLoad(gameObject);

        FILEReader = new FileReader();
        InitializePopupData();
        AddMainCommands();
        AssignVolumes();

    }

    private void AssignVolumes()
    {
        postProcessVolume.profile.TryGet(out bloom);
    }

    private void AddMainCommands()
    {
        mainCommand = new Dictionary<string, Action<FileReader.Content>>()
        {
            { "setenvironment", ChangeEnvironment },
            { "environment", ChangeEnvironment },
            { "env", ChangeEnvironment },
        };
        COMMAND.AddToCommandDict(mainCommand.Keys.ToList(), this);
    }

    private void Start()
    {
        if (ENVIRONMENT == null && environmentReferences.Count > 0)
        {
            ChangeEnvironment(defaultEnvironment.environment, defaultEnvironment.parent);
        }
    }

    private void Update()
    {
        updateAction.Invoke();
    }

    private void OnApplicationQuit()
    {
        FILEReader.receiver.ApplicationClose();
        Save();
    }

    //private void StartNodeJSServer()
    //{
    //    return;
    //    string strCmdText = $"/C cd \"../\" & node {serverPath}Server/server.js & pause";
    //    System.Diagnostics.Process.Start("CMD.exe", strCmdText);
    //}

    private void Load()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                object deserializedObject = formatter.Deserialize(stream);
                save = (SaveContainer)deserializedObject;
                stream.Close();

            }
        }
        catch (Exception)
        {
            save = new SaveContainer();
        }
        save.Check();

    }

    private void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(stream, save);
        stream.Close();
    }

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
    public void ChangeEnvironment(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            EnvironmentReference reference = environmentReferences.SingleOrDefault(x => x.environmentName == content.commands[1]);
            if (reference != null)
            {
                ChangeEnvironment(reference.environment, reference.parent);
                @this.popups.PutPopup($"Environment changed to {reference.environmentName}", envSwitchPopup);
                return;
            }
            else
            {
                content.errors = "Environment Not Found";
                @this.popups.PutPopup($"{content.commands[1]} not found!", warningPopup);
                return;
            }
        }
        @this.popups.PutPopup("Syntax is /env cat", notAllowedPopup);
    }
    /// <summary>
    /// Will instantiate an environment
    /// </summary>
    public void ChangeEnvironment(GameObject environment, Transform parent = null)
    {
        if (ENVIRONMENT != null)
        {
            ENVIRONMENT.StopEnvironment();
            Destroy(ENVIRONMENT.gameObject);
        }
        ENVIRONMENT = Instantiate(environment, parent).GetComponent<Environment>();
    }

    public void ChangeEnvironment(string environmentName)
    {
        EnvironmentReference reference = @this.environmentReferences.SingleOrDefault(x => x.environmentName == environmentName);
        if (reference != null)
        {
            @this.ChangeEnvironment(reference.environment, reference.parent);
        }
    }

}
