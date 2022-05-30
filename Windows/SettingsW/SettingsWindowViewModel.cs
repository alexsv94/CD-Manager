using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.SettingsW
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        #region Binding Props
        private string _workingDirectoryPath = string.Empty;
        public string WorkingDirectoryPath
        {
            get => _workingDirectoryPath;
            set
            {
                _workingDirectoryPath = value;
                OnPropertyChanged(nameof(WorkingDirectoryPath));
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
            _settingsLoader = SettingsLoader.Load();
            SetBindingValues();
        }

        private void SetBindingValues()
        {
            foreach (PropertyInfo property in typeof(SettingsLoader).GetProperties())
            {
                if (!GetType().GetProperties().Any(x => x.Name == property.Name))
                    continue;
                
                GetType().GetProperty(property.Name)!
                        .SetValue(this, property.GetValue(_settingsLoader));
            }
        }

        public void ChangeWorkDirectory()
        {
            using var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WorkingDirectoryPath = dialog.SelectedPath;
                _settingsLoader!.WorkingDirectoryPath = WorkingDirectoryPath;
            }
        }

        #region Handlers
        public void OnClose(CancelEventArgs e)
        {
            var result = SCMessageBox.ShowMsgBox("Сохранить изменения? " +
                "Если вы нажмете 'Нет', все изменения откатятся.",
                "Менеджер КД",
                MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case SCMessageBoxResult.Yes:
                    {
                        _settingsLoader!.Save();
                    }
                    break;
                case SCMessageBoxResult.Cancel:
                    {
                        e.Cancel = true;
                    }
                    break;
            }            
        }
        #endregion
    }
}