using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IAuthorizeInput<TData>
    {
        IClaims Claims { get; }

        TData Data { get; set; }
    }
}
