using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitec.Middleware.Contracts
{
    public interface ICredirCardReader
    {
        event EventHandler CardDataObtained;
        event EventHandler CardInserted;
        event EventHandler CardInsertTimeout;
        event EventHandler CardReadFailure;
    }
}
