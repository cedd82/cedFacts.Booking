﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace FACTS.GenericBooking.Repository.Postgres.Entities
{
    [Table("process_event_data")]
    public partial class ProcessEventData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("user_code")]
        [StringLength(50)]
        public string UserCode { get; set; }
        [Required]
        [Column("app_name")]
        [StringLength(50)]
        public string AppName { get; set; }
        [Required]
        [Column("event_name")]
        [StringLength(50)]
        public string EventName { get; set; }
        [Required]
        [Column("process_url")]
        [StringLength(150)]
        public string ProcessUrl { get; set; }
        [Required]
        [Column("process_data", TypeName = "jsonb")]
        public string ProcessData { get; set; }
        
        [Column("process_data_output", TypeName = "jsonb")]
        public string ProcessDataOutput { get; set; }
        [Required]
        [Column("status_code")]
        [StringLength(50)]
        public string StatusCode { get; set; }
        [Required]
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
    }
}