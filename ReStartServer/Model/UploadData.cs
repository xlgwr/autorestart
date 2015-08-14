using System;
using System.ComponentModel.DataAnnotations;

namespace ReStartServer.Model
{
    public class UploadData : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(75)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(75)]
        public string DeviceCd { get; set; }

        [Required]
        [StringLength(32)]
        public string Groupstamp { get; set; }

        [Required]
        public DateTime PowerDate { get; set; }

        [Required]
        public double PowerValue { get; set; }

        [Required]
        public int ValueLevel { get; set; }

        [Required]
        public double MeterValue { get; set; }

        //add by xlg 2015-07-05
        //[Required]
        //[JsonIgnore]
        public double DiffMeterValuePre { get; set; }
        //[Required]
        //[JsonIgnore]
        public DateTime PrePowerDate { get; set; }
        //end by xlg

        [Required]
        public double A1 { get; set; }

        [Required]
        public double A2 { get; set; }

        [Required]
        public double A3 { get; set; }

        [Required]
        public double V1 { get; set; }

        [Required]
        public double V2 { get; set; }

        [Required]
        public double V3 { get; set; }

        [Required]
        public double Pf { get; set; }

        [Required]
        public int Uploaded { get; set; }
    }
}