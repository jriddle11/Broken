using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Scripts.Interfaces
{
    public interface ICollider
    {
        public bool CollidesWith(ICollider other);
    }
}
