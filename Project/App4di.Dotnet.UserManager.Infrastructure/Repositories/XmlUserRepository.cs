/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Entities;
using App4di.Dotnet.UserManager.Infrastructure.Mapping;

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

    public List<User> LoadUsers()
    {
        EnsureDataFileExists();
        return dataManager.Load(Constants.XmlDataFilePath)
            .Select(UserMapper.ToUser)
            .ToList();
    }

    public void SaveUsers(List<User> users)
    {
        ArgumentNullException.ThrowIfNull(users);
        EnsureDataFileExists();
        dataManager.Save(users.Select(UserMapper.ToUserData).ToList(), Constants.XmlDataFilePath);
    }

    private static void EnsureDataFileExists()
    {
        if (!File.Exists(Constants.XmlDataFilePath))
            throw new FileNotFoundException();
    }
}
