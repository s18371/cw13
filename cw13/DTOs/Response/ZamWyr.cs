using cw13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw13.DTOs.Response
{
    public class ZamWyr
    {
        public Zamowienie Zam{ get; set; }
        public List<string> Wyr { get; set; }
    }
}
