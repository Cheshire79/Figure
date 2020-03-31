using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLayer;
using BusinessLayer.Entities;
using BusinessLayer.Exceptions;
using WEBUI.Models;
using WEBUI.Models.Figures;
using WEBUI.Models.Figures.Circle;
using WEBUI.Models.FigureStore;

namespace WEBUI.Controllers
{
    public class FigureStoreController : Controller
    {
        string GetConnectionStringToBD()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionToBD"].ConnectionString;
            return connectionString;
        }
        string GetConnectionStringToFile()
        {
            // by default seek bd File near .exe; so in bin\Debug\
            // so need to copy in that place. 
            // if create Release also need to copy bin\Release\
            // and by the wat this directory are Temp one))
            // so copy to the folder at more high level
            // кроме того что єто такое, зачем єто нужно - "занимательная вивисекция

            // https://politikus.ru/articles/politics/56971-malysh-i-karlson-nauchnyy-perevod.html
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionToFile"].ConnectionString;
            AdjustDataDirectory();
            return connectionString;
        }

        private void AdjustDataDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));// todo
            Console.WriteLine("path= {0}", absolute);
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }
        //
        // GET: /Figure/
        private FiguresService _figuresService;
        public FigureStoreController()
        {
            _figuresService = new FiguresService(GetConnectionStringToFile());
        }
        public ActionResult FigureStoreList()//+
        {
            try
            {
                List<FigureStoreView> fsv = new List<FigureStoreView>();
                foreach (FiguresStoreBll fs in _figuresService.GetAllStores())
                {
                    fsv.Add(new FigureStoreView(fs.Id, fs.Name, fs.Count, fs.GetAreas()));
                }
                FigureStoresView fsvList = new FigureStoresView() { FigureStores = fsv };
                return View(fsvList);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult FigureStoreEdit(int? id)//+
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                FiguresStoreBll fs = _figuresService.GetStoreById(id.Value);
                List<FigureView> figures = new List<FigureView>();
                foreach (var item in fs.Figures)
                {
                    figures.Add(FigureViewFactory.Create(item));
                }

                FigureStoreDetailView fsdv = new FigureStoreDetailView(fs.Id, fs.Name, fs.Count, fs.GetAreas());
                fsdv.Figures = figures;
                return View(fsdv);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }

        public ActionResult StoreCreate()// +
        {
            return View();
        }

        [HttpPost]
        public ActionResult StoreCreate(FigureStoreCreatingAndEditingView fs)// +
        {
            if (ModelState.IsValid)//todo else
            {
                try
                {
                    _figuresService.CreateFigureStore(new FiguresStoreBll(0, fs.Name));
                    return RedirectToAction("FigureStoreList");
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }
            }
            return View(fs);
        }
        public ActionResult StoreEdit(int? id)// +
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                FiguresStoreBll fs = _figuresService.GetStoreById(id.Value);
                FigureStoreCreatingAndEditingView fscv = new FigureStoreCreatingAndEditingView() { Id = fs.Id, Name = fs.Name };
                return View(fscv);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult StoreEdit(FigureStoreCreatingAndEditingView fs)// +
        {
            if (ModelState.IsValid)
            {
                //Range validation where value is with comma and not with dot
                //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
                {
                    try
                    {
                        _figuresService.UpdateStore(new FiguresStoreBll(fs.Id, fs.Name));
                        return RedirectToAction("FigureStoreList");
                    }
                    catch (DataNotFoundException ex)
                    {
                        return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
                    }
                    catch (Exception ex)
                    {
                        return HttpNotFound(ex.Message);
                    }
                }

            }
            return View(fs);
        }

        [HttpPost]
        public ActionResult FigureStoreDelete(int? id)//+
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                var fs = _figuresService.GetStoreById(id.Value);
                if (fs.Count > 0)
                    return RedirectToAction("DataCouldNotBeDeleted", "Error", new { message = "Cannot remove storage. The storage " + fs.Name + " is not empty" });
                _figuresService.DeleteStore(id.Value);
                return RedirectToAction("FigureStoreList");
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult CirclesForAdd(int? storeId)//+ 
        {
            if (storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                FiguresStoreBll fs = _figuresService.GetStoreById(storeId.Value);
                List<CirlceView> cirlces = new List<CirlceView>();
                foreach (var item in _figuresService.GetAllCirlces())
                {
                    cirlces.Add(new CirlceView() { Id = item.Id, Name = item.Name, Radius = item.Radius, Area = item.GetArea() });
                }

                CirlcesViewForAddingToStore c = new CirlcesViewForAddingToStore()
                {
                    Cirlces = cirlces,
                    StoreId = fs.Id,
                    StoreName = fs.Name,
                    PagingInfo = new PagingInfoView
                    {
                        CurrentPage = 1,
                        ItemsPerPage = 10,
                        TotalItems = 1
                    },
                };
                return View(c);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult AddCircle(int? storeId, int? figureId)//+
        {
            if (figureId == null || storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.AddCirclceInStore(storeId.Value, figureId.Value);
                return RedirectToAction("FigureStoreEdit", new { id = storeId });
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult SquaresForAdd(int? storeId)//+
        {
            if (storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                List<SquareView> squares = new List<SquareView>();
                foreach (var item in _figuresService.GetAllSquares())
                {
                    squares.Add(new SquareView() { Id = item.Id, Name = item.Name, Side = item.Side, Area = item.GetArea() });
                }
                FiguresStoreBll fs = _figuresService.GetStoreById(storeId.Value);
                SquaresViewForAddingToStore c = new SquaresViewForAddingToStore()
                {
                    Squares = squares,
                    StoreId = fs.Id,
                    StoreName = fs.Name,
                    PagingInfo = new PagingInfoView
                    {
                        CurrentPage = 1,
                        ItemsPerPage = 10,
                        TotalItems = 1
                    },
                };
                return View(c);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult AddSquare(int? storeId, int? figureId)//+
        {
            if (figureId == null || storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.AddSquareInStore(storeId.Value, figureId.Value);
                return RedirectToAction("FigureStoreEdit", new { id = storeId });
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult RectanglesForAdd(int? storeId)//+
        {
            if (storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                List<RectangleView> rectangles = new List<RectangleView>();
                foreach (var item in _figuresService.GetAllRectangles())
                {
                    rectangles.Add(new RectangleView() { Id = item.Id, Name = item.Name, Width = item.Width, Height = item.Height, Area = item.GetArea() });
                }
                FiguresStoreBll fs = _figuresService.GetStoreById(storeId.Value);
                RectanglesViewForAddingToStore c = new RectanglesViewForAddingToStore()
                {
                    Rectangles = rectangles,
                    StoreId = fs.Id,
                    StoreName = fs.Name,
                    PagingInfo = new PagingInfoView
                    {
                        CurrentPage = 1,
                        ItemsPerPage = 10,
                        TotalItems = 1
                    },
                };
                return View(c);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult AddRectangle(int? storeId, int? figureId)//+
        {
            if (figureId == null || storeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.AddRectangleInStore(storeId.Value, figureId.Value);
                return RedirectToAction("FigureStoreEdit", new { id = storeId });
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult RemoveFigureFrom(int? storeId, int? figureId, int? idInStore)//++
        {
            if (figureId == null || storeId == null || idInStore == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.RemoveFigureFromStore(storeId.Value, figureId.Value, idInStore.Value);
                return RedirectToAction("FigureStoreEdit", new { id = storeId });
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataCouldNotBeDeleted", "Error", new { message = "Cannot remove figure. " + ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _figuresService.Dispose();
            base.Dispose(disposing);
        }
    }
}