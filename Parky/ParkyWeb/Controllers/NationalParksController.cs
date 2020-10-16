using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View( new NationalPark() { });
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();

            if(id == null)
            {
                //Insert Or create
                return View(obj);
            }
            obj = await _npRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());

            if(obj == null)
            {
                //Edit or Update
                return NotFound();
            }
            return View(obj);
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            //WorkFlow

            var model = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            return Json(new { data = model});
        }
    }
}
