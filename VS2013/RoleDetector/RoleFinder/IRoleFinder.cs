using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleFinder
{
    public interface IRoleFinder
    {
        Role[] FindRole(string[] parsedSkills);
    }
}
