/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Text.Json;

namespace App4di.Dotnet.UserManager.Infrastructure.Data;

public class JsonDataManager<T> : IDataManager<T>
    where T : class
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public List<T> Load(string pathAndName)
    {
        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(pathAndName), SerializerOptions) ?? [];
    }

    public void Save(List<T> data, string pathAndName)
    {
        File.WriteAllText(pathAndName, JsonSerializer.Serialize(data, SerializerOptions));
    }
}
