using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_GuilleSimon24.Models;


public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private readonly ProductoRepository RepositorioProducto;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        RepositorioProducto = new ProductoRepository();
    }

    public IActionResult Index()
    {   
        var productos = RepositorioProducto.getProductos();
        return View(productos);
    }

    
[HttpGet]
public IActionResult Crear()
{
    var producto = new Productos();  // Inicializas el modelo si es necesario
    return View(producto);  // Pasas el objeto a la vista
}


    [HttpPost]
    public IActionResult Crear(Productos producto)
    {
        if (ModelState.IsValid)
        {
            RepositorioProducto.CrearProducto(producto);
            return RedirectToAction("Listar");
        }
        return View(producto);
    }

    [HttpGet]
        public IActionResult Modificar(int id)
        {
            var producto = RepositorioProducto.GetProductoID(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Modificar(int id, Productos producto)
        {
            if (ModelState.IsValid)
            {
                producto.IdProducto = id;
                RepositorioProducto.ModificarProducto(id, producto);
                return RedirectToAction("Listar");
            }
            return View(producto);
        }
         public IActionResult Eliminar(int id)
        {
            RepositorioProducto.DeleteProducto(id);
            return RedirectToAction("Listar");
        }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}