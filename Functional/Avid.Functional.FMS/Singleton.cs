using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avid.Functional.FMS
{
    public sealed class Singleton
    {
        private static volatile Singleton _instance = null;
        private static object synlock = new Object();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(synlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Singleton();
                        }
                    }
                }
                return (_instance);
            }
        }
    }
}
