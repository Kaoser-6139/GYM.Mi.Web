using GYM.Domain;
using GYM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository) : base(dbContext)
        {
            UserRepository =userRepository;
        }

        public IUserRepository UserRepository { get; private set; }
    }
}
