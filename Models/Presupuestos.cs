public class Presupuestos
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuestos()
    {
    }

     public Presupuestos(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.FechaCreacion = fechaCreacion;
        detalle = new List<PresupuestoDetalle>();
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    internal List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

    public int montoPresupuesto()
    {
        int monto = 0;
        foreach (var compra in detalle){
            monto += compra.Producto.Precio * compra.Cantidad;
        }
        return monto;
    }

    public double montoPresupuestoConIVA()
    {
        int monto = montoPresupuesto();
        return monto * 1.21;
    }

    public int cantidadDeProductos()
    {
        int cant = 0;
        foreach (var item in detalle)
        {
            cant += item.Cantidad;
        }
        return cant;
    }
}