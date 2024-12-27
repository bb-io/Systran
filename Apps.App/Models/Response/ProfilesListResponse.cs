using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Apps.Systran.Models.Response
{
    public class ProfilesListResponse
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }
    }

    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Activated { get; set; }
        public bool Running { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string SharingStatus { get; set; }
        public string Owner { get; set; }
    }
}
