using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

[InitializeOnLoad]
public class ConsoleMessageCopier
{
    private static List<LogEntry> logEntries = new List<LogEntry>();

    static ConsoleMessageCopier()
    {
        Application.logMessageReceived += LogMessageReceived;
    }

    private static void LogMessageReceived(string condition, string stackTrace, LogType type)
    {
        logEntries.Add(new LogEntry
        {
            Message = condition,
            StackTrace = stackTrace,
            Type = type
        });
    }

    [MenuItem("Tools/Console Message Copier/Copy Info Messages", false, 1)]
    private static void CopyInfoMessages()
    {
        CopyMessages(LogType.Log);
    }

    [MenuItem("Tools/Console Message Copier/Copy Warning Messages", false, 2)]
    private static void CopyWarningMessages()
    {
        CopyMessages(LogType.Warning);
    }

    [MenuItem("Tools/Console Message Copier/Copy Error Messages", false, 3)]
    private static void CopyErrorMessages()
    {
        CopyMessages(LogType.Error);
    }

    [MenuItem("Tools/Console Message Copier/Copy All Messages", false, 4)]
    private static void CopyAllMessages()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var entry in logEntries)
        {
            sb.AppendLine(entry.Message);
            sb.AppendLine(entry.StackTrace);
            sb.AppendLine();
        }

        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        Debug.Log("All messages copied to clipboard!");
    }

    private static void CopyMessages(LogType type)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var entry in logEntries)
        {
            if (entry.Type == type || (type == LogType.Error && entry.Type == LogType.Exception))
            {
                sb.AppendLine(entry.Message);
                sb.AppendLine(entry.StackTrace);
                sb.AppendLine();
            }
        }

        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        Debug.Log(type.ToString() + " messages copied to clipboard!");
    }

    private class LogEntry
    {
        public string Message;
        public string StackTrace;
        public LogType Type;
    }
}
