using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Services;

namespace Unitec.Middleware
{
    public class CreditCardReader : GenericDevice, ICredirCardReader
    {
        public event EventHandler CardDataObtained;
        public event EventHandler CardInserted;
        public event EventHandler CardInsertTimeout;
        public event EventHandler CardReadFailure;

        #region Event
        protected virtual void OnCardDataObtained(EventArgs e)
        {
            EventHandler handler = CardDataObtained;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardInserted(EventArgs e)
        {
            EventHandler handler = CardInserted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardInsertTimeout(EventArgs e)
        {
            EventHandler handler = CardInsertTimeout;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnCardReadFailure(EventArgs e)
        {
            EventHandler handler = CardReadFailure;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region implementation of Generic Device

        protected override bool CheckHealth(out int code, out string status)
        {
            throw new NotImplementedException();
        }

        protected override bool ConnectToDevice()
        {
            throw new NotImplementedException();
        }

        protected override bool DisableDevice()
        {
            throw new NotImplementedException();
        }

        protected override bool DisconnectFromDevice()
        {
            throw new NotImplementedException();
        }

        protected override bool EnableDevice()
        {
            throw new NotImplementedException();
        }

        protected override bool InitializeDevice()
        {
            throw new NotImplementedException();
        }


        protected override bool ResetHardware()
        {
            throw new NotImplementedException();
        }

        protected override bool RunDiagnosticTests(out List<string> symptomsCodes, string deviceInfo)
        {
            throw new NotImplementedException();
        }

        protected override bool TerminateDevice()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
