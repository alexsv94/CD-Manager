using OrganizerWpf.Dialogs.AddChangesRowDialog;
using OrganizerWpf.Dialogs.AddExtendToRowDialog;
using OrganizerWpf.Dialogs.ProductsChooseDialog;
using OrganizerWpf.Dialogs.RecentDocumentsDialog;
using OrganizerWpf.Models;
using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.NoticeCreate
{
    public class NoticeCreateFormViewModel : ViewModelBase
    {
        #region Binding Props
        private string _decNumber = "ФИАШ.";
        public string DecNumber
        {
            get => _decNumber;
            set
            {
                _decNumber = value;
                OnPropertyChanged(nameof(DecNumber));
            }
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        private string _changeReason = "Улучшение";
        public string ChangeReason
        {
            get => _changeReason;
            set
            {
                _changeReason = value;
                OnPropertyChanged(nameof(ChangeReason));
            }
        }

        private string _basisForRelease = "Изменения, инициируемые КБ";
        public string BasisForRelease
        {
            get => _basisForRelease;
            set
            {
                _basisForRelease = value;
                OnPropertyChanged(nameof(BasisForRelease));
            }
        }

        private string _backlog = "Задел использовать";
        public string Backlog
        {
            get => _backlog;
            set
            {
                _backlog = value;
                OnPropertyChanged(nameof(Backlog));
            }
        }

        private ObservableCollection<ProductModel> _extendToList = new();
        public ObservableCollection<ProductModel> ExtendToList
        {
            get => _extendToList;
            set
            {
                _extendToList = value;
                OnPropertyChanged(nameof(ExtendToList));
            }
        }

        private string _author = "Савчук А.";
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private int _filesCount = 0;
        public int FilesCount
        {
            get => _filesCount;
            set
            {
                _filesCount = value;
                OnPropertyChanged(nameof(FilesCount));
            }
        }

        private string _changesSummary = "Корректировка КД";
        public string ChangesSummary
        {
            get => _changesSummary;
            set
            {
                _changesSummary = value;
                OnPropertyChanged(nameof(ChangesSummary));
            }
        }

        private ObservableCollection<DocumentModel> _changesList = new();
        public ObservableCollection<DocumentModel> ChangesList
        {
            get => _changesList;
            set
            {
                _changesList = value;
                OnPropertyChanged(nameof(ChangesList));
            }
        }
        #endregion

        #region Commands
        private RelayCommand? _openRecentDocsCommand = null;
        public RelayCommand OpenRecentDocsCommand =>
            _openRecentDocsCommand ??= new RelayCommand(obj => OpenRecentDocs());

        private RelayCommand? _addExtendToRowCommand = null;
        public RelayCommand AddExtendToRowCommand =>
            _addExtendToRowCommand ??= new RelayCommand(obj => AddExtendToRow());

        private RelayCommand? _openProductsChooseDialogCommand = null;
        public RelayCommand OpenProductsChooseDialogCommand =>
            _openProductsChooseDialogCommand ?? new RelayCommand(obj => OpenProductsChooseDialog());

        private RelayCommand? _addChangesRowCommand = null;
        public RelayCommand AddChangesRowCommand =>
            _addChangesRowCommand ?? new RelayCommand(obj => AddChangesRow());

        private RelayCommand? _createNoticeCommand = null;
        public RelayCommand? CreateNoticeCommand =>
            _createNoticeCommand ??= new RelayCommand(obj => CreateNotice());
        #endregion
        public NoticeCreateFormViewModel()
        {

        }

        public void OnExtendToItemMouseDoubleClick(object sender)
        {
            var item = (ProductModel)(sender as DataGridRow)!.DataContext;
            ExtendToList.Remove(item);
        }

        public void OnChangesListItemMouseDoubleClick(object sender)
        {
            var item = (DocumentModel)(sender as DataGridRow)!.DataContext;
            ChangesList.Remove(item);
        }

        private void AddExtendToRow()
        {
            var dialog = new AddExtendToRowDialog();

            if ((bool)dialog.ShowDialog()!)
            {
                var newProductRow = new ProductModel() 
                { 
                    Name = dialog.ProductName, 
                    DecNumber = dialog.DecNumber 
                };

                if (ExtendToList.Any(x => x.Name == dialog.ProductName))
                {
                    SCMessageBox.ShowMsgBox("Изделие уже есть в списке", "Строка не добавлена");
                }
                else
                {
                    ExtendToList.Add(newProductRow);
                }
            }
        }

        private void AddChangesRow()
        {
            var dialog = new AddChangesRowDialog();

            if ((bool)dialog.ShowDialog()!)
            {
                var newChangesRow = new DocumentModel()
                {
                    Name = dialog.DocName,
                    VersionHistory = new()
                    {
                        new VersionModel() { Version = dialog.NewVersion },
                        new VersionModel() { Version = dialog.OldVersion }
                    }
                };

                if (ChangesList.Any(x => x.Name == dialog.DocName))
                {
                    SCMessageBox.ShowMsgBox("Документ уже есть в списке", "Строка не добавлена");
                }
                else
                {
                    ChangesList.Add(newChangesRow);
                }
            }
        }

        private void OpenRecentDocs()
        {
            var dialog = new RecentDocumentsDialog();
            dialog.ExcludeList.AddRange(ChangesList);

            if ((bool)dialog.ShowDialog()!)
            {
                ChangesList.AddRange(dialog.ChosenRecentDocs);
            }
        }

        private void OpenProductsChooseDialog()
        {
            var dialog = new ProductsChooseDialog();
            dialog.ExcludedProducts.AddRange(ExtendToList);

            if ((bool)dialog.ShowDialog()!)
            {
                ExtendToList.AddRange(dialog.ChosenProducts);
            }
        }

        private void CreateNotice()
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Документы Word (*.doc, *.docx)|*.doc, *.docx|Все файлы (*.*)|*.*";
            dialog.DefaultExt = ".docx";
            dialog.SupportMultiDottedExtensions = true;

            string fileName = $"Извещение {DecNumber}.docx";
            
            if (!string.IsNullOrWhiteSpace(Settings.CurrentProductDirectoryPath))
            {
                dialog.FileName = Path.Combine(Settings.CurrentProductDirectoryPath, fileName);
            }
            else
            {
                dialog.FileName = Path.Combine("C:", "Users", Environment.UserName, "Desktop", fileName);
            }

            if (dialog.ShowDialog() != DialogResult.OK) return;

            string path = dialog.FileName;

            var items = new Dictionary<string, string>
            {
                { "<DEC_NUM>", DecNumber },
                { "<CREATION_DATE>", CreationDate.ToShortDateString()},
                { "<CHANGE_REASON>", ChangeReason},
                { "<BASIS_FOR_RELEASE>", BasisForRelease},
                { "<BACKLOG_NOTICE>", Backlog},
                { "<CHANGES_SUMMARY>", ChangesSummary},
                { "<CHANGES>", CombineChanges()},
                { "<EXTEND_TO>", CombineEtendTo()},
                { "<AUTHOR>", Author},
                { "<FILES_COUNT>", ChangesList.Count.ToString()}
            };

            var helper = new WordHelper(Environment.CurrentDirectory + "\\Templates\\notice_template.docx");
            helper.Process(items, path);
        }

        private string CombineChanges()
        {
            string result = string.Empty;

            foreach (var item in ChangesList)
            {
                result += $"Документ {item.ShortName} версии {item.PreviousVersion!.Version} заменить на версию {item.Version.Version}.\r";
            }

            return result;
        }

        private string CombineEtendTo()
        {
            var result = string.Empty;

            foreach (var item in ExtendToList)
            {
                result += item.Name + " " + item.DecNumber + "\r";
            }

            return result;
        }
    }
}
