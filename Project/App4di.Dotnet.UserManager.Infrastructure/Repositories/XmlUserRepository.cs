/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Entities;

namespace App4di.Dotnet.UserManager.Infrastructure.Repositories;

public class XmlUserRepository : IUserRepository
{
    private readonly IDataManager<User> dataManager;

    public XmlUserRepository()
        : this(new XmlDataManager<User>())
    {
    }

    internal XmlUserRepository(IDataManager<User> dataManager)
    {
        this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
    }

    public List<User> LoadUsers()
    {
        EnsureDataFileExists();
        return dataManager.Load(Constants.XmlDataFilePath);
    }

    public void SaveUsers(List<User> users)
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
