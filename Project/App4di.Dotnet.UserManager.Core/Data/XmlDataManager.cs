/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.IO;
using System.Xml.Serialization;

namespace App4di.Dotnet.UserManager.Core.Data;

public class XmlDataManager<T> : IDataManager<T>
    where T : class
{
    public List<T> Load(string pathAndName)
    {
        using var reader = new StreamReader(pathAndName);
        return new XmlSerializer(typeof(List<T>)).Deserialize(reader) as List<T> ?? [];
    }

    public void Save(List<T> data, string pathAndName)
    {
        using var writer = new StreamWriter(pathAndName);
        new XmlSerializer(typeof(List<T>)).Serialize(writer, data);
    }
}
