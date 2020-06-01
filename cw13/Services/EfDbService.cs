using cw13.DTOs.Request;
using cw13.DTOs.Response;
using cw13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw13.Services
{
    public class EfDbService : ICukierniaDbService
    {
        public CukierniaDbContext _context { get; set; }
        public EfDbService(CukierniaDbContext context)
        {
            _context = context;
        }
        public List<ZamWyr> GetOrders(string nazwisko)
        {
            var listaOdp = new List<ZamWyr>();
            try
            {
                if((_context.Klienci.Any(e => e.Nazwisko.Equals(nazwisko))))
                {
                    
                    var lista = _context.Zamowienia.Where(e => e.Klient.Nazwisko.Equals(nazwisko)).ToList();
                    foreach (Zamowienie zam in lista)
                    {
                        var id = _context.Zamowienie_WyrobyCukiernicze.Where(e => e.IdZamowienia == zam.IdZamowienia).Select(e => e.IdWyrobuCukierniczego).ToList();
                        var list2 = new List<string>();
                        foreach (int Id in id)
                        {
                            list2.Add(_context.WyrobCukiernicze.Where(e => e.IdWyrobuCukierniczego == Id).Select(e => e.Nazwa).FirstOrDefault());
                        }
                        var odp = new ZamWyr { Zam = zam, Wyr = list2 };
                        listaOdp.Add(odp);
                    }
                    return listaOdp;

                }
                else
                {
                    Zamowienie zamo = new Zamowienie { Uwagi = "Error" };
                    var odp = new ZamWyr { Zam = zamo, Wyr = null };
                    listaOdp.Add(odp);
                    return listaOdp;
                }
                
            }
            catch(Exception e)
            {
                Zamowienie zamo = new Zamowienie { Uwagi = "Baza" };
                var odp = new ZamWyr { Zam = zamo, Wyr = null };
                listaOdp.Add(odp);
                return listaOdp;
            }
            
        }

        public List<ZamWyr> GetOrders()
        {
            var listaOdp = new List<ZamWyr>();
            try
            {
                var lista = _context.Zamowienia.ToList();
                foreach(Zamowienie zam in lista){
                    var id = _context.Zamowienie_WyrobyCukiernicze.Where(e => e.IdZamowienia == zam.IdZamowienia).Select(e => e.IdWyrobuCukierniczego).ToList();
                    var list2 = new List<string>();
                    foreach (int Id in id)
                    {
                        list2.Add(_context.WyrobCukiernicze.Where(e => e.IdWyrobuCukierniczego == Id).Select(e => e.Nazwa).FirstOrDefault());
                    }
                    var odp = new ZamWyr { Zam = zam, Wyr = list2 };
                    listaOdp.Add(odp);
                }
                return listaOdp;
            }catch(Exception e)
            {

                Zamowienie zamo = new Zamowienie { Uwagi = "Baza" };
                var odp = new ZamWyr { Zam = zamo, Wyr = null };
                listaOdp.Add(odp);
                return listaOdp;
            }
        }

        public ZamWyr NewOrder(int id, ZamReq zam)
        {
            if(!(_context.Klienci.Any(e => e.IdKlient == id)))
            {
                var zamow = new Zamowienie
                {

                    Uwagi = "klient brak"
                };
                var zamowie = new ZamWyr { Zam = zamow };
                return zamowie;
            }
            foreach(WyrobReq w in zam.Wyroby)
            {
                if (!(_context.WyrobCukiernicze.Any(wyrob => wyrob.Nazwa == w.Wyrob)))
                {
                    var zamow = new Zamowienie
                    {

                        Uwagi = "brak"
                    };
                    var zamowie = new ZamWyr { Zam = zamow };
                    return zamowie;
                }
            }
            try 
            {
                var zamowienieNowe = new Zamowienie { IdPracownik = 1, DataPrzyjecia = zam.dataPrzyjecia, IdKlient = id, Uwagi = zam.Uwagi, Zamowienie_WyrobCukiernicze = new List<Zamowienie_WyrobCukierniczy>() };
                var wyrobyNazwy = new List<string>();
                foreach (WyrobReq wyr in zam.Wyroby)
                {
                    int Id = _context.WyrobCukiernicze.FirstOrDefault(wyrob => wyrob.Nazwa == wyr.Wyrob).IdWyrobuCukierniczego;
                    zamowienieNowe.Zamowienie_WyrobCukiernicze.Add(new Zamowienie_WyrobCukierniczy { IdWyrobuCukierniczego = Id, Uwagi = wyr.Uwagi, Ilosc = wyr.Ilosc });
                    wyrobyNazwy.Add(wyr.Wyrob);
                }

                var zamowie = new ZamWyr { Zam = zamowienieNowe,Wyr=wyrobyNazwy};
                _context.Add(zamowienieNowe);
                _context.SaveChanges();

                return zamowie;
            }catch(Exception e)
            {
                var zamow = new Zamowienie
                {
                    Uwagi = "error"
                };
                var zamowie = new ZamWyr { Zam = zamow };
                return zamowie;
            }
            
        }
    }
}
