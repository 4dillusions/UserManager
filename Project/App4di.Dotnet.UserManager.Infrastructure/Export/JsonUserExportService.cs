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

    public JsonUserExportService(JsonDataManager<UserData> dataManager)
    {
        this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
    }

    public string ExportUsers(IEnumerable<UserData> users)
    {
        ArgumentNullException.ThrowIfNull(users);

        var temporaryFilePath = DataFilePaths.CreateTemporaryFilePath(DataFilePaths.JsonDataFilePath);

        try
        {
            dataManager.Save(users.ToList(), temporaryFilePath);
            File.Move(temporaryFilePath, DataFilePaths.JsonDataFilePath, overwrite: true);
            return Path.GetFullPath(DataFilePaths.JsonDataFilePath);
        }
        finally
        {
            TryDeleteTemporaryFile(temporaryFilePath);
        }
    }

    private static void TryDeleteTemporaryFile(string filePath)
    {
        try
        {
            File.Delete(filePath);
        }
        catch
        {
            // Best-effort cleanup must not mask an export failure.
        }
    }
}
