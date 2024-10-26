using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public static class ListExtension
    {
        public static void Resize<T>(this List<T> list, int size, T c)
        {
            int cur = list.Count;

            if (size < cur)
            {
                list.RemoveRange(size, cur - size);
            }
            else if (size > cur)
            {
                if (size > list.Capacity)
                {
                    list.Capacity = size;
                }
                list.AddRange(Enumerable.Repeat(c, size - cur).ToList());
            }
        }

        public static void Resize<T>(this List<T> list, int sz) where T : new()
        {
            Resize(list, sz, new T());
        }
    }
}
