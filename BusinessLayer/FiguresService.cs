using System;
using System.Collections.Generic;
using System.Data;
using BusinessLayer.Entities;
using BusinessLayer.Exceptions;
using DataLayer;
using DataLayer.Entities;

namespace BusinessLayer
{
    public class FiguresService : IDisposable
    {
        DataSource _ds;
        public FiguresService(string connectionString)
        {
            _ds = new DataSource(connectionString);
        }

        public IEnumerable<FiguresStoreBll> GetAllStores()//+
        {
            List<FiguresStoreBll> fsList = new List<FiguresStoreBll>();

            foreach (DataRow item in _ds.GetStores().Rows)
                fsList.Add(new FiguresStoreBll((int)item["Id"], (string)item["Name"]));

            foreach (FiguresStoreBll fs in fsList)
            {
                fs.Figures = GetFiguresByStoreId(fs.Id);
            }
            return fsList;
        }

        public FiguresStoreBll GetStoreById(int id)//+
        {
            var rows = _ds.GetStoreById(id).Rows;
            if (rows.Count == 0)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", id));

            DataRow st = rows[0];
            FiguresStoreBll fs = new FiguresStoreBll((int)st["Id"], (string)st["Name"]);
            fs.Figures = GetFiguresByStoreId(id);

            return fs;
        }
        public List<FigureBll> GetAllFigures()//+
        {
            List<FigureBll> figures = new List<FigureBll>();
            foreach (DataRow item in _ds.GetCircles().Rows)
                figures.Add(new CirlceBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Radius"]));
            foreach (DataRow item in _ds.GetRectangles().Rows)
                figures.Add(new RectangleBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Width"], (float)(double)item["Height"]));
            foreach (DataRow item in _ds.GetSquares().Rows)
                figures.Add(new SquareBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Side"]));
            return figures;
        }
        private List<FigureBll> GetFiguresByStoreId(int id)//+
        {
            List<FigureBll> figures = new List<FigureBll>();
            foreach (DataRow item in _ds.GetCirclesByStoreId(id).Rows)
                figures.Add(new CirlceBll((int)item["Id"], (int)item["FigureId"], (string)item["Name"], (float)(double)item["Radius"]));
            foreach (DataRow item in _ds.GetRectanglesByStoreId(id).Rows)
                figures.Add(new RectangleBll((int)item["Id"], (int)item["FigureId"], (string)item["Name"], (float)(double)item["Width"], (float)(double)item["Height"]));
            foreach (DataRow item in _ds.GetSquaresByStoreId(id).Rows)
                figures.Add(new SquareBll((int)item["Id"], (int)item["FigureId"], (string)item["Name"], (float)(double)item["Side"]));
            return figures;
        }

        #region Circle
        public IEnumerable<CirlceBll> GetAllCirlces()
        {
            List<CirlceBll> c = new List<CirlceBll>();
            foreach (DataRow item in _ds.GetCircles().Rows)
            {
                c.Add(new CirlceBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Radius"]));
            }
            return c;
        }

        public int GetCirclesCount(ParametrsForViewBll p)
        {
            return _ds.GetCirclesCount(new ParametrsForView()
            { Amount = p.PageSize, OffSet = ((p.Page - 1)*p.PageSize), PartOfName = p.PartOfName});
        }
        public IEnumerable<CirlceBll> GetAllCirlces(ParametrsForViewBll p)
        {
            List<CirlceBll> c = new List<CirlceBll>();
            foreach (DataRow item in _ds.GetCircles(new ParametrsForView()
                                                    { Amount = p.PageSize, OffSet = ((p.Page - 1) * p.PageSize), PartOfName = p.PartOfName})
                .Rows)
            {
                c.Add(new CirlceBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Radius"]));
            }
            return c;
        }
        public CirlceBll GetCirlceById(int id)//+
        {
            var request = _ds.GetCircleById(id).Rows;
            if (request.Count == 0)
                throw new DataNotFoundException(string.Format("Cannot find circle with id ={0}", id));
            DataRow item = request[0];
            return new CirlceBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Radius"]);
        }
        public void CreateCirlce(CirlceBll c)
        {
            _ds.CreateCircle(new Cirlce(null, 0, c.Name, c.Radius));

        }
        public void UpdateCirlce(CirlceBll c)//+
        {
            if (_ds.UpdateCircle(new Cirlce(null,c.Id, c.Name, c.Radius)) == 0)
                throw new DataNotFoundException(string.Format("Cannot find circle with id ={0}", c.Id));
        }
        public void DeleteCirlce(int id)
        {
            if (_ds.DeleteCircle(id) == 0)
                throw new DataNotFoundException(string.Format("Element with id {0} has been already deleted", id));
        }
        #endregion

        #region Rectangle
        public IEnumerable<RectangleBll> GetAllRectangles()
        {
            List<RectangleBll> s = new List<RectangleBll>();
            foreach (DataRow item in _ds.GetRectangles().Rows)
            {
                s.Add(new RectangleBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Width"], (float)(double)item["Height"]));
            }
            return s;

        }
        public RectangleBll GetRectangleById(int id)//+
        {
            var request = _ds.GetRectangleById(id).Rows;
            if (request.Count == 0)
                throw new DataNotFoundException(string.Format("Cannot find rectangle with id ={0}", id));
            DataRow item = request[0];
            return new RectangleBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Width"], (float)(double)item["Height"]);
        }
        public void CreateRectangle(RectangleBll r)
        {
            _ds.CreateRectangle(new Rectangle(null,0, r.Name, r.Width, r.Height));
        }
        public void UpdateRectangle(RectangleBll r)//+
        {
            if (_ds.UpdateRectangle(new Rectangle(null, r.Id, r.Name, r.Width, r.Height)) == 0)
                throw new DataNotFoundException(string.Format("Cannot find rectangle with id ={0}", r.Id));
        }
        public void DeleteRectangle(int id)
        {
            if (_ds.DeleteRectangle(id) == 0)
                throw new DataNotFoundException(string.Format("Element with id {0} has been already deleted", id));
        }
        #endregion

        #region Square
        public IEnumerable<SquareBll> GetAllSquares()
        {
            List<SquareBll> s = new List<SquareBll>();
            foreach (DataRow item in _ds.GetSquares().Rows)
            {
                s.Add(new SquareBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Side"]));
            }
            return s;

        }

        public int GetSquaresCount(ParametrsForViewBll p)
        {
            return _ds.GetSquaresCount(new ParametrsForView() { Amount = p.PageSize, OffSet = ((p.Page - 1) * p.PageSize) });
        }
        public IEnumerable<SquareBll> GetAllSquares(ParametrsForViewBll p)
        {
            List<SquareBll> c = new List<SquareBll>();
            foreach (DataRow item in _ds.GetSquares(new ParametrsForView() { Amount = p.PageSize, OffSet = ((p.Page - 1) * p.PageSize) })
                .Rows)
            {
                c.Add(new SquareBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Side"]));
            }
            return c;
        }
        public SquareBll GetSquareById(int id)//+
        {
            var request = _ds.GetSquareById(id).Rows;
            if (request.Count == 0)
                throw new DataNotFoundException(string.Format("Cannot find square with id ={0}", id));
            DataRow item = request[0];
            return new SquareBll(null, (int)item["Id"], (string)item["Name"], (float)(double)item["Side"]);
        }
        public void CreateSquare(SquareBll s)
        {
            _ds.CreateSquare(new Square(null, 0, s.Name, s.Side));
        }
        public void UpdateSquare(SquareBll s)//+
        {
            if (_ds.UpdateSquare(new Square(null, s.Id, s.Name, s.Side)) == 0)
                throw new DataNotFoundException(string.Format("Cannot find square with id ={0}", s.Id));
        }
        public void DeleteSquare(int id)
        {
            if (_ds.DeleteSquare(id) == 0)
                throw new DataNotFoundException(string.Format("Element with id {0} has been already deleted", id));
        }
        #endregion
        public void AddCirclceInStore(int storeId, int figureId)//+
        {
            if (_ds.GetCircleById(figureId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find circle with id ={0}", figureId));
            if (_ds.GetStoreById(storeId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", storeId));
            _ds.AddFigureInStore(storeId, figureId);
        }
        public void AddSquareInStore(int storeId, int figureId)//+
        {
            if (_ds.GetSquareById(figureId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find square with id ={0}", figureId));
            if (_ds.GetStoreById(storeId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", storeId));
            _ds.AddFigureInStore(storeId, figureId);
        }
        public void AddRectangleInStore(int storeId, int figureId)//+
        {
            if (_ds.GetRectangleById(figureId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find rectangle with id ={0}", figureId));
            if (_ds.GetStoreById(storeId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", storeId));
            _ds.AddFigureInStore(storeId, figureId);
        }
        public void RemoveFigureFromStore(int storeId, int figureId, int idInStore)//+
        {
            if (_ds.GetFigureById(figureId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find figure with id ={0}", figureId));
            if (_ds.GetStoreById(storeId).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", storeId));
            int deletedElements = _ds.RemoveFigureFromStore(idInStore);
            if (deletedElements == 0)
                throw new DataNotFoundException(string.Format(" Element with id {0} has been already deleted.", figureId));
        }

        public void CreateFigureStore(FiguresStoreBll fs)//+
        {
            _ds.CreateFiguresStore(new FiguresStore() { Id = 0, Name = fs.Name });
        }

        public void UpdateStore(FiguresStoreBll fs)//+
        {
            if (_ds.GetStoreById(fs.Id).Rows.Count != 1)
                throw new DataNotFoundException(string.Format("Cannot find store with id ={0}", fs.Id));
            _ds.UpdateStore(new FiguresStore() { Id = fs.Id, Name = fs.Name });
        }
        public void DeleteStore(int id)
        {
            _ds.DeleteStore(id);
        }
        private bool disposed = false;
        protected void Dispose(bool flag)
        {
            if (!disposed)
            {
                _ds.Dispose();
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
