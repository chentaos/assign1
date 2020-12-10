using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models.ViewModel
{
    public class StudentMembershipViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<CommunityMembersipViewModel> Memberships { get; set; }
    }
}
