using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ashrftest.Models
{
    public class Measurement
    {
        [Key]
        public int MeasurementID { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Last Name must be less than or equal 20 chars only.")]
        [Display(Name = "Sensor Type")]
        public string SensorType { get; set; }

        [Required]
        [Range(0, 7000)]
        [Display(Name = "Measurement Value")]
        public float MeasurementValue { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        public int SensorID { get; set; }
    }
}
