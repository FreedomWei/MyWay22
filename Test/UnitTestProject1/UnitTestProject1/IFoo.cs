using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public interface IFoo
    {
        void DoSomeThing(string thingName);
        bool IsLoveFoo();
        //http://jwfzl.fzl1314.com 
        string FooName { get; set; }
    }



 }


