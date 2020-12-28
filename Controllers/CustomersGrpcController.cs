using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using GrpcCustomersService;


namespace Cocis_Giorgia_Lab2.Controllers
{
    public class CustomersGrpcController : Controller
    {
        private readonly GrpcChannel channel;
        public CustomersGrpcController()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
        }
        [HttpGet]
        public IActionResult Index()
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            CustomerList cust = client.GetAll(new Empty());
            return View(cust);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);
                var createdCustomer = client.Insert(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Details(CustomerId customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            Customer cust = client.Get(customerId);
               
            return View(cust);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(CustomerId customerId)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            Customer cust = client.Get(customerId);

            if (cust == null)
            {
                return NotFound();
            }
           return View(cust);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Customer customer)
        {
            if (customer == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            CustomerId customerId = new CustomerId
            {
                Id = customer.CustomerId
            };
            Customer cust = client.Get(customerId);

            if (cust == null)
            {
                return NotFound();
            }

            cust.Name = customer.Name;
            cust.Adress = customer.Adress;
            cust.Birthdate = customer.Birthdate;
            client.Update(cust);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(CustomerId customerId)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            Customer cust = client.Get(customerId);

            if (cust == null)
            {
                return NotFound();
            }
            return View(cust);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Customer customer)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerId customerId = new CustomerId
            {
                Id = customer.CustomerId
            };
            
            client.Delete(customerId);
            return RedirectToAction(nameof(Index));
        }
    }
}