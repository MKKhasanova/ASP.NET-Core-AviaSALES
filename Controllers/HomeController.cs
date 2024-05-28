using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using Онлайн_билеты.Models;
using System;

namespace Онлайн_билеты.Controllers
{
    public class HomeController : Controller
    {
        BiletsContext db;

        public string NULL { get; private set; }

        public HomeController(BiletsContext context)
        {
            db = context;
        }

        public ActionResult Index(int? OtkudaId, int? KudaId)
        {
            IQueryable<Tickets> tickets = db.Ticket.Include(x => x.otkuda) //
                                                .Include(x => x.kuda);

            if (OtkudaId != null && OtkudaId != 0)
            {
                tickets = tickets.Where(x => x.OtkudaId == OtkudaId);
            }

            if (KudaId != null && KudaId != 0)
            {
                tickets = tickets.Where(x => x.KudaId == KudaId);
            }

            // Передаем список товаров в представление через ViewBag
            ViewBag.Ticket = tickets.ToList();

            //====================================================
            // Формируем список типов устройств
            //====================================================
            var genList = db.Otkudas.ToList();

           
            genList.Insert(0, new Otkuda { Id = 0, Otkud = "Откуда" });
            SelectList otkudas = new SelectList(genList, "Id", "Otkud", OtkudaId);

            // Передаем список устройств в представление через ViewBag
            ViewBag.Otkuda = otkudas;

            var couList = db.Kudas.ToList();
            couList.Insert(0, new Kuda { Id = 0, Kud = "Куда" });
            SelectList countries = new SelectList(couList, "Id", "Kud", KudaId);
           
            ViewBag.Kuda = countries;

            return View();
        }

        public IActionResult Create(int? OtkudaId, int? KudaId)
        {
            var genList = db.Otkudas.ToList();

           

            SelectList genres = new SelectList(genList, "Id", "Otkud", OtkudaId);

            ViewBag.Otkuda = genres;

            var couList = db.Kudas.ToList();
           
            SelectList countries = new SelectList(couList, "Id", "Kud", KudaId);
            // Передаем список фирм в представление через ViewBag
            ViewBag.Kuda = countries;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tickets ticket)
        {
            db.Ticket.Add(ticket);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        /* [HttpGet]
         [ActionName("Delete")]
         public async Task<IActionResult> ConfirmDelete(int? id)
         {
             if (id != null)
             {
                 Tickets ticket = await db.Ticket.FirstOrDefaultAsync(b => b.Id == id);
                 if (ticket != null)
                     return View(ticket);
             }
             return NotFound();
         }

         [HttpPost]
         public async Task<IActionResult> Delete(int? id)
         {
             if (id != null)
             {
                 Tickets ticket = await db.Ticket.FirstOrDefaultAsync(b => b.Id == id);
                 if (ticket != null)
                 {
                     db.Ticket.Remove(ticket);
                     await db.SaveChangesAsync();
                     return RedirectToAction("Index");
                 }
             }
             return NotFound();
         }*/

        // Корзина
        public ActionResult Korzina(int? id)
        {
            Tickets tickets = db.Ticket.FirstOrDefault(b => b.Id == id);
            int count = 1;
            int flag = 0; //счётчик

            Korzina korzina = new Korzina()
            {
                // Если поле Id не определен атрибутом Identity
                //Id = tovar.Id, 
                Flight_number = tickets.Flight_number,
                OtkudaId = tickets.OtkudaId,
                KudaId = tickets.KudaId,
                DateP = tickets.DateP,
                Price = (int)tickets.Price // Приводим цену к int
            };
            db.Korzinas.Add(korzina);

            tickets.Kol--;
            db.SaveChanges();

            // Получаем сумму всех билетов в корзине
            int totalPrice = db.Korzinas.Sum(k => k.Price);

            // Передаем сумму в ViewBag, чтобы использовать ее в представлении
            ViewBag.TotalPrice = totalPrice;

            // Переход на главную страницу приложения
            return RedirectToAction("Index");
        }

        //Шаг 3. При выборе ссылки Корзина вызывается метод ViewKorzina.Создайте этот метод в коде контроллера Home:
        public ActionResult ViewKorzina()
        {
            // Получаем из БД все записи таблицы Person


            // Передаем все объекты записи в ViewBag
            IQueryable<Korzina> korzinas = db.Korzinas.Include(k => k.otkuda).Include(k => k.kuda);
            ViewBag.Korzinas = korzinas;
            //Кол-во
    
            // Возвращаем представление
            return View();
        }

        //Корзина Отмена
        public ActionResult Cancel(int? id)
        {
            Korzina korzina = db.Korzinas.FirstOrDefault(k => k.Id == id);
            db.Korzinas.Remove(korzina);
            
            db.SaveChanges();

            // Переход на главную страницу приложения
            return RedirectToAction("ViewKorzina");
        }
        public ActionResult ExecuteOrder(string name)
        {
            // Передача списка клиентов в представление
            IEnumerable<Client> clients = db.Clients;

            if (name != null)
            {
                clients = db.Clients.Where(x => x.Client_Name.Contains(name)).ToList();
            }
            ViewBag.Clients = clients;
            // Передача списка товаров из Корзины в предствление
            IQueryable<Korzina> korzinas = db.Korzinas.Include(k => k.otkuda).Include(k => k.kuda);
            ViewBag.Korzinas = korzinas;

           
            // Возвращаем представление
            return View();
        }
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Client clients = await db.Clients.FirstOrDefaultAsync(p => p.Id == id);
                if (clients != null)
                    return View(clients);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Client client)
        {
            db.Clients.Update(client);
            await db.SaveChangesAsync();
            return RedirectToAction("ExecuteOrder");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Client clients = await db.Clients.FirstOrDefaultAsync(p => p.Id == id);
                if (clients != null)
                    return View(clients);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Client client = await db.Clients.FirstOrDefaultAsync(p => p.Id == id);
                if (client != null)
                {
                    db.Clients.Remove(client);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ExecuteOrder");
                }
            }
            return NotFound();
        }

