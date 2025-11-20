using CORE.APP.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Domain
{
    public class Role : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        
        public List<UserRole> UserRoles { get; set; }
    }
}
