using Stripe;
using System;
using System.Collections.Generic;

namespace SpiderFleetStripe.Stripe.Customers
{
    public class CustomerStripe
    {

        /// <summary>
        /// Creacion de Usuario de Stripe con datos minimos
        /// </summary>
        /// <param name="email"></param>
        /// <param name="drescription"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Customer Cretae(string key, string email, string drescription, string name, string phone)
        {
            Customer customer = new Customer();
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new CustomerCreateOptions
                {
                    Email = email,
                    Description = drescription,
                    Name = name,
                    //Address = new AddressOptions
                    //{
                    //    Line1 = "",
                    //    Line2 = "",
                    //    City = "Puebla",
                    //    Country = "Mexico",                        
                    //    PostalCode = "72000",
                    //    State = "Puebla"
                    //},
                    Phone = phone,
                    //Metodo de Pago
                    //PaymentMethod = "pm_1FWS6ZClCIKljWvsVCvkdyWg",
                    //InvoiceSettings = new CustomerInvoiceSettingsOptions
                    //{
                    //    DefaultPaymentMethod = "pm_1FWS6ZClCIKljWvsVCvkdyWg",
                    //},

                };
                var service = new CustomerService();
                customer = service.Create(options);

                return customer;
            }
            catch (Exception ex)
            {
                return customer;
            }
        }


        public Customer Update(string key, string idCustomer)
        {
            Customer customer = new Customer();
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new CustomerUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                      {
                        { "order_id", "6735" },
                      },
                };
                var service = new CustomerService();
                service.Update(idCustomer, options);

                return customer;
            }
            catch (Exception ex)
            {
                return customer;
            }
        }

        /// <summary>
        /// Metodo que regresa los datos del cliente por id
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        public Customer ReadId(string key, string idCustomer)
        {
            Customer customer = new Customer();
            try
            {
                StripeConfiguration.ApiKey = key;
                var service = new CustomerService();
                customer = service.Get(idCustomer);

                return customer;
            }
            catch (Exception ex)
            {
                return customer;
            }
        }

        /// <summary>
        /// Metodo que regresa la lisa de todos los usuarios  
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Read(string key)
        {
            var service = new CustomerService();
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new CustomerListOptions
                {
                    Limit = 10,
                };
                
                //service.List(options);

                return service.List(options);
            }
            catch (Exception ex)
            {
                return service;
            }
        }
    }
}