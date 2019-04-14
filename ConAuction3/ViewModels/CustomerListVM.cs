using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels
{
    class CustomerListVM {
        private readonly List<Customer> _customers;

        public CustomerListVM(List<Customer> customers) {
            _customers = customers;
        }


        public ObservableCollection<Customer> ObservableCustomers => new ObservableCollection<Customer>(_customers);

        public Customer GetCustomerFromId(int id) {
            return _customers.Find(c => c.Id == id);
        }

    }
}
