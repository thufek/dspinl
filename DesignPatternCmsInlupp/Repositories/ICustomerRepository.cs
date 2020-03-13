using DesignPatternCmsInlupp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternCmsInlupp.Repositories
{
    interface ICustomerRepository
    {
        void SaveCustomer(Customer customer);
        void SetInvoices(Customer customer);
        void SetPayments(Customer customer);
        void SetLoans(Customer customer);
        Customer FindCustomer(string personnummer);
        List<Customer> GetCustomers();
    }
}
