using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace Parking.Model
{
    [Table("Parkings")]
    class Parkings
    {
        [PrimaryKey, NotNull, AutoIncrement]
        public int ParkingId { get; set; }

        [MaxLength(200)]
        public string ParkingName { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }
        
        [MaxLength(200)]
        public string ParkingLatitude { get; set; }
        
        [MaxLength(200)]
        public string ParkingLongitude { get; set; }

        [MaxLength(50)]
        public string ParkingCarSlots { get; set; }

        [MaxLength(50)]
        public string ParkingCarFees { get; set; }

        [MaxLength(50)]
        public string ParkingBikeSlots { get; set; }

        [MaxLength(50)]
        public string ParkingBikeFilled { get; set; }

        [MaxLength(50)]
        public string ParkingCarFilled { get; set; }

        [MaxLength(50)]
        public string ParkingBikeFees { get; set; }

        [MaxLength(20)]
        public byte[] imgbyte { get; set; }

    }
}
