using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CustomerInvoiceGroupRepository : ICustomerInvoiceGroupRepository
    {
        private readonly ITrexContextProvider _entityContext;

        public CustomerInvoiceGroupRepository(ITrexContextProvider contextProvider)
        {
            _entityContext = contextProvider;
        }

        public List<CustomerInvoiceGroup> GetCustomerInvoiceGroupIDByCustomerID(Customer customer)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var data = (from cig in db.CustomerInvoiceGroups
                            where cig.CustomerID == customer.CustomerID
                            select cig).ToList();

                foreach (var cig in data)
                {
                    if (cig.City == null)
                        cig.City = customer.City;

                    if (cig.Address1 == null)
                        cig.Address1 = customer.StreetAddress;

                    if (cig.Address2 == null)
                        cig.Address2 = customer.Address2;

                    if (cig.Attention == null)
                        cig.Attention = customer.ContactName;

                    if (cig.ZipCode == null)
                        cig.ZipCode = customer.ZipCode;

                    if (cig.Country == null)
                        cig.Country = customer.Country;

                    if (cig.SendFormat == 0)
                        cig.SendFormat = customer.SendFormat;

                    if (cig.Email == null)
                        cig.Email = customer.Email;

                    if (cig.EmailCC == null)
                        cig.EmailCC = customer.EmailCC;
                }

                return data;
            }

        }

        public int GetCustomerIdByCustomerInvoiceGroupId(int customerInvoiceGroupId)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var customerInvoiceGroup =
                    db.CustomerInvoiceGroups.SingleOrDefault(
                        cig => cig.CustomerInvoiceGroupID == customerInvoiceGroupId);
                if (customerInvoiceGroup != null)
                    return customerInvoiceGroup.CustomerID;
                return -20;
            }
        }

        public void InsertInDatabase(CustomerInvoiceGroup group)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                db.CustomerInvoiceGroups.ApplyChanges(group);

                db.SaveChanges(SaveOptions.DetectChangesBeforeSave | SaveOptions.AcceptAllChangesAfterSave);
            }
        }

        public void DeleteInDatabase(int CustomerInvoiceGroupId)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var cig = db.CustomerInvoiceGroups.SingleOrDefault(
                    c => c.CustomerInvoiceGroupID == CustomerInvoiceGroupId);

                //What about all the projects refering to this?
                //Find all projects to this group - foreach change the reference to something else
                db.CustomerInvoiceGroups.DeleteObject(cig);

                db.SaveChanges();
            }
        }

        public CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceGroupID(int customerInvoiceGroupId)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                return db.CustomerInvoiceGroups
                    .First(cig => cig.CustomerInvoiceGroupID == customerInvoiceGroupId);
            }
        }

        public void OverwriteCig(CustomerInvoiceGroup cig)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var cigdata = (from entity in db.CustomerInvoiceGroups
                               where entity.CustomerInvoiceGroupID == cig.CustomerInvoiceGroupID
                               select entity).First();

                cigdata.Attention = cig.Attention;
                cigdata.City = cig.City;
                cigdata.Email = cig.Email;
                cigdata.EmailCC = cig.EmailCC;
                cigdata.Country = cig.Country;
                cigdata.ZipCode = cig.ZipCode;
                cigdata.Address1 = cig.Address1;
                cigdata.Address2 = cig.Address2;
                cigdata.SendFormat = cig.SendFormat;
                cigdata.DefaultCig = cig.DefaultCig;
                cigdata.Label = cig.Label;

                db.CustomerInvoiceGroups.ApplyChanges(cigdata);
                db.SaveChanges();
            }
        }

        public ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var p = db.Projects;
                var i = db.Invoices;

                var tn = (from d in i
                          select d.CustomerInvoiceGroupId);

                var pr = (from d in p
                          select d.CustomerInvoiceGroupID);

                if (tn.Contains(customerInvoiceGroupId) || pr.Contains(customerInvoiceGroupId))
                    return
                        new ServerResponse(
                            "You can't delete this cig, there are still projects and/or invoices that uses it \n" +
                            "delete them, before you can delete this cig", false);
                DeleteInDatabase(customerInvoiceGroupId);
            }
            return new ServerResponse("CustomerInvoiceGroup deleted", true);

        }
    }
}