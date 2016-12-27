using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JenkinsNotification.Core.Services
{
    public interface IProgress
    {
        void SetMessage(string message);
    }
}