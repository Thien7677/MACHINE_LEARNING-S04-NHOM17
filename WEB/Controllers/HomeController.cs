using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ: Menu tối giản, chỉ có các nút chọn Dataset để xem
        public ActionResult Index()
        {
            return View();
        }

        // Trang 1: Xem Tổng quan & Pipeline thực nghiệm
        public ActionResult Overview()
        {
            var summary = WaterDataService.GetSummary();
            return View(summary);
        }

        // Trang 2: Xem chi tiết Dataset 1 (Hóa - Lý)
        public ActionResult Dataset1()
        {
            var model = WaterDataService.GetDatasetWithSamples("dataset1", 200);
            return View(model);
        }

        // Trang 3: Xem chi tiết Dataset 2 (Vi sinh & Kim loại)
        public ActionResult Dataset2()
        {
            var model = WaterDataService.GetDatasetWithSamples("dataset2", 200);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetTableColumns(string id)
        {
            var (headers, _) = WaterDataService.GetCsvData(id ?? "dataset1");
            return Json(headers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTableData(string id, int draw = 1, int start = 0, int length = 10)
        {
            string searchValue = Request["search[value]"] ?? string.Empty;
            var response = WaterDataService.QueryCsvData(id ?? "dataset1", draw, start, length, searchValue);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
