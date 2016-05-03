using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatOoad.Services
{
    //cisto da je exception drukciji od obicnog ako zatreba
    //bolje je imati sto vise specijaliziranih exceptions jer olaksavaju debbugin
    //FourSquareAuthenticationException ako se uhvati negdje u root aplikacije ce bolje uputiti programera
    //gdje je nastao problem nego samo Exception
    class FourSquareAuthenticationException : Exception
    {
        public FourSquareAuthenticationException(string message) : base(message)
        {
        }
    }
}
