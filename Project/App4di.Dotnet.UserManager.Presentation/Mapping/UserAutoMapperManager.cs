/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Presentation.Models;
using FW4di.Dotnet.Core.Mapping;

namespace App4di.Dotnet.UserManager.Presentation.Mapping;

public class UserAutoMapperManager : AutoMapperManager
{
    public UserAutoMapperManager()
        : base(CreateMappingList())
    {
    }

    private static MappingList CreateMappingList()
    {
        var mappingList = new MappingList();
        mappingList.AddMapping<UserData, User>();
        mappingList.AddMapping<User, UserData>();
        mappingList.AddMapping<User, User>();
        return mappingList;
    }
}
