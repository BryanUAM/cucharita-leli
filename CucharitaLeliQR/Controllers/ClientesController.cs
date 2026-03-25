using Microsoft.AspNetCore.Authorization;
using CucharitaLeliQR.Data;
using CucharitaLeliQR.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.IO;

namespace CucharitaLeliQR.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly SodaContext _context;

        public ClientesController(SodaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _context.Clientes.ToList();
            return View(lista);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        private DateTime ObtenerHoraCostaRica()
        {
            var zonaCR = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaCR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // 🔥 DEBUG
                }
            }

            if (ModelState.IsValid)
            {
                cliente.Puntos = 0;
                cliente.CodigoQR = Guid.NewGuid().ToString();

                _context.Clientes.Add(cliente);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var clienteDB = _context.Clientes.Find(cliente.Id);

                if (clienteDB == null)
                    return NotFound();

                clienteDB.Nombre = cliente.Nombre;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        public IActionResult SumarPuntosQR(string id)
        {
            return Content("Este código QR solo puede ser procesado desde el sistema interno de caja.");
        }

        [HttpPost]
        public IActionResult ProcesarEscaneoInterno(string codigo)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.CodigoQR == codigo);

            if (cliente == null)
            {
                return Json(new
                {
                    ok = false,
                    mensaje = "Cliente no encontrado"
                });
            }

            cliente.Puntos += 10;

            if (cliente.Puntos > 80)
                cliente.Puntos = 80;

            cliente.UltimoEscaneo = ObtenerHoraCostaRica();
            _context.SaveChanges();

            int faltan = 80 - cliente.Puntos;
            if (faltan < 0)
                faltan = 0;

            return Json(new
            {
                ok = true,
                nombre = cliente.Nombre,
                puntos = cliente.Puntos,
                faltan = faltan,
                mensaje = "Puntos agregados correctamente"
            });
        }

        [HttpGet]
        public JsonResult SumarPuntosAjax(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return Json(new { mensaje = "Error" });
            }

            if (cliente.Puntos >= 80)
            {
                return Json(new
                {
                    nombre = cliente.Nombre,
                    puntos = cliente.Puntos,
                    mensaje = "🎁 Ya tiene un premio pendiente",
                    fecha = cliente.UltimoEscaneo?.ToString("dd/MM/yyyy HH:mm")
                });
            }

            cliente.Puntos += 10;

            if (cliente.Puntos > 80)
                cliente.Puntos = 80;

            cliente.UltimoEscaneo = ObtenerHoraCostaRica();

            _context.SaveChanges();

            int faltan = 80 - cliente.Puntos;

            string mensaje = cliente.Puntos >= 80
                ? "🎁 ¡Premio disponible!"
                : $"Te faltan {faltan} puntos para premio";

            return Json(new
            {
                nombre = cliente.Nombre,
                puntos = cliente.Puntos,
                mensaje,
                fecha = cliente.UltimoEscaneo?.ToString("dd/MM/yyyy HH:mm")
            });
        }

        [HttpGet]
        public JsonResult EntregarPremioAjax(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return Json(new { mensaje = "Error" });
            }

            cliente.PremiosCanjeados += 1;
            cliente.Puntos = 0;
            cliente.UltimoEscaneo = ObtenerHoraCostaRica();

            _context.SaveChanges();

            return Json(new
            {
                nombre = cliente.Nombre,
                fecha = cliente.UltimoEscaneo?.ToString("dd/MM/yyyy HH:mm"),
                premios = cliente.PremiosCanjeados
            });
        }

        public IActionResult GenerarQR(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);

                if (cliente == null)
                    return Content("Error: cliente no encontrado");

                if (string.IsNullOrEmpty(cliente.CodigoQR))
                {
                    cliente.CodigoQR = Guid.NewGuid().ToString();
                    _context.SaveChanges();
                }

                string contenidoQR = cliente.CodigoQR;

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(contenidoQR, QRCodeGenerator.ECCLevel.Q))
                {
                    PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                    byte[] qrBytes = qrCode.GetGraphic(20);

                    return File(qrBytes, "image/png");
                }
            }
            catch (Exception ex)
            {
                return Content("ERROR REAL: " + ex.ToString());
            }
        }
        public IActionResult Dashboard()
        {
            var clientes = _context.Clientes.ToList();

            ViewBag.TotalClientes = clientes.Count;
            ViewBag.ClientesConPremio = clientes.Count(c => c.Puntos >= 80);
            ViewBag.PremiosCanjeados = clientes.Sum(c => c.PremiosCanjeados);

            return View(clientes);
        }

        public IActionResult Eliminar(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult EscanearQR()
        {
            return View();
        }
    }
}