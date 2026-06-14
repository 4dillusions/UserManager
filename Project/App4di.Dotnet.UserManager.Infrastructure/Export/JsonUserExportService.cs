/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;

namespace App4di.Dotnet.UserManager.Infrastructure.Export;

public class JsonUserExportService : IUserExportService
{
    private readonly IDataManager<UserData> dataManager;

    public JsonUserExportService()
        : this(new JsonDataManager<UserData>())
    {
    }

    internal JsonUserExportService(IDataManager<UserData> dataManager)
    {
        this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
    }

    public bool ExportUsers(List<UserData> users)
    {
        ArgumentNullException.ThrowIfNull(users);

        if (File.Exists(Constants.JsonDataFilePath))
            File.Delete(Constants.JsonDataFilePath);

        dataManager.Save(users, Constants.JsonDataFilePath);
        return File.Exists(Constants.JsonDataFilePath);
    }
}
