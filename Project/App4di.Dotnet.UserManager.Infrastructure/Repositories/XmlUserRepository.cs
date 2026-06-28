/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;

namespace App4di.Dotnet.UserManager.Infrastructure.Repositories;

public class XmlUserRepository : IUserRepository
{
    private readonly IDataManager<UserData> dataManager;

    public XmlUserRepository()
        : this(new XmlDataManager<UserData>())
    {
    }

    internal XmlUserRepository(IDataManager<UserData> dataManager)
    {
        this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
    }

    public IReadOnlyList<UserData> LoadUsers()
    {
        EnsureDataFileExists();
        return dataManager.Load(DataFilePaths.XmlDataFilePath);
    }

    public void SaveUsers(IEnumerable<UserData> users)
    {
        ArgumentNullException.ThrowIfNull(users);
        EnsureDataFileExists();
        dataManager.Save(users.ToList(), DataFilePaths.XmlDataFilePath);
    }

    private static void EnsureDataFileExists()
    {
        if (!File.Exists(DataFilePaths.XmlDataFilePath))
            throw new FileNotFoundException();
    }
}
