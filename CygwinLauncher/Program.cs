﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using CommandLine;

using Brebo.CygwinLauncher.Base;

namespace Brebo.CygwinLauncher.Launcher
{
    static class Program
    {
        private const int REMOVE_TIME = 3000;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // CygwinLauncher.exe -d "C:\" のように実行すると、args[2] = "C:\\\"" のように
            // " が入った状態でパースされるMicrosoftのひどい仕様に対処する。
            //string[] args = Environment.GetCommandLineArgs();

            string[] args = SplitCommandLineArgs(Environment.CommandLine);

            CommandLineOptions options = new CommandLineOptions();
            if (!Parser.Default.ParseArguments(args, options))
            {
                MessageBox.Show("コマンドライン引数が不正です。", "CygwinLauncher - エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string directory = null;
            string commandFilePath = null;

            if (options.Command != null)
            {
                directory = Path.GetDirectoryName(options.Command);

                commandFilePath = Path.GetTempFileName();
                File.WriteAllText(commandFilePath, GetTemplate(directory, options.Command));
            }
            else
            {
                directory = options.Directory ?? Directory.GetCurrentDirectory();
            }

            bool done = false;

            DateTime startTime = DateTime.Now;

            CygwinLauncherSettings settings = CygwinLauncherSettings.Load();

            do
            {
                try
                {
                    using (FileStream fileStream = new FileStream(settings.DirectoryFile, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(directory);
                    }
                    done = true;
                }
                catch
                {
                    if (DateTime.Now - startTime > settings.Timeout)
                    {
                        try
                        {
                            File.Delete(settings.DirectoryFile);
                        }
                        catch { }
                    }
                    Thread.Sleep(settings.Interval);
                }
            }
            while (!done);
                
            string arguments = settings.PuttyParameter;
            if (options.Command != null)
            {
                arguments += " -m \"" + commandFilePath + "\" -t";
            }

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = settings.PuttyPath;
                processStartInfo.Arguments = arguments;
                Process process = Process.Start(processStartInfo);

                Thread.Sleep(REMOVE_TIME);
                File.Delete(commandFilePath);
            }
            catch { }
        }

        static string[] SplitCommandLineArgs(string argsStr)
        {
            List<string> args = new List<string>();
            StringBuilder buf = new StringBuilder();
            StringBuilder arg = new StringBuilder();

            bool quoted = false;

            foreach (char ch in argsStr)
            {
                switch (ch)
                {
                    case '"':
                        if (quoted)
                        {
                            args.Add(arg.ToString());
                            arg.Clear();
                            quoted = false;
                        }
                        else
                        {
                            quoted = true;
                        }
                        break;

                    case ' ':
                        if (quoted)
                        {
                            arg.Append(ch);
                        }
                        else if (arg.Length > 0)
                        {
                            args.Add(arg.ToString());
                            arg.Clear();
                        }
                        break;

                    default:
                        arg.Append(ch);
                        break;
                }
            }

            if (arg.Length > 0)
            {
                args.Add(arg.ToString());
            }

            return args.ToArray();
        }

        static string GetTemplate(string directory, string command)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string commandData = File.ReadAllText(Path.Combine(Path.GetDirectoryName(assembly.Location), "CommandTemplate.sh"));
            commandData = commandData.Replace("${directory}", directory);
            commandData = commandData.Replace("${filePath}", command);

            return commandData;
        }
    }
}
