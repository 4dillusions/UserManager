/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Xml.Serialization;

namespace App4di.Dotnet.UserManager.Infrastructure.Data;

public class XmlDataManager<T> : IDataManager<T>
    where T : class
{
    private static readonly XmlSerializer serializer = new(typeof(List<T>));

    public List<T> Load(string pathAndName)
    {
        using var reader = new StreamReader(pathAndName);
        return serializer.Deserialize(reader) as List<T> ?? [];
    }

    public void Save(List<T> data, string pathAndName)
    {
        using var writer = new StreamWriter(pathAndName);
        serializer.Serialize(writer, data);
    }
}
