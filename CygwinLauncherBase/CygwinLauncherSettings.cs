using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Brebo.CygwinLauncher.Base
{
    public class CygwinLauncherSettings
    {
        private const string DIR = @"Brebo\CygwinLauncher";
        private const string FILE_NAME = "CygwinLauncher.xml";

        public string PuttyPath { get; set; }
        public string PuttyParameter { get; set; }
        public string DirectoryFile { get; set; }
        public TimeSpan Timeout { get; set; }
        public int Interval { get; set; }

        public CygwinLauncherSettings()
        {
            this.PuttyPath = @"";
            this.PuttyParameter = "";
            this.DirectoryFile = "";
            this.Timeout = new TimeSpan(0, 0, 3);
            this.Interval = 100;
        }

        private static string GetSettingsFilePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, DIR, FILE_NAME);
            return path;
        }

        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.NewLineHandling = NewLineHandling.Entitize;

            string filePath = GetSettingsFilePath();
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (XmlWriter writer = XmlTextWriter.Create(stream, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CygwinLauncherSettings));
                serializer.Serialize(writer, this);
            }
        }

        public static CygwinLauncherSettings Load()
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                using (FileStream stream = new FileStream(GetSettingsFilePath(), FileMode.Open, FileAccess.Read, FileShare.Read))
                using (XmlReader reader = XmlTextReader.Create(stream))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CygwinLauncherSettings));
                    return (CygwinLauncherSettings)serializer.Deserialize(reader);
                }
            }
            catch
            {
                return new CygwinLauncherSettings();
            }
        }
    }
}
