using Microsoft.Data.Sqlite;

class ProductoRepository
{
    string cadenaConexion = "Data Source=db/Tiendo.db";
    public void CrearProducto(Productos producto)
    {
        string query = $"INSERT INTO Productos (Descripcion, Precio) VALUES (@descrip, @precio);";

        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();
            
            command.Parameters.Add(new SqliteParameter("@descrip", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));

            command.ExecuteNonQuery();

            sqlitecon.Close();
            
        }
    }

    public void ModificarProducto(int id, Productos producto)
    {
        string query = @"UPDATE Productos SET Descripcion = '@descrip', Precio = '@precio' 
        WHERE idProducto = @idQuery;";

        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();
            
            command.Parameters.Add(new SqliteParameter("@idQuery", id));
            command.Parameters.Add(new SqliteParameter("@descrip", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));

            command.ExecuteNonQuery();

            sqlitecon.Close();
        }
    }

    public List<Productos> getProductos()
    {
        List<Productos> prods = new List<Productos>();
        string query = @"SELECT P.idProducto, P.Descripcion, P.Precio FROM Productos P";

        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();
            using(SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["idProducto"]);
                    string descrip = Convert.ToString(reader["Descripcion"]);
                    int precio = Convert.ToInt32(reader["Precio"]);
                    Productos prod1 = new Productos(id, descrip, precio);
                    prods.Add(prod1);   
                }
            }
            sqlitecon.Close();
        }
        return prods;
    }

    public Productos GetProductoID(int id)
    {
        string query = @"SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @idQuery";
        Productos buscado = new Productos();
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();

            command.Parameters.Add(new SqliteParameter("@idQuery", id));
            
            
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    buscado.IdProducto = Convert.ToInt32(reader[0]);
                    buscado.Descripcion = Convert.ToString(reader[1]);
                    buscado.Precio = Convert.ToInt32(reader[2]);
                    
                }                
            }

            sqlitecon.Close();
        }

        return buscado;
    }

    public void DeleteProducto(int id)
    {
        string query = @"DELETE FROM Productos WHERE idProducto = @idquery";
        using (SqliteConnection sqlitecon = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(query, sqlitecon);
            sqlitecon.Open();
            
            command.Parameters.Add(new SqliteParameter("@idQuery", id));

            command.ExecuteNonQuery();

            sqlitecon.Close();
        }
    }
}