﻿ using UnityEngine;
 using UnityEngine.UI;
 using System.Collections.Generic;
 using System.Text.RegularExpressions;
 using System.Linq;

public class ConsoleViewer : MonoBehaviour
{
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
    protected ScrollRect scrollWindow;
    protected RectTransform consoleRect;
    protected Text consoleOutput;
    protected const string NEWLINE = "\n";
    protected int lineBuffer = 50;
    protected int currentBuffer;
    protected string lastMessage;
    protected bool collapseLog = false;
    OVRCameraRig rig;
    private Vector3 menuOffset;

    public static ConsoleViewer Instance;

    /// <summary>
    /// The SetCollapse method determines whether the console will collapse same message output into the same line. A state of `true` will collapse messages and `false` will print the same message for each line.
    /// </summary>
    /// <param name="state">The state of whether to collapse the output messages, true will collapse and false will not collapse.</param>
    public virtual void SetCollapse(bool state)
    {
        collapseLog = state;
    }

    /// <summary>
    /// The ClearLog method clears the current log view of all messages
    /// </summary>
    public virtual void ClearLog()
    {
        consoleOutput.text = "";
        currentBuffer = 0;
        lastMessage = "";
    }

    protected virtual void Awake()
    {
        rig = FindObjectOfType<OVRCameraRig>();
        menuOffset = transform.position; // TODO: this is unpredictable/busted

        Instance = this;

        logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Assert, assertMessage },
            { LogType.Error, errorMessage },
            { LogType.Exception, exceptionMessage },
            { LogType.Log, infoMessage },
            { LogType.Warning, warningMessage }
        };
        scrollWindow = transform.Find("Panel/Scroll View").GetComponent<ScrollRect>();
        consoleRect = transform.Find("Panel/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        consoleOutput = transform.Find("Panel/Scroll View/Viewport/Content/ConsoleOutput").GetComponent<Text>();

        consoleOutput.fontSize = fontSize;
        ClearLog();
    }

    protected virtual void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    protected virtual void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        consoleRect.sizeDelta = Vector2.zero;
    }

    protected virtual string GetMessage(string message, LogType type)
    {
        string color = ColorUtility.ToHtmlStringRGBA(logTypeColors[type]);
        return "<color=#" + color + ">" + message + "</color>" + NEWLINE;
    }

    protected virtual void HandleLog(string message, string stackTrace, LogType type)
    {
        string logOutput = GetMessage(message, type);

        if (!collapseLog || lastMessage != logOutput)
        {
            consoleOutput.text += logOutput;
            lastMessage = logOutput;
        }

        consoleRect.sizeDelta = new Vector2(consoleOutput.preferredWidth, consoleOutput.preferredHeight);
        scrollWindow.verticalNormalizedPosition = 0;
        currentBuffer++;
        if (currentBuffer >= lineBuffer)
        {
            IEnumerable<string> lines = Regex.Split(consoleOutput.text, NEWLINE).Skip(lineBuffer / 2);
            consoleOutput.text = string.Join(NEWLINE, lines.ToArray());
            currentBuffer = lineBuffer / 2;
        }
    }

    public void Show() {

        gameObject.SetActive(true);

        transform.position = rig.transform.TransformPoint(menuOffset);
        Vector3 newEulerRot = rig.transform.rotation.eulerAngles;
        newEulerRot.x = 0.0f;
        newEulerRot.z = 0.0f;
        transform.eulerAngles = newEulerRot;

    }

    public void Hide()
    {

        gameObject.SetActive(false);

    }

}