        /* public ActionResult OrderComplete(int id)
         {
             //Korzina Korz = db.Korzinas.FirstOrDefault(p => p.Id == id);
             Client client = db.Clients.FirstOrDefault(p => p.Id == id);
             Check check = new Check()
             {
                 Flight_number = client.,
                 Client_Name = client.Client_Name,
                 Data = DateTime.Now,
                 Price = 0
             };



             db.Checks.Add(check);
             db.SaveChanges();

             // Переход на главную страницу приложения
             return RedirectToAction("OrderComplete");
         }
         public ActionResult OrderComplete()
         {
             // Получаем из БД все записи таблицы Person
             IEnumerable<Check> check = db.Checks;

             // Передаем все объекты записи в ViewBag
             ViewBag.Checks = check;

             // Возвращаем представление
             return View();
         }*/

        public ActionResult OrderComplete(int Id)
        {
           

            Client client = db.Clients.FirstOrDefault(p => p.Id == Id);
            foreach (Korzina k in db.Korzinas)
            {
                Check chcks = new Check
                {
                    Flight_number = k.Flight_number,
                    Client_Name = client.Client_Name,
                    Data = DateTime.Now,
                    Price = k.Price
                };
                db.Checks.Add(chcks);
                db.Korzinas.Remove(k);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //Формирование чека
        /* public ActionResult OrderComplete(int Id)
         {
             DateTime ldate = System.DateTime.Today;


             foreach (Korzina k in db.Korzinas)
             {
                 Check check = new Check
                 {
                     Flight_number = k.Flight_number,
                     BookId = k.BookId,
                     LoanDate = ldate.ToString(),
                     ReturnDate = rdate.ToString(),
                     Returned = false
                 };
                 db.Loans.Add(check);
                 db.Korzinas.Remove(k);
             }
             db.SaveChanges();
             return RedirectToAction("Index");
         }*/



    }
}
