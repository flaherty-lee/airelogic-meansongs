using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanSongs.Exceptions
{
    public class RetriesCountException : Exception
    {
        public RetriesCountException(string message) : base(message)
        {
        }
    }
}
