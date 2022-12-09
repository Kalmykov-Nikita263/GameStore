using GameStore.Domain;
using GameStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GameController : Controller
    {
        private readonly DataManager _dataManager;

        public GameController(DataManager dataManager) => _dataManager = dataManager;

        public IActionResult Edit(Guid id)
        {
            var entity = _dataManager.Games.GetGameById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(Game model) 
        { 
            if(ModelState.IsValid) 
            { 
                _dataManager.Games.SaveGame(model);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(model);
        }

        public IActionResult AddNewGame()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewGame(Game model)
        {
            if (ModelState.IsValid)
            {
                _dataManager.Games.SaveGame(model);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            _dataManager.Games.DeleteGame(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}