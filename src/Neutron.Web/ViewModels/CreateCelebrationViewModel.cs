using System;
using System.ComponentModel.DataAnnotations;

namespace Neutron.Web.ViewModels
{
    public class CreateCelebrationViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 99)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }
    }
}
