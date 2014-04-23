using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly IAppSettings _appSettings;
        private readonly IInvoiceLineFactory _invoiceLineFactory;
        private readonly IInvoiceRepository _invoiceRepository;
        private ICustomerRepository _customerRepository;
        private IInvoiceFactory _invoiceFactory;
        private ITimeEntryRepository _timeEntryRepositoy;

        public InvoiceManager(IInvoiceRepository invoiceRepository, IInvoiceFactory invoiceFactory, IInvoiceLineFactory invoiceLineFactory, ICustomerRepository customerRepository,
                              ITimeEntryRepository timeEntryRepositoy, IAppSettings appSettings)
        {
            _invoiceFactory = invoiceFactory;
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _timeEntryRepositoy = timeEntryRepositoy;
            _appSettings = appSettings;
            _invoiceLineFactory = invoiceLineFactory;
        }

        #region IInvoiceManager Members

        public void BookTimeEntries(Invoice invoice, List<int> timeEntryList)
        {
            var connection = new SqlConnection(_appSettings.AppConnectionString);

            var command = new SqlCommand("spBookTimeEntries", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

            command.Parameters.Add(new SqlParameter("invoiceId", invoice.ID));

            if (timeEntryList != null || timeEntryList.Count != 0)
            {
                //Convert the list to a comma separated list
                var includeList = string.Join(",", timeEntryList.ConvertAll(t => t.ToString()).ToArray());
                command.Parameters.Add(new SqlParameter("timeEntryList", includeList));
            }

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                GenerateInvoiceLines(invoice);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("DB error when using spBookTimeEntries", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void ResetInvoice(Invoice invoice)
        {
            ResetBookedTimeEntries(invoice);
        }

        public XmlDocument SerializeInvoice(Invoice invoice)
        {
            {
                var invoiceSerializer = new XmlSerializer(invoice.GetType());
                var xDocument = new XmlDocument();

                using (var writer = new StringWriter())
                {
                    invoiceSerializer.Serialize(writer, invoice);
                    xDocument.LoadXml(writer.ToString());
                }

                using (var strWriter = new StringWriter())
                {
                    var invoiceLinesDoc = new XmlDocument();
                    var invoiceLineSerializer = new XmlSerializer(typeof (List<InvoiceLine>));

                    var invoiceLines = invoice.InvoiceLines.ToList();

                    invoiceLineSerializer.Serialize(strWriter, invoiceLines);
                    invoiceLinesDoc.LoadXml(strWriter.ToString());

                    var invoiceLineNode = xDocument.ImportNode(invoiceLinesDoc.DocumentElement, true);
                    xDocument.DocumentElement.AppendChild(invoiceLineNode);
                }
                return xDocument;
            }
        }

        #endregion

        private void ResetBookedTimeEntries(Invoice invoice)
        {
            var connection = new SqlConnection(_appSettings.AppConnectionString);
            var command = new SqlCommand("spResetBookedTimeEntries", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

            command.Parameters.Add(new SqlParameter("invoiceId", invoice.ID));
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("DB error when using spResetBookedTimeEntries", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        private void GenerateInvoiceLines(Invoice invoice)
        {
            var connection = new SqlConnection(_appSettings.AppConnectionString);
            var command = new SqlCommand("spGetGeneratedInvoiceLines", connection)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };

            command.Parameters.Add(new SqlParameter("invoiceId", invoice.ID));

            IDataReader reader = null;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var units = reader.GetDouble(reader.GetOrdinal("TimeSpent"));
                        var pricePrUnit = reader.GetDouble(reader.GetOrdinal("price"));

                        //TODO: Get hour text from resource file

                        var invoiceLine = _invoiceLineFactory.Create(invoice, units, pricePrUnit, "timer", string.Empty, InvoiceLine.UnitTypes.Hours, false, 0.25);
                        invoice.InvoiceLines.Add(invoiceLine);
                    }
                }
                _invoiceRepository.Update(invoice);
            }

            catch (SqlException ex)
            {
                throw new DataAccessException("DB error when using spGetGeneratedInvoiceLines", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connection.Close();
            }
        }
    }
}