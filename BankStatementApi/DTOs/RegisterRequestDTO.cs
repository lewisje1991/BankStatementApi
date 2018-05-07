using System.Collections.Generic;

namespace BankStatementApi.DTOs
{
    public class RegisterRequestDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public Dictionary<string, string> attributes { get; set; }
    }
}