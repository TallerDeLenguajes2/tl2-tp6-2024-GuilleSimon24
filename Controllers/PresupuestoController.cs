using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_GuilleSimon24.Models;


public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private readonly PresupuestoRepository _presupuestoRepo;
    private readonly ProductoRepository _productoRepo;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _presupuestoRepo = new PresupuestoRepository();
        _productoRepo = new ProductoRepository();
    }

    // Listar todos los presupuestos
    public IActionResult Index()
    {
        var presupuestos = _presupuestoRepo.GetPresupuestos();
        return View(presupuestos);
    }

    // Crear un presupuesto (GET)
    public IActionResult CrearPre()
    {
        return View();
    }

    // Crear un presupuesto (POST)
    [HttpPost]
    public IActionResult CrearPre(Presupuestos presupuesto)
    {
        if (ModelState.IsValid)
        {
            _presupuestoRepo.CrearPresupuesto(presupuesto);
            return RedirectToAction("ListarPre");
        }
        return View(presupuesto);
    }

    // Modificar un presupuesto (GET)
    public IActionResult ModificarPre(int id)
    {
        var presupuesto = _presupuestoRepo.ObtenerPresupuestoPorId(id);
        if (presupuesto == null) return NotFound();
        return View(presupuesto);
    }

    // Modificar un presupuesto (POST)
    [HttpPost]
    public IActionResult ModificarPre(Presupuestos presupuesto)
    {
        if (ModelState.IsValid)
        {
            _presupuestoRepo.ModificarPresupuesto(presupuesto);
            return RedirectToAction("ListarPre");
        }
        return View(presupuesto);
    }

    // Eliminar un presupuesto
    public IActionResult EliminarPre(int id)
    {
        _presupuestoRepo.DeletePresupuesto(id);
        return RedirectToAction("ListarPre");
    }

    // Ver detalles de un presupuesto específico
    public IActionResult VerPresupuestoPre(int id)
    {
        var presupuesto = _presupuestoRepo.ObtenerPresupuestoPorId(id);
        return View(presupuesto);
    }

    // Agregar un producto a un presupuesto (GET)
    public IActionResult AgregarProductosPre(int id)
    {
        var presupuesto = _presupuestoRepo.ObtenerPresupuestoPorId(id);
        ViewBag.Productos = _productoRepo.getProductos(); // Obtener lista de productos
        return View(presupuesto);
    }

    // Agregar un producto a un presupuesto (POST)
    [HttpPost]
    public IActionResult AgregarProductoPre(int id, int productoId, int cantidad)
    {
        var presupuesto = _presupuestoRepo.ObtenerPresupuestoPorId(id);
        var producto = _productoRepo.GetProductoID(productoId);

        if (presupuesto != null && producto != null)
        {
            _presupuestoRepo.AgregarProducto(id, producto.IdProducto, cantidad);
        }
        return RedirectToAction("VerPresupuestoPre", new { id });
    }
}
