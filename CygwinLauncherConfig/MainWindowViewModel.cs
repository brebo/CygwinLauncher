using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;


using Brebo.CygwinLauncher.Base;

namespace Brebo.CygwinLauncher.Config
{
    class MainWindowViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool hasViewError;
        public bool HasViewError
        {
            get { return this.hasViewError; }
            set
            {
                this.SetProperty(ref this.hasViewError, value);
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string puttyPath;
        [Required()]
        [FileExists()]
        public string PuttyPath
        {
            get { return this.puttyPath; }
            set
            {
                this.SetProperty(ref this.puttyPath, value); 
                this.ValidateProperty(value);
            }
        }

        private string puttyParameter;
        [Required()]
        public string PuttyParameter
        {
            get { return this.puttyParameter; }
            set
            {
                this.SetProperty(ref this.puttyParameter, value);
                this.ValidateProperty(value);
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string directoryFile;
        [Required()]
        [DirectoryExists()]
        public string DirectoryFile
        {
            get { return this.directoryFile; }
            set
            {
                this.SetProperty(ref this.directoryFile, value); 
                this.ValidateProperty(value);
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private TimeSpan timeout;
        public TimeSpan Timeout
        {
            get { return this.timeout; }
            set
            {
                this.SetProperty(ref this.timeout, value);
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private int interval;
        public int Interval
        {
            get { return this.interval; }
            set
            {
                this.SetProperty(ref this.interval, value);
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.errorsContainer = new ErrorsContainer<string>(OnErrorsChanged);

            CopySettings(CygwinLauncherSettings.Load());
        }

        protected void ValidateProperty(object value, [CallerMemberName]string propertyName = null)
        {
            var context = new ValidationContext(this) { MemberName = propertyName };
            var validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateProperty(value, context, validationErrors))
            {
                var errors = validationErrors.Select(error => error.ErrorMessage);
                this.errorsContainer.SetErrors(propertyName, errors);
            }
            else
            {
                this.errorsContainer.ClearErrors(propertyName);
            }
        }

        private ErrorsContainer<string> errorsContainer;
        protected void OnErrorsChanged([CallerMemberName]string propertyName = null)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return this.errorsContainer.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return this.errorsContainer.HasErrors; }
        }

        private DelegateCommand referPuttyPathCommand;
        public DelegateCommand ReferPuttyPathCommand
        {
            get
            {
                return this.referPuttyPathCommand = this.referPuttyPathCommand ?? new DelegateCommand(ExecuteReferPuttyPathCommand);
            }
        }

        private void ExecuteReferPuttyPathCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = this.puttyPath;
            dialog.Filter = "実行可能ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                this.PuttyPath = dialog.FileName;
            }
        }

        private DelegateCommand referDirectoryFileCommand;
        public DelegateCommand ReferDirectoryFileCommand
        {
            get
            {
                return this.referDirectoryFileCommand = this.referDirectoryFileCommand ?? new DelegateCommand(ExecuteReferDirectoryFileCommand);
            }
        }

        private void ExecuteReferDirectoryFileCommand()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = this.directoryFile;
            if (dialog.ShowDialog() == true)
            {
                this.DirectoryFile = dialog.FileName;
            }
        }

        private DelegateCommand saveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                return (this.saveCommand = this.saveCommand ?? new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            CygwinLauncherSettings settings = new CygwinLauncherSettings();
            settings.PuttyPath = this.PuttyPath;
            settings.PuttyParameter = this.PuttyParameter;
            settings.DirectoryFile = this.DirectoryFile;
            settings.Timeout = this.Timeout;
            settings.Interval = this.Interval;
            settings.Save();
        }

        private bool CanExecuteSaveCommand()
        {
            return !this.hasViewError;
        }


        private DelegateCommand reloadCommand;
        public DelegateCommand ReloadCommand
        {
            get
            {
                return (this.reloadCommand = this.reloadCommand ?? new DelegateCommand(ExecuteReloadCommand));
            }
        }

        private void ExecuteReloadCommand()
        {
            CopySettings(CygwinLauncherSettings.Load());
        }

        private DelegateCommand resetCommand;
        public DelegateCommand ResetCommand
        {
            get
            {
                return (this.resetCommand = this.resetCommand ?? new DelegateCommand(ExecuteResetCommand));
            }
        }

        private void ExecuteResetCommand()
        {
            CopySettings(new CygwinLauncherSettings());
        }

        private void CopySettings(CygwinLauncherSettings settings)
        {
            this.PuttyPath = settings.PuttyPath;
            this.PuttyParameter = settings.PuttyParameter;
            this.DirectoryFile = settings.DirectoryFile;
            this.Timeout = settings.Timeout;
            this.Interval = settings.Interval;
        }
        
    }
}
