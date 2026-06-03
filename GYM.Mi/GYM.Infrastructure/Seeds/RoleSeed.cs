using GYM.Mi.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Seeds
{
    public  class RoleSeed
    {
        public static ApplicationRole[] GetRoles()
        {
            return [
                  new ApplicationRole
                  {
                    Id =new Guid("001D9A62-8105-4FAD-9EF8-5271265734BD"),
                    Name = "Admin",
                    NormalizedName="ADMIN",
                    ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 1).ToString(),

                  },
                  new ApplicationRole
                  {
                       Id =new Guid("EAE0E7FE-1820-454B-B88B-9AC89F711D84"),
                      Name = "Manager",
                      NormalizedName="MANAGER",
                      ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 3).ToString(),

                  },
                  new ApplicationRole
                  {
                      Id =new Guid("056988A3-D645-4C7A-A453-BC0CAE9F1748"),
                     Name = "Trainer",
                     NormalizedName="TRAINER",
                    ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 4).ToString(),

                  },
                  new ApplicationRole{
                     Id =new Guid("7D6739D9-44FA-4236-ABB6-B2D855E3657C"),
                     Name="User",
                     NormalizedName="USER",
                     ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 5).ToString(),

                  }
            ];

        }
    }
}
