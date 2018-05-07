using System.Collections.Generic;

namespace BankStatementApi.DTOs
{
    public class LoginRequestDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}