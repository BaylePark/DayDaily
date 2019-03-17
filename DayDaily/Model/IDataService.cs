using DayDaily.Model.VO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public interface IDataService
    {
        Task LoadAsync();
        IList<UserInfo> GetAllUserInfos();
        DeveloperInfo GetCurrentDeveloperInfo();
        void SetCurrentDeveloper(string name);
        string GetWelcomeMessage();
    }
}
