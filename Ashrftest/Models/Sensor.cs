using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Models
{
    public enum SensorValue
    {
        Low, Medium, High
    }

    public class Sensor
    {
        [Key]
        public int SensorID { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Last Name must be less than or equal 20 chars only.")]
        [Display(Name = "Sensor Type")]

        public string SensorType { get; set; }
        public SensorValue SensorValue { get; set; }
        [Required]
        [Range(0, 10000)]
        public double SensorMaxValue { get; set; }
        [Required]
        [Range(0, 7000)]
        public double SensorMinValue { get; set; }
        [Required]
        public double DataTime { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}
