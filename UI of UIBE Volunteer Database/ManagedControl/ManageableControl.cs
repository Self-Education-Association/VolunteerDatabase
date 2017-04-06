using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VolunteerDatabase.Desktop.ManagedControl
{
    internal abstract class ManageableControl : UserControl, IManageableControl, IDisposable
    {
        public abstract void LogOutHandler();

        public abstract void MessageHandler(string message);

        public abstract void RefreshHandler();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ControlManager.GetInstance().LoadControl(this);
        }

        #region IDisposable Support
        public bool IsDisposed => _disposedValue;

        private bool _disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ControlManager.GetInstance().UnloadControl(this);
                    LogOutHandler();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
