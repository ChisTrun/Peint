using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Paint.Services
{
    public class DllReader<T>
    {
        public static List<T> GetAbilities(string folder) {
            var abilities = new List<T>();
            var fis = (new DirectoryInfo(folder)).GetFiles("*.dll");
            foreach (var fi in fis)
            {
                var assembly = Assembly.LoadFrom(fi.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass && (!type.IsAbstract))
                    {
                        if (typeof(T).IsAssignableFrom(type))
                        {
                            var item = (T)Activator.CreateInstance(type)!;
                            abilities.Add(item!);
                        }
                    }
                }
            }
            return abilities;
        }
}
}
