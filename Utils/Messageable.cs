using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoEmpresarialERP.Utils
{
    public interface Messageable
    {
        Task<ContentDialogResult> Success(string message);

        Task<ContentDialogResult> Error(string message);

    }
}
