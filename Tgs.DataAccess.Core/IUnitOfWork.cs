using System;
using System.Collections.Generic;
using System.Text;
using Tgs.DataAccess.Core.Context;

namespace Tgs.DataAccess.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }
        void Commit();
    }
}
