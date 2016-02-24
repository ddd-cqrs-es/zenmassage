using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.UserContext
{
    public interface ICustomer
    {
        
    }

    public interface ITherapist : ICustomer
    {
        
    }

    public class Customer : ICustomer
    {
    }

    public class Therapist : Customer, ITherapist
    {
        
    }
}
