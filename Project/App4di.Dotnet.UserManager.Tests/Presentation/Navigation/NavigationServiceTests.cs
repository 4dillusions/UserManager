/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using App4di.Dotnet.UserManager.Presentation.Navigation;
using App4di.Dotnet.UserManager.Presentation.ViewModels;

namespace App4di.Dotnet.UserManager.Tests.Presentation.Navigation;

[TestClass]
public class NavigationServiceTests
{
    [TestMethod]
    public void MainViewModelReflectsNavigationServiceState()
    {
        var navigationService = new NavigationService();
        var mainViewModel = new MainViewModel(navigationService);
        string? changedProperty = null;
        mainViewModel.PropertyChanged += (_, args) => changedProperty = args.PropertyName;

        Assert.AreEqual(ViewType.Login, mainViewModel.ViewType);

        navigationService.Navigate(ViewType.UserList);

        Assert.AreEqual(ViewType.UserList, mainViewModel.ViewType);
        Assert.AreEqual(nameof(MainViewModel.ViewType), changedProperty);
    }
}
