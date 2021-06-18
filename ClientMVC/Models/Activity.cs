using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.Models
{
    public class Activity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string HostUsername { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<Profile> Attendees { get; set; } = new List<Profile>();

        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public Profile Host { get; set; }
    }
}
