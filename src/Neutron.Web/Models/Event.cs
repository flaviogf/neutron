using System;

namespace Neutron.Web.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Target { get; set; }
    }
}
