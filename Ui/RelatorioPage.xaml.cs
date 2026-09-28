using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoEmpresarialERP.Utils;
using GestaoEmpresarialERP.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static GestaoEmpresarialERP.App;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP.Ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RelatorioPage : Page
    {
        private Messenger _messenger;
        public RelatorioPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame? frame = this.Parent as Frame;
            frame?.Navigate(typeof(Pdv));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _messenger = new(this.XamlRoot);
            try
            {
                using var context = new AppDbContext();
                var query = from sv in context.ServicosRealizados
                            join s in context.Servicos on sv.ServicoId equals s.Id
                            join v in context.Vendas on sv.VendaId equals v.Id
                            join f in context.FormasPagamento on sv.FormaPagamentoId equals f.Id
                            orderby v.DtVenda descending
                            select new { s.Nome, s.Preco, sv.Quantidade, Data = v.DtVenda, v.Total, FormaPagamento = f.Nome };
                List<RelatorioView> vendas = [];
                foreach (var item in query)
                {
                    vendas.Add(new RelatorioView
                    {
                        Nome = item.Nome,
                        Data = item.Data,
                        Preco = item.Preco,
                        Subtotal = item.Preco * item.Quantidade,
                        FormaPagamento = item.FormaPagamento,
                        Quantidade = item.Quantidade
                    });
                }
                TxFaturamento.Text = $"R$ {vendas.Sum(v => v.Subtotal):F2}";
                lstVendas.ItemsSource = vendas;
            }
            catch (Exception ex)
            {
                await _messenger.Error($"Erro: {ex.InnerException?.Message}");
            }

        }
    }
}
