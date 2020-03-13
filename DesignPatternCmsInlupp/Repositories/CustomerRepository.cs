using System.Web.Mvc;
using DesignPatternCmsInlupp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DesignPatternCmsInlupp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer FindCustomer(string personnummer)
        {
            Customer customer = null;
            string databas = HttpContext.Current.Server.MapPath("~/customers.txt");
            foreach (var line in System.IO.File.ReadAllLines(databas))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 1) continue;
                if (parts[0] == personnummer)
                    if (customer == null)
                        customer = new Customer { PersonNummer = personnummer };
            }
            if (customer == null) return null;
            SetLoans(customer);
            SetInvoices(customer);
            SetPayments(customer);
            return customer;
        }

        public List<Customer> GetCustomers()
        {
            var model = new List<Customer>();
            string databas = HttpContext.Current.Server.MapPath("~/customers.txt");
            foreach (var line in System.IO.File.ReadAllLines(databas))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 1) continue;
                var customer = new Customer { PersonNummer = parts[0] };
                model.Add(customer);
            }
            return model;
        }

        public void SaveCustomer(Customer customer)
        {
            string databas = HttpContext.Current.Server.MapPath("~/customers.txt");
            var allLines = System.IO.File.ReadAllLines(databas).ToList();
            foreach (var line in allLines)
            {
                string[] parts = line.Split(';');
                if (parts.Length < 1) continue;
                if (parts[0] == customer.PersonNummer)
                    return;
            }
            allLines.Add(customer.PersonNummer);
            System.IO.File.WriteAllLines(databas, allLines);
        }

        public void SetInvoices(Customer customer)
        {
            string databas = HttpContext.Current.Server.MapPath("~/invoices.txt");
            foreach (var line in System.IO.File.ReadAllLines(databas))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 2) continue;
                var loan = customer.Loans.FirstOrDefault(r => r.LoanNo == parts[0]);
                if (loan == null) continue;
                var invoice = new Invoice
                {
                    InvoiceNo = Convert.ToInt32(parts[1]),
                    Belopp = Convert.ToInt32(parts[2]),
                    InvoiceDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DueDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                };
                loan.Invoices.Add(invoice);
            }
        }

        public void SetLoans(Customer customer)
        {
            string databas = HttpContext.Current.Server.MapPath("~/loans.txt");
            foreach (var line in System.IO.File.ReadAllLines(databas))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 2) continue;
                if (parts[0] == customer.PersonNummer)
                {
                    var loan = new Loan
                    {
                        LoanNo = parts[1],
                        Belopp = Convert.ToInt32(parts[2]),
                        FromWhen = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        InterestRate = Convert.ToDecimal(parts[4])
                    };

                    customer.Loans.Add(loan);
                }
            }
        }

        public void SetPayments(Customer customer)
        {
            string databas = HttpContext.Current.Server.MapPath("~/payments.txt");
            foreach (var line in System.IO.File.ReadAllLines(databas))
            {
                string[] parts = line.Split(';');
                if (parts.Length < 2) continue;
                var invoice = customer.Loans.SelectMany(r => r.Invoices).FirstOrDefault(i => i.InvoiceNo == Convert.ToInt32(parts[0]));
                if (invoice == null) continue;
                var payment = new Payment
                {
                    Belopp = Convert.ToInt32(parts[1]),
                    PaymentDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    BankPaymentReference = parts[3],
                };
                invoice.Payments.Add(payment);
            }
        }
    }
}