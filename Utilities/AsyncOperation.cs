using OrganizerWpf.ViewModels;
using OrganizerWpf.Windows.OperationProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities
{
    public class AsyncOperation : ViewModelBase
    {
        private int _totalStepsCount;
        public int TotalStepsCount {
            get => _totalStepsCount;
            set
            {
                _totalStepsCount = value;
                OnPropertyChanged(nameof(TotalStepsCount));
            }
        }
        
        private int _completedStepsCount;
        public int CompletedStepsCount
        {
            get => _completedStepsCount;
            set
            {
                _completedStepsCount = value;
                OnPropertyChanged(nameof(CompletedStepsCount));
                OnPropertyChanged(nameof(Progress));
            }
        }

        public int Progress => (int)((double)CompletedStepsCount / (double)TotalStepsCount * 100);

        public AsyncOperation(bool showProgressWindow, string name = "")
        {
            if (showProgressWindow)
                OperationProgressWindow.ShowProgressWindow(this, name);
        }
    }
}
