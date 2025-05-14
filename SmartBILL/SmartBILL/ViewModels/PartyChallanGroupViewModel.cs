using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBILL.Models;

namespace SmartBILL.ViewModels
{
    public class PartyChallanGroupViewModel
    {
        public int PartyChId { get; set; }
        public string PartyChallanNo { get; set; }
        public DateTime PartyDate { get; set; }
        public string PartyName { get; set; }

        public ObservableCollection<PartyChallanItem> PartyChallanItems { get; set; }

        public PartyChallanGroupViewModel()
        {
            PartyChallanItems = new ObservableCollection<PartyChallanItem>();
        }
        
    }
}
