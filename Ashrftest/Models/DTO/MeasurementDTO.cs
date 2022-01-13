using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Models.DTO
{
    public class MeasurementDTO
    {
        [Key]
        public int MeasurementID { get; set; }
        //public int SensorID { get; set; }
        public string SensorType { get; set; }

        public float MeasurementValue { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
    }
}
