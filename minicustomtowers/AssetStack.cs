using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This code has been modified from 1330 Studio's BTD6E Modules. Huge thanks to them for letting me use this code.

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
