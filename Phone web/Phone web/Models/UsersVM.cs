using Phone_web.Models;
using System;
using System.Collections.Generic;

namespace Phone_web.Models
{

    public partial class UsersVM

    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersVM()
        {
            this.Customers = new HashSet<Customer>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}