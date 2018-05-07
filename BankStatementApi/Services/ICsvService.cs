using System.Collections.Generic;
using System.IO;
using System.Web;

namespace BankStatementApi.Services
{
    public interface ICsvService
    {
        List<T> CsvToDtoList<T>(MemoryStream postedFile, string bankName);
    }
}