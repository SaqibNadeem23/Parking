using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Parking.Model
{
    [Table("Users")]
    class Users
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }     

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string UserType { get; set; }

        [MaxLength(50)]
        public string StatusId { get; set; }

        [MaxLength(50)]
        public string VehicleSelected { get; set; }
    }
}
