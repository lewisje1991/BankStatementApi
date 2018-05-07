using BankStatementApi.Mappers;
using CsvHelper;
using System.IO;

namespace BankStatementApi.Factories
{
    public class CsvReaderFactory
    {
        public CsvReader GetInstance(StreamReader file, string bankName)
        {
            var csvReader = new CsvReader(file);

            switch (bankName)
            {
                case "BankOfScotland":
                    csvReader.Configuration.RegisterClassMap<BankOfScotlandCsvMapper>();
                    break;
                default:
                    break;
            }

            return csvReader;
        }
    }

}