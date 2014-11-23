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
    public class FileExistsAttribute : ValidationAttribute
    {
        private const string ERROR_MESSAGE = "指定されたファイルが見つかりません。";

        public FileExistsAttribute()
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

            return File.Exists(filePath);
        }

    }
}
