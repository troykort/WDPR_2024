using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _dbContext;

        public EmployeeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RegisterEmployeeAsync(string email, string name)
        {
            // Haal domein op uit het e-mailadres
            var emailDomain = email.Split('@').Last();

            // Zoek het bijbehorende bedrijf
            var businessCustomer = await _dbContext.BusinessCustomers
                .FirstOrDefaultAsync(bc => bc.Domain == emailDomain);

            if (businessCustomer == null)
            {
                throw new Exception("No company found with this email domain.");
            }

            // Maak een nieuwe werknemer aan
            var employee = new Employee
            {
                Name = name,
                Email = email,
                BusinessCustomerId = businessCustomer.Id
            };

            // Voeg toe aan de database
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}

