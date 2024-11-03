using Microsoft.Data.Sqlite;
class PresupuestoRepository
{
    string cadenaConexion = "Data Source=db/Tiendo.db";
    public bool CrearPresupuesto(Presupuestos presupuesto)
    {
        ProductoRepository repoProductos = new ProductoRepository();
        foreach (var detalle in presupuesto.Detalle)
        {
            if (repoProductos.GetProductoID(detalle.Producto.IdProducto) == null)
            {
                return false;
            }
        }
        string query1 = @"INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPre, @nombrePre, @fechaPre)";
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query1, sqlitecon);
            DateTime fecha = DateTime.Today;
            sqlitecon.Open();

            command.Parameters.Add(new SqliteParameter("@idPre", presupuesto.IdPresupuesto));
            command.Parameters.Add(new SqliteParameter("@nombrePre", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fechaPre", fecha));

            command.ExecuteNonQuery();

            sqlitecon.Close();

        }

        foreach (var item in presupuesto.Detalle)
        {
            string query2 = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPre, @idProdu, @canti)";
            using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
            {
                SqliteCommand command = new SqliteCommand(query2, sqlitecon);
                Productos prod1 = item.Producto;
                sqlitecon.Open();

                command.Parameters.Add(new SqliteParameter("@idPre", presupuesto.IdPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProdu", prod1.IdProducto));
                command.Parameters.Add(new SqliteParameter("@canti", item.Cantidad));

                command.ExecuteNonQuery();

                sqlitecon.Close();
            }
        }
        return true;
    }

    public List<Presupuestos> GetPresupuestos()
    {
        string queryDetalle = @"SELECT idPresupuesto, NombreDestinatario, FROM Presupuestos";
        List<Presupuestos> presupuestos = new List<Presupuestos>();
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryDetalle, sqlitecon);
            sqlitecon.Open();


            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idpres = Convert.ToInt32(reader["idPresupuesto"]);
                    string nombre = Convert.ToString(reader["NombreDestinatario"]);
                    DateTime fecha = Convert.ToDateTime(reader["FechaCreacion"]);
                    Presupuestos presu1 = new Presupuestos(idpres, nombre, fecha);
                    presupuestos.Add(presu1);
                }
            }
            sqlitecon.Close();
        }
        return presupuestos;
    }

    public Productos getDetalle(int id)
    {
        string query = @"SELECT pr.idProd, prod.Descripcion, prod.Precio FROM PresupuestosDetaller as pr INNER JOIN Productos as prod WHERE idPresupuesto = @idquery";
        Productos buscado = new Productos();
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();

            command.Parameters.Add(new SqliteParameter("@idquery", id));
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idProd = Convert.ToInt32(reader["idProd"]);
                    string descrip = Convert.ToString(reader["Descripcion"]);
                    int precio = Convert.ToInt32(reader["Precio"]);

                    buscado.IdProducto = idProd;
                    buscado.Descripcion = descrip;
                    buscado.Precio = precio;

                }
            }
            sqlitecon.Close();
        }

        return buscado;
    }

     public Presupuestos ObtenerPresupuestoPorId(int id)
    {
        Presupuestos presupuesto = null;

        string query = 
        @"SELECT 
            P.idPresupuesto,
            P.NombreDestinatario,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad
        FROM 
            Presupuestos P
        JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        WHERE 
            P.idPresupuesto = @id;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int cont = 1;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (cont == 1)
                    {
                        presupuesto = new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Productos producto = new Productos(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    presupuesto.Detalle.Add(detalle);
                    cont++;
                }
            }
            connection.Close();
        }
        return presupuesto;
    }


    public void DeletePresupuesto(int id)
    {
        string query1 = @"DELETE FROM Presupuestos WHERE idPresupuesto = @idQuery";
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query1, sqlitecon);
            sqlitecon.Open();

            command.Parameters.Add(new SqliteParameter("@idquery", id));

            command.ExecuteNonQuery();

            sqlitecon.Close();
        }

        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idquery";

        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query2, sqlitecon);
            sqlitecon.Open();

            command.Parameters.Add(new SqliteParameter("@idquery", id));

            command.ExecuteNonQuery();

            sqlitecon.Close();
        }
    }

    public bool AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        ProductoRepository repoProductos = new ProductoRepository();
        if (ObtenerPresupuestoPorId(idPresupuesto) == null || repoProductos.GetProductoID(idProducto) == null)
        {
            return false;
        }


        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresu, @idProd, @cant)";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresu", idPresupuesto);
            command.Parameters.AddWithValue("@idProd", idProducto);
            command.Parameters.AddWithValue("@cant", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
}