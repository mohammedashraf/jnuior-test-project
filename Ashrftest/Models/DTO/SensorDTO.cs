using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Models.DTO
{
    public class SensorDTO
    {
        [Key]
        public int SensorID { get; set; }
        public string SensorType { get; set; }
        public SensorValue SensorValue { get; set; }
        public double SensorMaxValue { get; set; }
        public double SensorMinValue { get; set; }
        public double DataTime { get; set; }

       
    }
}
