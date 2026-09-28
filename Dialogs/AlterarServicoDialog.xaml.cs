using System;
using GestaoEmpresarialERP.Entities;
using GestaoEmpresarialERP.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static GestaoEmpresarialERP.App;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlterarServicoDialog : ContentDialog
    {
        private Servico servicoEdicao;

        private readonly Messenger _messenger;
        public AlterarServicoDialog(XamlRoot root, Servico servico)
        {
            this.XamlRoot = root;
            this.servicoEdicao = servico;
            InitializeComponent();
            _messenger = new(this.XamlRoot);
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            TxNome.Text = servicoEdicao.Nome;
            TxPreco.Text = servicoEdicao.Preco.ToString();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var servicoAlterado = new Servico
                    {
                        Id = servicoEdicao.Id,
                        Nome = TxNome.Text,
                        Preco = Double.Parse(TxPreco.Text)
                    };
                    context.Servicos.Update(servicoAlterado);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    await _messenger.Error($"Falha ao alterar serviço: {ex.InnerException?.Message}");
                }

            }
        }
    }
}
