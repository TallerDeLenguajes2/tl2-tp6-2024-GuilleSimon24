class PresupuestoDetalle
{
    private Productos producto;
    private int cantidad;

    public PresupuestoDetalle()
    {
    }

    public PresupuestoDetalle(Productos producto, int cantidad)
    {
        this.producto = producto;
        this.cantidad = cantidad;
    }

    public int Cantidad { get => cantidad; set => cantidad = value; }
    public Productos Producto { get => producto; set => producto = value; }
}