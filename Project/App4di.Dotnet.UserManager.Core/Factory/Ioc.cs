/*
4di .NET UserManager application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/

namespace App4di.Dotnet.UserManager.Core.Factory;

public static class Ioc<TInstance>
{
    private static TInstance? instance;
    private static Func<TInstance>? function;

    public static TInstance Instance
    {
        get
        {
            if (instance != null)
                return instance;

            throw new InvalidOperationException("No instance has been created for the requested type.");
        }
    }

    public static void Register<TType>()
        where TType : TInstance, new()
    {
        if (function == null)
            function = () => new TType();
    }

    public static void Register<TType>(Func<TInstance> function)
        where TType : TInstance
    {
        if (Ioc<TInstance>.function == null)
            Ioc<TInstance>.function = function;
    }

    public static void Create()
    {
        if (function != null && instance == null)
            instance = function();
    }
}
