using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdApp.Models;

namespace AdApp.Core.Helpers.User
{
    public interface IUserHelper
    {
        List<Models.User> GetAllUsers();
    }
}
