using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoEmpresarialERP.Utils
{
    public class Messenger : Messageable
    {
        private XamlRoot Root;
        public Messenger(XamlRoot root)
        {
            this.Root = root;
        }
        public async Task<ContentDialogResult> Error(string message)
        {
            ContentDialog cd = new()
            {
                Content = message,
                Title = "Erro",
                XamlRoot = this.Root,
                PrimaryButtonText = "OK"
            };
            return await cd.ShowAsync();
        }

        public async Task<ContentDialogResult> Success(string message)
        {
            ContentDialog cd = new()
            {
                Content = message,
                Title = "Sucesso",
                XamlRoot = this.Root,
                PrimaryButtonText = "OK"
            };
            return await cd.ShowAsync();
        }
    }
}
