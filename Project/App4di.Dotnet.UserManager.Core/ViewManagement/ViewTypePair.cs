/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

using System.Windows;

namespace App4di.Dotnet.UserManager.Core.ViewManagement;

public class ViewTypePair
{
    public Type ViewType { get; private set; }
    public Type ViewModelType { get; private set; }
    public FrameworkElement? CachedView { get; set; }

    public ViewTypePair(Type viewType, Type viewModelType)
    {
        this.ViewType = viewType;
        this.ViewModelType = viewModelType;
    }
}
