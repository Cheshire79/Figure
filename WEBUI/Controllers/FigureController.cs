using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Entities;
using BusinessLayer.Exceptions;
using WEBUI.Models;
using WEBUI.Models.Figures;
using WEBUI.Models.Figures.Circle;
using WEBUI.Models.Figures.Rectangle;
using WEBUI.Models.Figures.Square;

namespace WEBUI.Controllers
{
    public class FigureController : Controller
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

        private FiguresService _figuresService;
        public FigureController()
        {
            _figuresService = new FiguresService(GetConnectionStringToFile());
        }
        public ActionResult AllFigures()//+
        {
            try
            {
                List<FigureView> figures = new List<FigureView>();

                foreach (var item in _figuresService.GetAllFigures())
                {
                    figures.Add(FigureViewFactory.Create(item));
                }

                FiguresView f = new FiguresView()
                {
                    Figures = figures,

                    PagingInfo = new PagingInfoView
                    {
                        CurrentPage = 1,
                        ItemsPerPage = 10,
                        TotalItems = 1
                    },
                };
                return View(f);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }

        public PartialViewResult FiguresMenu(FigureType figureType = FigureType.All)
        {
            FiguresMenuView figuresMenu = new FiguresMenuView()
            {
                Figures = new List<FigureMenuView>()
                {
                    new FigureMenuView(){Type=FigureType.Circle, Name = "Circle", ControllerName = "Circles"},
                    new FigureMenuView(){Type=FigureType.Square, Name = "Square", ControllerName = "Squares"},
                    new FigureMenuView(){Type=FigureType.Rectangle, Name = "Rectangle", ControllerName = "Rectangles"}
                }
                ,
                SelectedFigureType = figureType
            };

            return PartialView(figuresMenu);

        }

        #region Circle

        public ActionResult Circles()//+
        {
            try
            {
                ParametrsForView p = new ParametrsForView();
                List<CirlceView> cirlces = new List<CirlceView>();

                ParametrsForViewBll pBll = new ParametrsForViewBll()
                {
                    Page = p.Page,
                    PageSize = Parametrs.PageSize,
                    PartOfName = p.PartOfName
                };
                foreach (var item in _figuresService.GetAllCirlces(pBll))
                {
                    cirlces.Add(new CirlceView() { Id = item.Id, Name = item.Name, Radius = item.Radius });
                }
                PagingInfoView pagingInfo = new PagingInfoView
                {
                    CurrentPage = p.Page,
                    ItemsPerPage = Parametrs.PageSize,
                    TotalItems = _figuresService.GetCirclesCount(pBll)
                };
                CirlcesView c = new CirlcesView()
                {
                    Cirlces = cirlces,
                    PagingInfo = pagingInfo,
                };
                return View(c);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult Circles(ParametrsForView parametrs)//+
        {
            try
            {
                List<CirlceView> cirlces = new List<CirlceView>();
                ParametrsForViewBll pBll = new ParametrsForViewBll()
                {
                    Page = parametrs.Page,
                    PageSize = Parametrs.PageSize,
                    PartOfName = parametrs.PartOfName ?? ""
                };
                int totalItems = _figuresService.GetCirclesCount(pBll);
                bool isPAgeActual = ((parametrs.Page - 1) * Parametrs.PageSize < totalItems);
                int updatedPage = isPAgeActual ? parametrs.Page : 1;
                pBll.Page = updatedPage;
                foreach (var item in _figuresService.GetAllCirlces(pBll))
                {
                    cirlces.Add(new CirlceView() { Id = item.Id, Name = item.Name, Radius = item.Radius });
                }

                PagingInfoView pagingInfo = new PagingInfoView
                {
                    CurrentPage = pBll.Page,
                    ItemsPerPage = Parametrs.PageSize,
                    TotalItems = totalItems
                };
                CirlcesView c = new CirlcesView()
                {
                    Cirlces = cirlces,
                    PagingInfo = pagingInfo,
                };
                return View(c);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult CircleCreate()//+
        {
            return View();
        }

        [HttpPost]
        public ActionResult CircleCreate(CircleCreatingAndEditingView c)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.CreateCirlce(new CirlceBll(null, 0, c.Name, float.Parse(c.Radius)));
                    return RedirectToAction("Circles");
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }
            }
            return View(c);
        }

        public ActionResult CircleEdit(int? id)//+
        {

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                CirlceBll c = _figuresService.GetCirlceById(id.Value);
                CircleCreatingAndEditingView cv = new CircleCreatingAndEditingView() { Id = c.Id, Name = c.Name, Radius = c.Radius.ToString() };
                return View(cv);
            }
            catch (DataNotFoundException ex)
            {
                return RedirectToAction("DataNotFound", "Error", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
            /*
  ActionResult CircleEdit(PagingInfoView pagingInfoView, int? id)//+
  * pass values from Html.BeginForm into controller
@Html.HiddenFor(model => pagingInfoView.TotalItems)
     @Html.HiddenFor(model => pagingInfoView.CurrentPage)
     @Html.HiddenFor(model => pagingInfoView.ItemsPerPage)
                         
      @Html.HiddenFor(model => id)
*/
        }

        [HttpPost]
        public ActionResult CircleEdit(CircleCreatingAndEditingView c)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.UpdateCirlce(new CirlceBll(null, c.Id, c.Name, float.Parse(c.Radius)));
                    return RedirectToAction("Circles");
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
            return View(c);
        }

        [HttpPost]
        public ActionResult CircleDelete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.DeleteCirlce(id.Value);
                return RedirectToAction("Circles");
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
        #endregion

        #region Rectangle
        public ActionResult Rectangles()
        {
            try
            {
                // throw new Exception("TEst");
                List<RectangleView> rectangles = new List<RectangleView>();
                foreach (var item in _figuresService.GetAllRectangles())
                {
                    rectangles.Add(new RectangleView() { Id = item.Id, Name = item.Name, Height = item.Height, Width = item.Width });
                }

                RectanglesView c = new RectanglesView()
                {
                    Rectangles = rectangles,

                    PagingInfo = new PagingInfoView
                    {
                        CurrentPage = 1,
                        ItemsPerPage = 10,
                        TotalItems = 1
                    },
                };

                return View(c);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult RectangleCreate()//+
        {
            return View();
        }

        [HttpPost]
        public ActionResult RectangleCreate(RectangleCreatingAndEditingView r)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.CreateRectangle(new RectangleBll(null, 0, r.Name, float.Parse(r.Width), float.Parse(r.Height)));
                    return RedirectToAction("Rectangles");
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }
            }
            return View(r);
        }

        public ActionResult RectangleEdit(int? id)//+
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                RectangleBll c = _figuresService.GetRectangleById(id.Value);
                RectangleCreatingAndEditingView cv = new RectangleCreatingAndEditingView() { Id = c.Id, Name = c.Name, Width = c.Width.ToString(), Height = c.Height.ToString() };
                return View(cv);
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
        public ActionResult RectangleEdit(RectangleCreatingAndEditingView r)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.UpdateRectangle(new RectangleBll(null, r.Id, r.Name, float.Parse(r.Width), float.Parse(r.Height)));
                    return RedirectToAction("Circles");
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
            return View(r);
        }

