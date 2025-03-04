﻿using System.ComponentModel.DataAnnotations;

namespace Entities.Dto.User
{
    public class UserForUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }
    }
}
