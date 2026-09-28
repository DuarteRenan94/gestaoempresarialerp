using GestaoEmpresarialERP.Entities;
using GestaoEmpresarialERP.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using GestaoEmpresarialERP.Utils;
using static GestaoEmpresarialERP.App;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP.Ui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Pdv : Page, Messageable
    {
        private List<Servico>? Servicos;

        private Servico? servico;

        private Messenger _messenger;

        private ObservableCollection<ServicoView> ServicosSelecionados = [];
        public Pdv()
        {
            InitializeComponent();
        }


        private void Resultados_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarServicos();
        }

        private async void CarregarServicos()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Fetch data asynchronously to prevent UI freezing
                    Servicos = await context.Servicos.OrderBy(s => s.Nome).ToListAsync();

                    // Bind the result to the UI
                    Resultados.ItemsSource = Servicos;
                }
                catch (System.Exception ex)
                {
                    await this.Error(ex.Message);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = TxPesquisa.Text.ToLower();
            var _filtrados = Servicos?.Where(x => x.Nome.ToLower().Contains(query));

            Resultados.ItemsSource = _filtrados;
        }

        private void BtnLimpar_Click(object sender, RoutedEventArgs e)
        {
            TxPesquisa.Text = "";
            ServicosSelecionados?.Clear();
            lstSelecionados.ItemsSource = ServicosSelecionados;
        }

        private void BtnVenda_Click(object sender, RoutedEventArgs args)
        {
            Frame frame = (Frame)Parent;
            AppState.Views = ServicosSelecionados!;
            frame.Navigate(typeof(VendaPage));
        }

        private async void AbrirDialogoAlteracao()
        {
            AlterarServicoDialog asd = new(this.XamlRoot, servico);
            await asd.ShowAsync();
        }

        [Deprecated("Comportamento delegado para o botão Selecionar", DeprecationType.Deprecate, 1)]
        private async void Resultados_ItemClick(object sender, ItemClickEventArgs e)
        {
            QuantidadeDialog cd = new();
            cd.XamlRoot = this.XamlRoot;
            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var servicoSelecionado = (Servico)e.ClickedItem;
                ServicoView novo = new();
                novo.Nome = servicoSelecionado.Nome;
                novo.Id = servicoSelecionado.Id;
                novo.Preco = servicoSelecionado.Preco;
                novo.Quantidade = cd.Quantidade;
                novo.Total = novo.Preco * cd.Quantidade;
                ServicosSelecionados?.Add(novo);
                lstSelecionados.ItemsSource = ServicosSelecionados;
            }
        }

        private async void Selecionar_Click(object sender, RoutedEventArgs e)
        {
            Servico selecionado = ((((e.OriginalSource as Button)?.Parent as Grid)?.Parent as Grid)?.DataContext as Servico)!;
            QuantidadeDialog cd = new();
            cd.XamlRoot = this.XamlRoot;
            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ServicoView novo = new()
                {
                    Nome = selecionado.Nome,
                    Id = selecionado.Id,
                    Preco = selecionado.Preco,
                    Quantidade = cd.Quantidade
                };
                novo.Total = novo.Preco * cd.Quantidade;
                ServicosSelecionados?.Add(novo);
                lstSelecionados.ItemsSource = ServicosSelecionados;
            }
        }

        private void BtnNovoServico_Click(object sender, RoutedEventArgs e)
        {
            Frame? frame = this.Parent as Frame;
            frame?.Navigate(typeof(Cadastro));
        }

        private async void BtnAlterar_Click(object sender, RoutedEventArgs e)
        {
            Servico servicoEdit = ((((e.OriginalSource as Button)?.Parent as Grid)?.Parent as Grid)?.DataContext as Servico)!;
            AlterarServicoDialog asd = new(this.XamlRoot, servicoEdit);
            ContentDialogResult result = await asd.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CarregarServicos();
            }
        }

        private async void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    Servico ServicoExclusao = ((((e.OriginalSource as Button)?.Parent as Grid)?.Parent as Grid)?.DataContext as Servico)!;
                    ContentDialog cd = new();
                    cd.XamlRoot = this.XamlRoot;
                    cd.Title = "Atenção";
                    cd.Content = $"Tem certeza de excluir {ServicoExclusao.Nome}?";
                    cd.PrimaryButtonText = "Sim";
                    cd.DefaultButton = ContentDialogButton.Primary;
                    cd.SecondaryButtonText = "Não";
                    ContentDialogResult result = await cd.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        context.Servicos.Remove(ServicoExclusao);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await _messenger.Error($"Erro: {ex.InnerException?.Message}");
                }

            }
        }

        [Deprecated("Método obsoleto em favor do botão alterar", DeprecationType.Deprecate, 1)]
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            AbrirDialogoAlteracao();
        }

        private void BtnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            Frame? frame = this.Parent as Frame;
            frame?.Navigate(typeof(RelatorioPage));
        }

        private void FluxoCaixa_Click(object sender, RoutedEventArgs e)
        {
            NavegarPara(typeof(FluxoCaixa));
        }

        private void NavegarPara(Type janela, object? args)
        {
            Frame? frame = this.Parent as Frame;
            frame?.Navigate(janela, args);
        }

        private void NavegarPara(Type janela)
        {
            NavegarPara(janela, null);
        }

        public async Task<ContentDialogResult> Success(string message)
        {
            ContentDialog cd = new()
            {
                Content = message,
                Title = "Sucesso",
                XamlRoot = this.XamlRoot,
                PrimaryButtonText = "OK"
            };
            return await cd.ShowAsync();
        }

        public async Task<ContentDialogResult> Error(string message)
        {
            ContentDialog cd = new()
            {
                Content = message,
                Title = "Erro",
                XamlRoot = this.XamlRoot,
                PrimaryButtonText = "OK"
            };
            return await cd.ShowAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this._messenger = new(this.XamlRoot);
        }
    }
}
