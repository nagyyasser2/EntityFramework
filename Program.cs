using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DatabaseFirstApproachEntities())
            {
                var newUser = new Users()
                {
                    id = 1,
                    name = "test",
                    phone = "44"
                };

                context.Users.Add(newUser);

                context.SaveChanges();
            }
        }
    }
}
