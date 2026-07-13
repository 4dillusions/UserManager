/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using FW4di.Dotnet.Core.IO;

namespace App4di.Dotnet.UserManager.Infrastructure.Data;

public class XmlDataManager<T> : IDataManager<T>
    where T : class
{
    public List<T> Load(string pathAndName)
    {
        return XmlHelper.DeserializeFromFile<List<T>>(pathAndName);
    }

    public void Save(List<T> data, string pathAndName)
    {
        XmlHelper.SerializeToFile(data, pathAndName);
    }
}
