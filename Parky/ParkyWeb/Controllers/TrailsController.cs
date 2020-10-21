using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository;
using ParkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _npTrail;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository npTrail)
        {
            _npRepo = npRepo;
            _npTrail = npTrail;
        }

        public IActionResult Index()
        {
            return View( new Trail() { });
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if(id == null)
            {
                //Insert Or create
                return View(objVM);
            }
            objVM.Trail = await _npTrail.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());

            if(objVM.Trail == null)
            {
                //Edit or Update
                return NotFound();
            }
            return View(objVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
                if(obj.Trail.Id == 0)
                {
                    await _npTrail.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _npTrail.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            //WorkFlow

            var model = await _npTrail.GetAllAsync(SD.TrailAPIPath);
            return Json(new { data = model});
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npTrail.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Deleted Successfull" });
            }
            return Json(new { success = false, message = "Delete not Successfull" });
        }
    }
}
