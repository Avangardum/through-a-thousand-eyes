using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ThroughAThousandEyes
{
    public static class TteLogger
    {
        private static bool LogToConsole = true;
        private static bool LogToFile = true;
        private static bool OverrideLog = true;
        private static string LogPath = "Log.txt";

        private static readonly string FullPath = Application.persistentDataPath + '\\' + LogPath;
        
        private static bool _hasLoggedAnything;

        public static void WriteMessage(string message)
        {
            if (LogToConsole)
            {
                Debug.Log(message);
            }

            if (LogToFile)
            {
                using (StreamWriter sw = new StreamWriter(FullPath, !OverrideLog || _hasLoggedAnything))
                {
                    DateTime time = DateTime.Now;
                    string timeString =
                        $"{time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}.{time.Millisecond:D3} F{Time.frameCount, -7}";
                    sw.WriteLine($"{timeString} {message}");
                }
            }

            _hasLoggedAnything = true;
        }

        public static void WriteMessage(object message) => WriteMessage(message.ToString());
    }
}