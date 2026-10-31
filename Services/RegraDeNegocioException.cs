using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteSystem.Services
{
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem): base(mensagem) { }
    }
}