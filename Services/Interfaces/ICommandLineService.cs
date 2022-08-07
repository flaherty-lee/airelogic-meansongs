using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanSongs.Services.Interfaces
{
    public interface ICommandLineService
    {
        public void WriteLine(string message);
        public void Write(string message);
        public string? ReadLine();
    }
}
