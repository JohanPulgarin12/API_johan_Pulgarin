using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Core
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }
        public string DocNum { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telf { get; set; }
        public string Email { get; set; }
    }
}
