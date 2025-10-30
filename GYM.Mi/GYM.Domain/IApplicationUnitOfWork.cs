using GYM.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain
{
    public interface IApplicationUnitOfWork:IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
    }
}
