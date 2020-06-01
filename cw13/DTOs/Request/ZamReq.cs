using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw13.DTOs.Request
{
    public class ZamReq
    {
        public DateTime dataPrzyjecia { get; set; }
        public string Uwagi { get; set; }
        public List<WyrobReq> Wyroby { get; set; }
    }
}
