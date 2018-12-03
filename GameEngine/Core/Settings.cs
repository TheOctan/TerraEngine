using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    public class Settings
    {
        public int Music { get; set; }
        public int Value { get; set; }

        public bool FullScreen { get; set; }

        public string NickName1 { get; set; }
        public string NickName2 { get; set; }

        public Settings()
        {
            
        }
    }
}
