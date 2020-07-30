using System;
using System.ComponentModel.DataAnnotations;

namespace Neutron.Web.ViewModels
{
    public class CreateEventViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? Target { get; set; }
    }
}
