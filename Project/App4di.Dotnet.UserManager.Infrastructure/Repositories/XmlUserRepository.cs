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

    public List<UserData> LoadUsers()
    {
        EnsureDataFileExists();
        return dataManager.Load(Constants.XmlDataFilePath);
    }

    public void SaveUsers(List<UserData> users)
    {
        ArgumentNullException.ThrowIfNull(users);
        EnsureDataFileExists();
        dataManager.Save(users, Constants.XmlDataFilePath);
    }

    private static void EnsureDataFileExists()
    {
        if (!File.Exists(Constants.XmlDataFilePath))
            throw new FileNotFoundException();
    }
}
