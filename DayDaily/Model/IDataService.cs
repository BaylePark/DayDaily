using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public interface IDataService
    {
        Task LoadAsync();
    }
}
