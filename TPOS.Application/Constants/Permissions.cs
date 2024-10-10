using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOS.Application.Constants
{
    public static class Permissions
    {
        public static class Company
        {
            public const string View = "company.view";
            public const string Create = "company.create";
            public const string Modify = "company.edit";
            public const string Delete = "company.delete";
        }

        public static class Branch
        {
            public const string View = "branch.view";
            public const string Create = "branch.create";
            public const string Modify = "branch.edit";
            public const string Delete = "branch.delete";
        }

        public static class Customer
        {
            public const string View = "customer.view";
            public const string Create = "customer.create";
            public const string Modify = "customer.edit";
            public const string Delete = "customer.delete";
        }

        public static class User
        {
            public const string View = "user.view";
            public const string Create = "user.create";
            public const string Modify = "user.edit";
            public const string Delete = "user.delete";
        }

        public static class Role
        {
            public const string View = "role.view";
            public const string Create = "role.create";
            public const string Modify = "role.edit";
            public const string Delete = "role.delete";
            public const string Assign = "role.assign";
        }
    }
}
