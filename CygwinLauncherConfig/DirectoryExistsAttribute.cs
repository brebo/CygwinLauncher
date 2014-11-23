using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brebo.CygwinLauncher.Config
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DirectoryExistsAttribute : ValidationAttribute
    {
        private const string ERROR_MESSAGE = "指定されたファイルが見つかりません。";

        public DirectoryExistsAttribute()
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return ERROR_MESSAGE;
        }

        public override bool IsValid(object value)
        {
            string filePath = value as string;
            if (filePath == null) { return true; }
            if (filePath.EndsWith(@"\") || filePath.EndsWith("/")) { return false; }

            string directory;
            try
            {
                if (Directory.Exists(filePath)) { return false; }
                directory = Path.GetDirectoryName(filePath);
                return Directory.Exists(directory);
            }
            catch { return false; }

        }

    }
}
