using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Dto
{
    public class UserDeleteDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