        [HttpPost]
        public ActionResult RectangleDelete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.DeleteRectangle(id.Value);
                return RedirectToAction("Rectangles");
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
        #endregion

        #region Square

        public ActionResult Squares(int page = 1)
        {
            try
            {
                List<SquareView> squares = new List<SquareView>();
                ParametrsForViewBll pBll = new ParametrsForViewBll()
                {
                    Page = page,
                    PageSize = Parametrs.PageSize,
                };
                foreach (var item in _figuresService.GetAllSquares(pBll))
                {
                    squares.Add(new SquareView() { Id = item.Id, Name = item.Name, Side = item.Side });
                }

                PagingInfoView pagingInfo = new PagingInfoView
                {
                    CurrentPage = page,
                    ItemsPerPage = Parametrs.PageSize,
                    TotalItems = _figuresService.GetSquaresCount(pBll)
                };
                SquaresView c = new SquaresView()
                {
                    Squares = squares,
                    PagingInfo = pagingInfo,
                };

                return View(c);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
        public ActionResult SquareCreate()//+
        {
            return View();
        }

        [HttpPost]
        public ActionResult SquareCreate(SquareCreatingAndEditingView s)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.CreateSquare(new SquareBll(null, 0, s.Name, float.Parse(s.Side)));
                    return RedirectToAction("Squares");
                }
                catch (Exception ex)
                {
                    return HttpNotFound(ex.Message);
                }
            }
            return View(s);
        }

        public ActionResult SquareEdit(int? id)//+
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                SquareBll s = _figuresService.GetSquareById(id.Value);
                SquareCreatingAndEditingView cv = new SquareCreatingAndEditingView() { Id = s.Id, Name = s.Name, Side = s.Side.ToString() };
                return View(cv);
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
        public ActionResult SquareEdit(SquareCreatingAndEditingView s)//+
        {
            if (ModelState.IsValid)
            //Range validation where value is with comma and not with dot
            //https://laracasts.com/discuss/channels/general-discussion/validate-numeric-with-both-comma-and-dot-nottation?page=1
            {
                try
                {
                    _figuresService.UpdateSquare(new SquareBll(null, s.Id, s.Name, float.Parse(s.Side)));
                    return RedirectToAction("Squares");
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
            return View(s);
        }

        [HttpPost]
        public ActionResult SquareDelete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                _figuresService.DeleteSquare(id.Value);
                return RedirectToAction("Squares");
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

        #endregion

        protected override void Dispose(bool disposing)
        {
            _figuresService.Dispose();
            base.Dispose(disposing);
        }
    }
}