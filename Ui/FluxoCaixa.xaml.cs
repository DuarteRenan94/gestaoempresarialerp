using System;
using System.Linq;
using GestaoEmpresarialERP.Entities;
using GestaoEmpresarialERP.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI;
using GestaoEmpresarialERP.Ui;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation.Metadata;
using static GestaoEmpresarialERP.App;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class FluxoCaixa : Page
{
    private Messenger _messenger;
    public FluxoCaixa()
    {
        InitializeComponent();
    }

    [Deprecated("Método obsoleto em favor de msg.Error(string message)", DeprecationType.Deprecate, 1)]
    private void ExibirMensagemErro(string mensagem)
    {
       //App.ExibirMensagemErro(this.XamlRoot, mensagem);
    }

    public static Brush AlterarCorMovimento(double movimento)
    {
        if (movimento == 0)
        {
            return new SolidColorBrush(Colors.Black);
        }
        if (movimento > 0)
        {
            return new SolidColorBrush(Colors.Green);
        }
        return new SolidColorBrush(Colors.Red);
    }

    private async void CarregarResultados()
    {
        using (var context = new AppDbContext())
        {
            try
            {
                var _movimentos = await context.Movimentacoes.ToListAsync();
                lstMovimentos.ItemsSource = _movimentos;
            }
            catch (Exception ex)
            {
                //ExibirMensagemErro(ex.InnerException?.Message!);
                await _messenger.Error(ex.InnerException?.Message!);
            }

        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        _messenger = new(this.XamlRoot);
        CarregarResultados();
    }

    private void Voltar_Click(object sender, RoutedEventArgs e)
    {
        Frame? frame = this.Parent as Frame;
        frame?.Navigate(typeof(Pdv));
    }

    private async void Registrar_Click(object sender, RoutedEventArgs e)
    {
        if (TxMovimento.Text == "")
        {
            await _messenger.Error("Movimentação vazia");
            return;
        }
        using (var context = new AppDbContext())
        {
            try
            {
                Caixa movAnterior = await context?.Movimentacoes.OrderBy(c => c.DtMovimentacao).Reverse().FirstAsync()!;
                double movimento = Double.Parse(TxMovimento.Text);
                var novo = new Caixa
                {
                    Movimento = movimento,
                    SaldoAnterior = movAnterior.SaldoAtual,
                    Descricao = TxDescricao.Text,
                    SaldoAtual = movAnterior.SaldoAtual + movimento,
                    DtMovimentacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };
                context.Movimentacoes.Add(novo);
                await context.SaveChangesAsync();
                ContentDialogResult result = await _messenger.Success("Movimentação registrada com sucesso");
                if (result == ContentDialogResult.Primary)
                {
                    CarregarResultados();
                }
            }
            catch (Exception ex)
            {
                await _messenger.Error(ex.Message);
            }
        }
    }
}
