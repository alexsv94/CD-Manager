using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities.Types
{
    public class DateInterval : ObservableContainer
    {
        private DateTime _startDate;
        public DateTime StartDate 
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _endDate;
        public DateTime EndDate 
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public DateInterval()
        {
            SetDefault();
        }

        public DateInterval(DateTime startDate, DateTime endDate) =>
            (StartDate, EndDate) = (startDate, endDate);

        public void SetDefault()
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.Now;
        }

        public bool Contains(DateTime? date)
        {
            if (date == null) return false;

            DateTime clampedStart = DateTime.Parse(StartDate.ToShortDateString());
            DateTime clampedEnd = DateTime.Parse(EndDate.ToShortDateString()).AddDays(1);

            return date >= clampedStart && date <= clampedEnd;
        }
    }
}
