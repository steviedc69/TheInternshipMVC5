﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Internship.ViewModels
{
    public class ManageModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig nieuw wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord en de bevestiging komen niet overeen")]
        public string ConfirmPassword { get; set; }
    }
}