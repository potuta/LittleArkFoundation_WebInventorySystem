﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Models;
using LittleArkFoundation_WebInventorySystem.Data.Repositories;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ConnectionService _connectionService;

        public UsersController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index(string dbType, bool isArchive)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);

            await using (var context = new ApplicationDbContext(connectionString))
            {
                var users = await context.Users.ToListAsync();

                var viewModel = new UsersViewModel
                {
                    Users = users,
                    NewUser = new UsersModel(),
                    Roles = new RolesRepository(_connectionService).GetRoles(dbType)
                };

                ViewBag.isArchive = isArchive;
                return View(viewModel);
            }
        }
        
        // CREATE: Create a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string dbType, UsersViewModel viewModel)
        {
            //Console.WriteLine($"viewModel is null: {viewModel == null}");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                    }
                }
                return View("Index", viewModel);
            }

            string connectionString = _connectionService.GetConnectionString(dbType);

            using (var context = new ApplicationDbContext(connectionString))
            {
                viewModel.NewUser.CreatedAt = DateTime.Now;
                context.Users.Add(viewModel.NewUser);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // 🟢 READ: Show details
        public async Task<IActionResult> Details(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);

            await using (var context = new ApplicationDbContext(connectionString))
            {
                var user = await context.Users.FindAsync(id);
                if (user == null) return NotFound();
                return View(user);
            }

        }

        // EDIT
        public async Task<IActionResult> Edit(string dbType, int id)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);
            using (var context = new ApplicationDbContext(connectionString))
            {
                var user = await context.Users.FindAsync(id);
                if (user == null) return NotFound();

                var viewModel = new UsersViewModel
                {
                    Users = new List<UsersModel>(),
                    NewUser = user,
                    Roles = new RolesRepository(_connectionService).GetRoles(dbType)
                };

                return View(viewModel);
            }
        }

        // 🔵 UPDATE: Save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string dbType, UsersViewModel user)
        {
            Console.WriteLine($"viewModel is null: {user == null}");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                    }
                }
                user.Roles = new RolesRepository(_connectionService).GetRoles(dbType);
                return View("Index", user);
            }

            string connectionString = _connectionService.GetConnectionString(dbType);

            using (var context = new ApplicationDbContext(connectionString))
            {
                //context.Entry(user).State = EntityState.Modified;
                context.Update(user.NewUser);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
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
