using System.Collections.Generic;

namespace UserManager.Core.Data
{
    public interface IDataManager<T> 
        where T : class
    {
        List<T> Load(string pathAndName);
        void Save(List<T> data, string pathAndName);
    }
}
