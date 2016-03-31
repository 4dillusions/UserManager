using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace UserManager.Core.Data
{
    public class JsonDataManager<T> : IDataManager<T> 
        where T : class
    {
        public List<T> Load(string pathAndName)
        {
            return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(pathAndName));
        }

        public void Save(List<T> data, string pathAndName)
        {
            File.WriteAllText(pathAndName, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
