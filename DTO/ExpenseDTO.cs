﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ExpenseDTO
    {
        [Required]
        public string ? Description { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
