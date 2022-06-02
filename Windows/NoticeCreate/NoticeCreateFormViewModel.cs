using OrganizerWpf.Dialogs.AddExtendToRowDialog;
using OrganizerWpf.Dialogs.ProductsChooseDialog;
using OrganizerWpf.Dialogs.RecentDocumentsDialog;
using OrganizerWpf.Models;
using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        private List<ProductModel> _extendToList = new();
        public List<ProductModel> ExtendToList
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

        private List<DocumentModel> _changesList = new();
        public List<DocumentModel> ChangesList
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
        #endregion
        public NoticeCreateFormViewModel()
        {
            ExtendToList.Add(new ProductModel() { Name = "TestProduct1", DecNumber = "ФИАШ.789456.587" });

            ChangesList.Add(new DocumentModel() { Name = "TestDoc1", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01")});
        }

        public void OnExtendToItemMouseDoubleClick(object sender)
        {
            var item = (ProductModel)(sender as DataGridRow)!.DataContext;
            ExtendToList = ExtendToList.Where(x => x != item).ToList();
        }

        public void OnChangesListItemMouseDoubleClick(object sender)
        {
            var item = (DocumentModel)(sender as DataGridRow)!.DataContext;
            ChangesList = ChangesList.Where(x => x != item).ToList();
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
                    ExtendToList = AddItemToList<ProductModel>(ExtendToList, newProductRow);
                }
            }
        }

        private void OpenRecentDocs()
        {
            var dialog = new RecentDocumentsDialog();
            dialog.ExcludeList.AddRange(ChangesList);

            if ((bool)dialog.ShowDialog()!)
            {
                ChangesList = AddItemCollectionToList<DocumentModel>(ChangesList, dialog.ChosenRecentDocs);
            }
        }

        private void OpenProductsChooseDialog()
        {
            var dialog = new ProductsChooseDialog();
            dialog.ExcludedProducts.AddRange(ExtendToList);

            if ((bool)dialog.ShowDialog()!)
            {
                ExtendToList = AddItemCollectionToList<ProductModel>(ExtendToList, dialog.ChosenProducts);
            }
        }

        private List<T> AddItemToList<T>(List<T> targetList, T item)
        {
            var list = new List<T>();
            list.AddRange(targetList);
            list.Add(item);
            return list;
        }

        private List<T> AddItemCollectionToList<T>(List<T> targetList, IEnumerable<T> items)
            where T : class, new()
        {
            var list = new List<T>();
            list.AddRange(targetList);
            list.AddRange(items);
            return list;
        }
    }
}
