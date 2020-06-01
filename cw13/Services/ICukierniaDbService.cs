using cw13.DTOs.Request;
using cw13.DTOs.Response;
using cw13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw13.Services
{
    public interface ICukierniaDbService
    {
        public List<ZamWyr> GetOrders(string nazwisko);
        public List<ZamWyr> GetOrders();
        public ZamWyr NewOrder(int id, ZamReq zam);
    }
}
