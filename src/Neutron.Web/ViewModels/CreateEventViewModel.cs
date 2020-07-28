using System;

namespace Neutron.Web.ViewModels
{
    public class CreateEventViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? Target { get; set; }
    }
}
