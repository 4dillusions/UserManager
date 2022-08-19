using System;

namespace UserManager.Core.Factory
{
    public static class Ioc<TInstance>
    {
        static TInstance instance;
        static Func<TInstance> function;

        public static TInstance Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                throw new NullReferenceException();
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
}
