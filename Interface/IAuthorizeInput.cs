﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerDatabase.Interface
{
    public interface IAuthorizeInput<TData, TUser> where TUser : IUser
    {
        IClaims<TUser> Claims { get; }//令牌

        TData Data { get; set; }
    }
}
