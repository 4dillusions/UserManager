using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace UserManager.Core.Data
{
    public class XmlDataManager<T> : IDataManager<T> 
        where T : class
    {
        public List<T> Load(string pathAndName)
        {
            List<T> result = null;

            using (var reader = new StreamReader(pathAndName))
                result = new XmlSerializer(typeof(List<T>)).Deserialize(reader) as List<T>;

            return result;
        }

        public void Save(List<T> data, string pathAndName)
        {
            using (var writer = new StreamWriter(pathAndName))
                new XmlSerializer(typeof(List<T>)).Serialize(writer, data);
        }
    }
}
