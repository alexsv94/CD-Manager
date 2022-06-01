using OrganizerWpf.Models;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        public NoticeCreateFormViewModel()
        {
            ExtendToList.Add(new ProductModel() { Name = "TestProduct1" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct2" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct3" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct4" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct5" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct6" });
            ExtendToList.Add(new ProductModel() { Name = "TestProduct7" });
        }

        public void OnExtendToItemMouseDown(object sender)
        {
            var item = (ProductModel)(sender as DataGridRow)!.DataContext;
            var list = new List<ProductModel>();
            list.AddRange(ExtendToList);
            list.Remove(item);
            ExtendToList = list;
        }
    }
}
