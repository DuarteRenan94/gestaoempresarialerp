using System;
using System.Linq;
using GestaoEmpresarialERP.Entities;
using Microsoft.EntityFrameworkCore;
using GestaoEmpresarialERP;
using Microsoft.UI.Xaml;
using GestaoEmpresarialERP.Ui;
using Microsoft.UI.Xaml.Controls;
using static GestaoEmpresarialERP.App;
using GestaoEmpresarialERP.Utils;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP.Ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VendaPage : Page
    {
        public int IdFormaSelecionada { get; set; }

        private Messenger msg;

        public VendaPage()
        {
            CarregarFormasPagamento();
            InitializeComponent();
        }

        private async void CarregarFormasPagamento()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var _formas = await context.FormasPagamento.ToListAsync();
                    CbFormas.ItemsSource = _formas;
                }
                catch (Exception ex)
                {
                    await msg.Error(ex.Message);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.msg = new(this.XamlRoot);
            lstPedido.ItemsSource = AppState.Views;
        }

        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = (Frame)Parent;
            frame.Navigate(typeof(Pdv));
        }

        private bool IsVendaValida()
        {
            return lstPedido.Items.Count > 0;
        }

        private bool IsFormaDePagamentoSelecionada()
        {
            return CbFormas.SelectedIndex > -1;
        }

        private void NavegarPara(Type janela)
        {
            Frame? frame = this.Parent as Frame;
            frame?.Navigate(janela);
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!IsVendaValida())
            {
                await msg.Error("Faltam itens no pedido");
                return;
            }
            if (!IsFormaDePagamentoSelecionada())
            {
                await msg.Error("A forma de pagamento não foi selecionada");
                return;
            }
            using (var context = new AppDbContext())
            { 
                if(AppState.Views == null)
                {
                    await msg.Error("Erro ao carregar itens do pedido");
                    return;
                }
                try
                {
                    double total = AppState.Views.Sum(v => v.Total);
                    var venda = new Venda
                    {
                        DtVenda = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        Total = total
                    };
                    Venda vendaSalva = context.Vendas.Add(venda).Entity;
                    await context.SaveChangesAsync();
                    foreach (var item in AppState.Views)
                    {
                        var servicoVenda = new ServicoVenda
                        {
                            ServicoId = item.Id,
                            VendaId = vendaSalva.Id,
                            Quantidade = item.Quantidade,
                            FormaPagamentoId = IdFormaSelecionada
                        };
                        context.ServicosRealizados.Add(servicoVenda);
                        await context.SaveChangesAsync();
                    }

                    Caixa movAnterior = await context.Movimentacoes.OrderBy(c => c.Id).LastOrDefaultAsync()!;
                    if(movAnterior == null)
                    {
                        await msg.Error("Erro ao carregar movimentos anteriores");
                        NavegarPara(typeof(Pdv));
                        return;
                    }
                    var novo = new Caixa
                    {
                        Movimento = total,
                        SaldoAnterior = movAnterior.SaldoAtual,
                        Descricao = $"Venda dia {vendaSalva.DtVenda}",
                        SaldoAtual = movAnterior.SaldoAtual + total,
                        DtMovimentacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    context.Movimentacoes.Add(novo);
                    await context.SaveChangesAsync();
                    ContentDialogResult result = await msg.Success("Venda registrada com sucesso");
                    if (result == ContentDialogResult.Primary)
                    {
                        NavegarPara(typeof(Pdv));
                    }
                }
                catch (Exception ex)
                {
                    await msg.Error(ex.InnerException?.Message!);
                }
            }
        }

    }
}
