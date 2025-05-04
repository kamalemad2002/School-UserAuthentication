using School.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Services
{
    public class PassServices
    {
        public static void Reset(string email,string newPass)
        {
        
            FileManager.UpdateUserPassword(email, School.Helpers.HashedPasswordSHA256.HashPassword(newPass));
            Console.WriteLine("Password reset successfully.");
        }
        public static void Change(string email, string currentPass, string newPass)
        {
            var users = FileManager.LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null && user.Password == School.Helpers.HashedPasswordSHA256.HashPassword(currentPass))
            {
                FileManager.UpdateUserPassword(email, School.Helpers.HashedPasswordSHA256.HashPassword(newPass));
            }
            else
            {
                throw new Exception("Incorrect current password.");
            }
        }

    }
}
