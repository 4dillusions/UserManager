/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Application.Authentication;
using App4di.Dotnet.UserManager.Application.DependencyInjection;
using App4di.Dotnet.UserManager.Application.Export;
using App4di.Dotnet.UserManager.Application.Repositories;
using App4di.Dotnet.UserManager.Application.Users;
using App4di.Dotnet.UserManager.Domain;
using App4di.Dotnet.UserManager.Infrastructure.Common;
using App4di.Dotnet.UserManager.Infrastructure.Data;
using App4di.Dotnet.UserManager.Infrastructure.DependencyInjection;
using App4di.Dotnet.UserManager.Presentation.DependencyInjection;
using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.Services;
using App4di.Dotnet.UserManager.Presentation.Session;
using App4di.Dotnet.UserManager.Presentation.Users;
using App4di.Dotnet.UserManager.Presentation.ViewModels;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.UserManager.Tests.Configuration;

[TestClass]
[DoNotParallelize]
public class DependencyRegistrationTests
{
    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(DataFilePaths.XmlDataFilePath))
            File.Delete(DataFilePaths.XmlDataFilePath);

        if (File.Exists(DataFilePaths.JsonDataFilePath))
            File.Delete(DataFilePaths.JsonDataFilePath);
    }

    [TestMethod]
    public void ApplicationRegistrationResolvesApplicationServices()
    {
        var di = CreateDiManager(di =>
        {
            di.Bind<IUserRepository, UserRepositoryStub>(DILifetimeScopes.Singleton);
            di.Bind<IAddressCityRepository, AddressCityRepositoryStub>(DILifetimeScopes.Singleton);
            di.BindApplication();
        });

        Assert.IsNotNull(di.GetDependency<IAuthenticationService>());
        Assert.IsNotNull(di.GetDependency<IUserQueryService>());
        Assert.IsNotNull(di.GetDependency<ISaveUserUseCase>());
        Assert.IsNotNull(di.GetDependency<IDeleteUserUseCase>());
    }

    [TestMethod]
    public void PresentationRegistrationResolvesPresentationServicesAndViewModels()
    {
        var di = CreateDiManager(di =>
        {
            BindPresentationDependencies(di);
            di.BindPresentation();
        });

        Assert.IsNotNull(di.GetDependency<ISessionService>());
        Assert.IsNotNull(di.GetDependency<IUserEditSessionService>());
        Assert.IsNotNull(di.GetDependency<INavigationService>());
        Assert.IsNotNull(di.GetDependency<MainViewModel>());
        Assert.IsNotNull(di.GetDependency<LoginViewModel>());
        Assert.IsNotNull(di.GetDependency<UserViewModel>());
        Assert.IsNotNull(di.GetDependency<UserListViewModel>());
    }

    [TestMethod]
    public void InfrastructureRegistrationResolvesInfrastructureImplementations()
    {
        var di = CreateDiManager(di => di.BindInfrastructure());

        Assert.IsNotNull(di.GetDependency<XmlDataManager<UserData>>());
        Assert.IsNotNull(di.GetDependency<JsonDataManager<UserData>>());
        Assert.IsNotNull(di.GetDependency<IUserRepository>());
        Assert.IsNotNull(di.GetDependency<IAddressCityRepository>());
        Assert.IsNotNull(di.GetDependency<IUserExportService>());
    }

    [TestMethod]
    public void FullCompositionResolvesStartupViewModelsAndCoreUseCases()
    {
        Directory.CreateDirectory(DataFilePaths.DataDirectoryPath);
        new XmlDataManager<UserData>().Save(CreateUsers(), DataFilePaths.XmlDataFilePath);

        var di = CreateDiManager(di =>
        {
            di.BindApplication();
            di.BindPresentation();
            di.BindInfrastructure();
            BindWpfUiAdapters(di);
        });

        Assert.IsNotNull(di.GetDependency<MainViewModel>());
        Assert.IsNotNull(di.GetDependency<LoginViewModel>());
        Assert.IsNotNull(di.GetDependency<UserListViewModel>());
        Assert.IsNotNull(di.GetDependency<UserViewModel>());
        Assert.IsTrue(di.GetDependency<IAuthenticationService>().Authenticate("User1", "Password1"));
    }

    private static IDIManager CreateDiManager(Action<IDIManager> bindings)
    {
        IDIManager di = new DIManager();
        di.Init(() => bindings(di));
        return di;
    }

    private static void BindPresentationDependencies(IDIManager di)
    {
        di.Bind<IUserNotificationService, UserNotificationServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IApplicationService, ApplicationServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IAuthenticationService, AuthenticationServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IUserQueryService, UserQueryServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IUserExportService, UserExportServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IDeleteUserUseCase, DeleteUserUseCaseStub>(DILifetimeScopes.Singleton);
        di.Bind<ISaveUserUseCase, SaveUserUseCaseStub>(DILifetimeScopes.Singleton);
    }

    private static void BindWpfUiAdapters(IDIManager di)
    {
        di.Bind<IUserNotificationService, UserNotificationServiceStub>(DILifetimeScopes.Singleton);
        di.Bind<IApplicationService, ApplicationServiceStub>(DILifetimeScopes.Singleton);
    }

    private static List<UserData> CreateUsers()
    {
        return
        [
            new UserData
            {
                UserId = 1,
                LoginName = "User1",
                Password = "Password1",
                FirstName = "First1",
                Surname = "Surname1",
                BirthDate = BirthDateRules.MinimumBirthDate,
                BirthPlace = "BirthPlace1",
                AddressCity = "City1"
            },
            new UserData
            {
                UserId = 2,
                LoginName = "User2",
                Password = "Password2",
                FirstName = "First2",
                Surname = "Surname2",
                BirthDate = BirthDateRules.MinimumBirthDate,
                BirthPlace = "BirthPlace2",
                AddressCity = "City2"
            }
        ];
    }

    public sealed class UserRepositoryStub : IUserRepository
    {
        public IReadOnlyList<UserData> LoadUsers() => CreateUsers();

        public void SaveUsers(IEnumerable<UserData> users)
        {
        }
    }

    public sealed class AddressCityRepositoryStub : IAddressCityRepository
    {
        public IReadOnlyList<string> LoadAddressCities() => ["City1", "City2"];
    }

    public sealed class UserNotificationServiceStub : IUserNotificationService
    {
        public void ShowMessage(string message, string? title = null)
        {
        }

        public bool ShowConfirmation(string message, string? title = null) => true;
    }

    public sealed class ApplicationServiceStub : IApplicationService
    {
        public void Shutdown()
        {
        }
    }

    public sealed class AuthenticationServiceStub : IAuthenticationService
    {
        public bool Authenticate(string loginName, string password) => true;
    }

    public sealed class UserQueryServiceStub : IUserQueryService
    {
        public IReadOnlyList<UserData> GetUsers(UserQueryCriteria filter) => CreateUsers();

        public IReadOnlyList<string> GetAddressCities() => ["City1", "City2"];
    }

    public sealed class UserExportServiceStub : IUserExportService
    {
        public string ExportUsers(IEnumerable<UserData> users) => string.Empty;
    }

    public sealed class DeleteUserUseCaseStub : IDeleteUserUseCase
    {
        public bool Execute(int userId) => true;
    }

    public sealed class SaveUserUseCaseStub : ISaveUserUseCase
    {
        public void Execute(IUserSaveSession userSaveSession)
        {
        }
    }
}
