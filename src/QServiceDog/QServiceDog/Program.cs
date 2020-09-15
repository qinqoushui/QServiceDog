using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QServiceDog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Q.DevExtreme.Tpl.ProgramHelper<Startup>().Run(nameof(QServiceDog), "·þÎñÊØ»¤¹·", args,
#if _Docker
                        true
#else
                        false
#endif
                        ,
#if DEBUG || IIS
                        true
#else
                        false
#endif
    );
        }
    }
}
