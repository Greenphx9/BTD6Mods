using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minicustomtowers
{
    public class AssetStack<T> : Stack<T>
    {
        public void PushAll(params T[] t)
        {
            for (var i = 0; i < t.Length; i++) Push(t[i]);
        }
    }
}
