using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.SettingsW
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        #region Binding Props
        private string _workDirectory = string.Empty;
        public string WorkDirectory
        {
            get => _workDirectory;
            set
            {
                _workDirectory = value;
                OnPropertyChanged(nameof(WorkDirectory));
            }
        }        
        #endregion

        #region Commands
        private RelayCommand? _workDirChooseDialogOpenCommand = null;
        public RelayCommand WorkDirChooseDialogOpenCommand =>
            _workDirChooseDialogOpenCommand ??=
                    new RelayCommand(obj => ChangeWorkDirectory());
        #endregion

        private readonly SettingsLoader? _settingsLoader;

        public SettingsWindowViewModel()
        {
            //Loading settings from JSON file
            if (File.Exists("Settings.json"))
            {
                _settingsLoader = SettingsLoader.FromJson(File.ReadAllText("Settings.json"));
                _settingsLoader ??= new SettingsLoader();
                _settingsLoader.UpdateSettings();
            }
            else
            {
                _settingsLoader = new SettingsLoader();
            }             
        }

        private void ChangeWorkDirectory()
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WorkDirectory = dialog.SelectedPath;
                _settingsLoader!.WorkingDirectoryPath = WorkDirectory;
            }
        }

        #region Handlers
        public void OnClose(object? sender, EventArgs e)
        {
            if (File.Exists("Settings.json"))
            {
                File.Delete("Settings.json");
            }

            File.WriteAllText("Settings.json", _settingsLoader!.ToJson());
            _settingsLoader!.UpdateSettings();
        }
        #endregion
    }
}