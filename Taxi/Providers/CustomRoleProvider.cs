using System;
using System.Linq;
using System.Web.Security;
using Taxi.Models;

namespace Taxi.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string callSign, string roleName)
        {
            bool outputResult = false;
            using (TaxiDBEntities db = new TaxiDBEntities())
            {
                try
                {
                    Employees user = db.Employees.FirstOrDefault(u => u.CallSign == callSign);
                    if (user != null)
                    {
                        RoleTable userRole = db.RoleTable.Find(user.IdRole);
                        if (userRole != null && userRole.Name == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
            }
            return outputResult;
        }

        public override string[] GetRolesForUser(string CallSign)
        {
            string[] role = new string[] { };
            using (TaxiDBEntities db = new TaxiDBEntities())
            {
                try
                {
                    Employees user = db.Employees.FirstOrDefault(u => u.CallSign == CallSign);
                    if (user != null)
                    {
                        RoleTable userRole = db.RoleTable.Find(user.IdRole);
                        if (userRole != null)
                        {
                            role = new[] { userRole.Name };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }
            return role;
        }

        public override void CreateRole(string roleName)
        {
            RoleTable newRole = new RoleTable() { Name = roleName };
            TaxiDBEntities db = new TaxiDBEntities();
            db.RoleTable.Add(newRole);
            db.SaveChanges();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}