using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;


public class ConsoleLogManager : MonoBehaviour
{
    [Tooltip("The text output")]
    public Text consoleOutput;

    [Tooltip("The size of the font the log text is displayed in.")]
    public int fontSize = 14;
    [Tooltip("The colour of the text for an info log message.")]
    public Color infoMessage = Color.black;
    [Tooltip("The colour of the text for an assertion log message.")]
    public Color assertMessage = Color.black;
    [Tooltip("The colour of the text for a warning log message.")]
    public Color warningMessage = Color.yellow;
    [Tooltip("The colour of the text for an error log message.")]
    public Color errorMessage = Color.red;
    [Tooltip("The colour of the text for an exception log message.")]
    public Color exceptionMessage = Color.red;

    protected Dictionary<LogType, Color> logTypeColors;
    protected const string NEWLINE = "\n";


    public static ConsoleLogManager instance;



    public virtual void ClearLog()
    {
        consoleOutput.text = "";

    }

    protected virtual void Awake()
    {
        instance = this;

        logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Assert, assertMessage },
            { LogType.Error, errorMessage },
            { LogType.Exception, exceptionMessage },
            { LogType.Log, infoMessage },
            { LogType.Warning, warningMessage }
        };
        
        

        consoleOutput.fontSize = fontSize;
        ClearLog();
    }

    protected virtual void OnEnable() { 
        Application.logMessageReceived += HandleLog; 
    }

    protected virtual void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    protected virtual string GetMessage(string message, LogType type)
    {
        string color = ColorUtility.ToHtmlStringRGBA(logTypeColors[type]);
        return "<color=#" + color + ">" + message + "</color>" + NEWLINE;
    }

    protected virtual void HandleLog(string message, string stackTrace, LogType type)
    {
        string logOutput = GetMessage(message, type);

        consoleOutput.text += logOutput;
       
    }




}
