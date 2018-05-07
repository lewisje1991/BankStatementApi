using System.Collections.Generic;
using System.IO;
using BankStatementApi.Factories;

namespace BankStatementApi.Services
{
    public class CsvService : ICsvService
    {
        private readonly CsvReaderFactory csvFactory;

        public CsvService(CsvReaderFactory csvFactory)
        {
            this.csvFactory = csvFactory;
        }

        public List<T> CsvToDtoList<T>(MemoryStream postedFile, string bankName)
        {
            var csvReader = csvFactory.GetInstance(new StreamReader(postedFile), bankName);

            var records = new List<T>();

            while (csvReader.Read())
            {
                records.Add(csvReader.GetRecord<T>());
            }

            return records;
        }
    }
}

