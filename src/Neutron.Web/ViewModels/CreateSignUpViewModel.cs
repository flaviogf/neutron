﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Neutron.Web.ViewModels
{
    public class CreateSignUpViewModel
    {
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
