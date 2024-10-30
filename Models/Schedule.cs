﻿using System.ComponentModel.DataAnnotations;

namespace time_trace.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public Guid? Sharecode { get; set; } = Guid.NewGuid();

        [Required]
        public ApplicationUser Owner { get; set; }
        public string? OwnerId { get; set; }
        public List<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();

    }
}
