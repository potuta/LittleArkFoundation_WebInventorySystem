﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Models;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ConnectionService _connectionService;

        public UsersController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index(string dbType)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);

            await using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.ToListAsync();
                var viewModel = new UsersViewModel
                {
                    Users = users ?? new List<UsersModel>(),
                    NewUser = new UsersModel()
                };
                return View(viewModel);
            }
        }
            
        // 🔵 CREATE: Create a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string dbType, UsersViewModel viewModel)
        {
            //if (ModelState.IsValid)
            //{

            //}
                string connectionString = _connectionService.GetConnectionString(dbType);

                using (var context = new ApplicationDbContext(connectionString))
                {
                    viewModel.NewUser.CreatedAt = DateTime.Now;
                    context.Users.Add(viewModel.NewUser);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index), new { dbType });
            

            //foreach (var modelState in ModelState.Values)
            //{
            //    foreach (var error in modelState.Errors)
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //}

            //return View("Index", viewModel);
        }

        // 🟢 READ: Show details
        public async Task<IActionResult> Details(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);

            using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.FindAsync(id);
                if (users == null) return NotFound();
                return View(users);
            }
            
        }

        // 🟠 UPDATE: Show edit form
        public async Task<IActionResult> Edit(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.FindAsync(id);
                if (users == null) return NotFound();
                return View(users);
            }
            
        }

        // 🔵 UPDATE: Save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string dbType, UsersModel user)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                if (ModelState.IsValid)
                {
                    context.Entry(user).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(user);
            }
        }

        // 🔴 DELETE: Show confirmation page
        public async Task<IActionResult> Delete(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.FindAsync(id);
                if (users == null) return NotFound();
                return View(users);
            }
        }

        // 🔴 DELETE: Remove the product from DB
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.FindAsync(id);
                context.Users.Remove(users);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}
