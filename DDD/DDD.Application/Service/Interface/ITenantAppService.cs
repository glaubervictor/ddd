using DDD.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Service.Interface
{
    public interface ITenantAppService : IDisposable
    {
        void Add(TenantDTO tenantDTO);
        void GetAll();
    }
}
