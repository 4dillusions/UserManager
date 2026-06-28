/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Infrastructure.Common;

public static class DataFilePaths
{
    public const string DataDirectoryPath = "Data";
    public static readonly string XmlDataFilePath = Path.Combine(DataDirectoryPath, "data.xml");
    public static readonly string JsonDataFilePath = Path.Combine(DataDirectoryPath, "data.json");

    public static string CreateTemporaryFilePath(string destinationFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationFilePath);

        var directoryPath = Path.GetDirectoryName(destinationFilePath);
        var temporaryFileName = $".{Path.GetFileName(destinationFilePath)}.{Guid.NewGuid():N}.tmp";

        return string.IsNullOrEmpty(directoryPath)
            ? temporaryFileName
            : Path.Combine(directoryPath, temporaryFileName);
    }
}
