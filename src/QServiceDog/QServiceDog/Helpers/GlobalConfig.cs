using AutoMapper;

namespace QServiceDog.Helpers
{
    public class GlobalConfig
    {
        static GlobalConfig _ = new GlobalConfig();
        public static GlobalConfig Instance { get; } = _;
        public IMapper Mapper { get; set; }
 
        public string Client { get; set; }

        public static readonly string Cloud = "CLOUD";
    }
}
