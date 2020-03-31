using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer
{
    public class DataSource : IDisposable
    {
        SqlConnection _connection;
        public DataSource(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public DataTable GetFigureById(int id)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                                    select f.id, Name from Figure f  
                                     where f.Id=@Id", _connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        #region Circle
        public DataTable GetCircles()
        {
            using (DataSet ds = new DataSet())
            {
                string sql = "select f.id,c.Id as CircleId, Name, Radius from Figure f " +
                                            "join Circle c on " +
                                            "f.Id_Circle= c.Id ";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public DataTable GetCircles(ParametrsForView p)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                    select f.id,c.Id as CircleId, Name, Radius from Figure f 
                                            join Circle c on 
                                            f.Id_Circle= c.Id
                                            where Name like '%'+ @PartOfName+'%'
                                            ORDER BY id 
                                            OFFSET @offset ROWS 
                                            FETCH NEXT @amount ROWS ONLY  "
                    , _connection))
                {
                    cmd.Parameters.Add("@PartOfName", SqlDbType.VarChar);
                    cmd.Parameters["@PartOfName"].Value = p.PartOfName;
                    cmd.Parameters.Add("@amount", SqlDbType.Int);
                    cmd.Parameters["@amount"].Value = p.Amount;
                    cmd.Parameters.Add("@offset", SqlDbType.Int);
                    cmd.Parameters["@offset"].Value = p.OffSet;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //throw new Exception();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        public int GetCirclesCount(ParametrsForView p)
        {
            _connection.Open();

            using (var cmd = new SqlCommand(@"
                    select count( f.id) from Figure f 
                                            join Circle c on 
                                            f.Id_Circle= c.Id
                                             where Name like '%'+ @PartOfName+'%'"
                , _connection))
            {
                cmd.Parameters.Add("@PartOfName", SqlDbType.VarChar);
                cmd.Parameters["@PartOfName"].Value = p.PartOfName;
                return (int)cmd.ExecuteScalar();
            }
        }
        public DataTable GetCircleById(int id)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                                    select f.id,c.Id as CircleId, Name, Radius from Figure f  
                                    join Circle c on 
                                       f.Id_Circle= c.Id 
                                     where f.Id=@Id", _connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //throw new Exception();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        public DataTable GetCirclesByStoreId(int id)
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select fs.id as Id, fs.Id_Figure as FigureId,f.Id_Circle, Name,Radius
                                from FigureStore fs
                                join Figure f  on f.Id=fs.Id_Figure
                                join Circle c on 
                                f.Id_Circle= c.Id 
                                where fs.Id_Store=@Id";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int);
                    da.SelectCommand.Parameters["@Id"].Value = id;
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public void CreateCircle(Cirlce c)
        {
            _connection.Open();
            InsertCircleUsingOneCommand(c);
        }
        public int UpdateCircle(Cirlce c)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @IdCircle int
                             set @IdCircle=(select Id_Circle from Figure
                             WHERE Id=@Id)

                            UPDATE Figure
                            SET  Name   = @name
                            WHERE Id=@Id

                            UPDATE Circle 
                            SET Radius = @radius
                            WHERE Id=@IdCircle", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = c.Id;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar);
                cmd.Parameters["@name"].Value = c.Name;
                cmd.Parameters.Add("@radius", SqlDbType.Float);
                cmd.Parameters["@radius"].Value = c.Radius;
                return cmd.ExecuteNonQuery();
            }
        }
        public int DeleteCircle(int id)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @Id_Circle int
                             set @Id_Circle=(select Id_Circle from Figure
                             WHERE Id=@Id)

                            DELETE FROM FigureStore 
                            WHERE Id_Figure=@Id;

                            DELETE FROM Figure
                            WHERE Id_Circle=@Id_Circle;
    
                            DELETE FROM Circle
                            WHERE Id=@Id_Circle", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;
                return cmd.ExecuteNonQuery();
            }
        }
        private int InsertCircle(Cirlce c)
        {
            using (var cmd = new SqlCommand(@"
                INSERT INTO Circle (Radius) 
               OUTPUT INSERTED.Id
                VALUES (@Radius) ", _connection))
            {
                cmd.Parameters.Add("@Radius", SqlDbType.Float);
                cmd.Parameters["@Radius"].Value = c.Radius;
                return (int)cmd.ExecuteScalar();
            }
        }
        private void InsertBaseClassFigureAfterCircle(Cirlce c, int id)
        {
            using (var cmd = new SqlCommand(@"
                INSERT INTO Figure (Id_Circle,Name) 
                VALUES (@Id_Circle,@Name) ", _connection))
            {
                //cmd.Parameters.AddRange(new[]
                //{
                //    new SqlParameter("@Id_Circle", SqlDbType.Int).Value = id,
                //    new SqlParameter("@Name", SqlDbType.NVarChar).Value = "234", 
                //});
                cmd.Parameters.Add("@Id_Circle", SqlDbType.Int);
                cmd.Parameters["@Id_Circle"].Value = id;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = c.Name;
                cmd.ExecuteNonQuery();
            }
        }
        private void InsertCircleUsingOneCommand(Cirlce c)
        {
            /*
            SCOPE_IDENTITY – это системная функция, 
             * которая возвращает последнее значение
             * идентификатора, вставленного в любую таблицу
             * в текущем сеансе в той же области.
              */
            //    https://info-comp.ru/programmirovanie/557-functions-ms-sql-identity-and-scope-identity.html
            using (var cmd = new SqlCommand(@"
                INSERT INTO Circle (Radius) 
                VALUES (@Radius) 
                declare @Id_Circle int
                set @Id_Circle =    SCOPE_IDENTITY()
                INSERT INTO Figure (Id_Circle,Name) 
                VALUES (@Id_Circle,@Name) "
                , _connection))
            {
                cmd.Parameters.Add("@Radius", SqlDbType.Float);
                cmd.Parameters["@Radius"].Value = c.Radius;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = c.Name;
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Rectangle
        public DataTable GetRectangles()
        {
            using (DataSet ds = new DataSet())
            {
                string sql = "select f.id,r.Id as RectangleId, Name, Width, Height from Figure f " +
                                            "join Rectangle r on " +
                                            "f.Id_Rectangle= r.Id ";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public DataTable GetRectangleById(int id)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                                    select f.id,r.Id as RectangleId, Name, Width, Height from Figure f  
                                    join Rectangle r on 
                                       f.Id_Rectangle= r.Id 
                                     where f.Id=@Id", _connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //throw new Exception();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        public void CreateRectangle(Rectangle r)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                INSERT INTO Rectangle (Width, Height) 
                VALUES (@Width, @Height) 
                declare @Id_Rectangle int
                set @Id_Rectangle =    SCOPE_IDENTITY()
                INSERT INTO Figure (Id_Rectangle,Name) 
                VALUES (@Id_Rectangle,@Name) "
               , _connection))
            {
                cmd.Parameters.Add("@Width", SqlDbType.Float);
                cmd.Parameters["@Width"].Value = r.Width;
                cmd.Parameters.Add("@Height", SqlDbType.Float);
                cmd.Parameters["@Height"].Value = r.Height;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = r.Name;
                cmd.ExecuteNonQuery();
            }
        }

        public int UpdateRectangle(Rectangle r)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @IdRectangle int
                             set @IdRectangle=(select Id_Rectangle from Figure
                             WHERE Id=@Id)

                            UPDATE Figure
                            SET  Name   = @name
                            WHERE Id=@Id

                            UPDATE Rectangle 
                            SET Width = @Width, Height = @Height
                            WHERE Id=@IdRectangle", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = r.Id;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar);
                cmd.Parameters["@name"].Value = r.Name;
                cmd.Parameters.Add("@Width", SqlDbType.Float);
                cmd.Parameters["@Width"].Value = r.Width;
                cmd.Parameters.Add("@Height", SqlDbType.Float);
                cmd.Parameters["@Height"].Value = r.Height;
                return cmd.ExecuteNonQuery();
            }
        }
        public int DeleteRectangle(int id)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @Id_Rectangle int
                             set @Id_Rectangle=(select Id_Rectangle from Figure
                             WHERE Id=@Id)

                            DELETE FROM FigureStore 
                            WHERE Id_Figure=@Id;

                            DELETE FROM Figure
                            WHERE Id_Rectangle=@Id_Rectangle;
    
                            DELETE FROM Rectangle
                            WHERE Id=@Id_Rectangle", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Squares
        public DataTable GetSquares()
        {
            using (DataSet ds = new DataSet())
            {
                string sql = "select f.id,s.Id as SquareId, Name, Side from Figure f " +
                                            "join Square s on " +
                                            "f.Id_Square = s.Id ";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public DataTable GetSquareById(int id)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                                    select f.id,s.Id as SquareId, Name, Side from Figure f  
                                    join Square s on 
                                       f.Id_Square= s.Id 
                                     where f.Id=@Id", _connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        //throw new Exception();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        public DataTable GetSquares(ParametrsForView p)
        {
            using (DataSet ds = new DataSet())
            {
                using (var cmd = new SqlCommand(@"
                    select f.id,s.Id as SquareId, Name, Side from Figure f  
                                            join Square s on 
                                            f.Id_Square = s.Id
                                            ORDER BY id 
                                            OFFSET @offset ROWS 
                                            FETCH NEXT @amount ROWS ONLY  "
                    , _connection))
                {
                    cmd.Parameters.Add("@amount", SqlDbType.Int);
                    cmd.Parameters["@amount"].Value = p.Amount;
                    cmd.Parameters.Add("@offset", SqlDbType.Int);
                    cmd.Parameters["@offset"].Value = p.OffSet;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        public int GetSquaresCount(ParametrsForView p)
        {
            _connection.Open();

            using (var cmd = new SqlCommand(@"
                    select count( f.id) from Figure f 
                                            join Square c on 
                                            f.Id_Square= c.Id"
                , _connection))
            {
                //      cmd.Parameters.Add("@amount", SqlDbType.Int);
                //       cmd.Parameters["@amount"].Value = p.Amount;
                //       cmd.Parameters.Add("@offset", SqlDbType.Int);
                //       cmd.Parameters["@offset"].Value = p.OffSet;

                //throw new Exception();
                return (int)cmd.ExecuteScalar();
            }
        }
        public void CreateSquare(Square s)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                INSERT INTO Square (Side) 
                VALUES (@Side) 
                declare @Id_Square int
                set @Id_Square =    SCOPE_IDENTITY()
                INSERT INTO Figure (Id_Square,Name) 
                VALUES (@Id_Square,@Name) "
               , _connection))
            {
                cmd.Parameters.Add("@Side", SqlDbType.Float);
                cmd.Parameters["@Side"].Value = s.Side;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = s.Name;
                cmd.ExecuteNonQuery();
            }
        }

        public int UpdateSquare(Square s)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @IdSquare int
                             set @IdSquare=(select Id_Square from Figure
                             WHERE Id=@Id)

                            UPDATE Figure
                            SET  Name   = @name
                            WHERE Id=@Id

                            UPDATE Square 
                            SET Side = @Side
                            WHERE Id=@IdSquare", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = s.Id;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar);
                cmd.Parameters["@name"].Value = s.Name;
                cmd.Parameters.Add("@Side", SqlDbType.Float);
                cmd.Parameters["@Side"].Value = s.Side;
                return cmd.ExecuteNonQuery();
            }
        }
        public int DeleteSquare(int id)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @Id_Square int
                             set @Id_Square=(select Id_Square from Figure
                             WHERE Id=@Id)

                            DELETE FROM FigureStore 
                            WHERE Id_Figure=@Id;

                            DELETE FROM Figure
                            WHERE Id_Square=@Id_Square;
    
                            DELETE FROM Square
                            WHERE Id=@Id_Square", _connection))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion
        public DataTable GetFigureStores()
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select s.Id, name, ISNULL(Fcount.number, 0) as Number
                            from FigureStore fs
                            right  join Store s on s.Id=fs.Id_Store
                            left join
                            ( select count(s.Id) as number,s.Id from FigureStore fs
                            join Store s on s.Id=fs.Id_Store
                            group by s.Id) 
                            as Fcount  on Fcount.ID = s.id 
                            group by s.Id,name,Fcount.number ";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public DataTable GetStores()
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select Id, name from  Store s";


                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public DataTable GetStoreById(int id)
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select Id, name from  Store s
                            WHERE Id=@Id";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int);
                    da.SelectCommand.Parameters["@Id"].Value = id;
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public DataTable GetRectanglesByStoreId(int id)
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select fs.id as Id, fs.Id_Figure  as FigureId,f.Id_Rectangle, Name, Width, Height
                                from FigureStore fs
                                join Figure f  on f.Id=fs.Id_Figure
                                join Rectangle r on 
                                f.Id_Rectangle = r.Id 
                                where fs.Id_Store=@Id";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int);
                    da.SelectCommand.Parameters["@Id"].Value = id;
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
        public DataTable GetSquaresByStoreId(int id)
        {
            using (DataSet ds = new DataSet())
            {
                string sql = @" select fs.id as Id, fs.Id_Figure as FigureId,f.Id_Square, Name, Side
                                from FigureStore fs
                                join Figure f  on f.Id=fs.Id_Figure
                                join Square r on 
                                f.Id_Square = r.Id 
                                where fs.Id_Store=@Id";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _connection))
                {
                    da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int);
                    da.SelectCommand.Parameters["@Id"].Value = id;
                    //throw new Exception();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public void AddFigureInStore(int storeId, int figureId)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                 INSERT INTO FigureStore (Id_Figure ,Id_Store) 
                VALUES (@IdFigure  ,@Id_Store)  "
        , _connection))
            {
                cmd.Parameters.Add("@IdFigure", SqlDbType.Int);
                cmd.Parameters["@IdFigure"].Value = figureId;
                cmd.Parameters.Add("@Id_Store", SqlDbType.Int);
                cmd.Parameters["@Id_Store"].Value = storeId;

                cmd.ExecuteNonQuery();

            }

        }

        public int RemoveFigureFromStore(int idInStore)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                            DELETE FROM FigureStore 
                            WHERE Id = @idInStore"
        , _connection))
            {
                cmd.Parameters.Add("@idInStore", SqlDbType.Int);
                cmd.Parameters["@idInStore"].Value = idInStore;
                return cmd.ExecuteNonQuery();
            }
        }

        public void CreateFiguresStore(FiguresStore fs)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                INSERT INTO Store (Name) 
                VALUES (@Name) "
               , _connection))
            {
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = fs.Name;
                cmd.ExecuteNonQuery();

            }
        }

        public void UpdateStore(FiguresStore fs)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                UPDATE Store
                SET  Name   = @name
                WHERE Id=@Id "
               , _connection))
            {
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters["@Name"].Value = fs.Name;
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = fs.Id;
                cmd.ExecuteNonQuery();

            }
        }

        public void DeleteStore(int id)
        {
            _connection.Open();
            using (var cmd = new SqlCommand(@"
                             declare @figureAmount int
                             set @figureAmount=(select count(*) from FigureStore
                                where Id_Store=@IdStore);
                            if (@figureAmount=0)
                            begin
                                DELETE FROM Store 
                                WHERE Id=@IdStore;
                            end ", _connection))
            {
                cmd.Parameters.Add("@IdStore", SqlDbType.Int);
                cmd.Parameters["@IdStore"].Value = id;
                cmd.ExecuteNonQuery();
            }
        }

        private bool disposed = false;

        protected void Dispose(bool flag)
        {
            if (!disposed)
            {
                _connection.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
