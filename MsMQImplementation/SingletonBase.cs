using System;
using System.Reflection;

namespace Data
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Construct();
                }

                return _instance;
            }
        }

        private static T Construct()
        {
            Type t = typeof(T);

            ConstructorInfo constructor = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);

            return (T)constructor.Invoke(null);
        }
    }
}