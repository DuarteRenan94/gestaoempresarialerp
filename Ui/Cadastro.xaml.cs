using System;
using GestaoEmpresarialERP.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using GestaoEmpresarialERP.Ui;
using static GestaoEmpresarialERP.App;
using GestaoEmpresarialERP.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP.Ui;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Cadastro : Page
{

    private Messenger msg;
    public Cadastro()
    {
        InitializeComponent();
    }

    private void BtnVoltar_Click(object sender, RoutedEventArgs e)
    {
        Frame? frame = this.Parent as Frame;
        frame?.Navigate(typeof(Pdv));
    }

    private async void BtnCadastrar_Click(object sender, RoutedEventArgs e)
    {
        using (var context = new AppDbContext())
        {
            try
            {
                var novo = new Servico
                {
                    Nome = NomeServico.Text,
                    Preco = Double.Parse(PrecoServico.Text)
                };
                context.Servicos.Add(novo);
                await context.SaveChangesAsync();
                ContentDialogResult result = await msg.Success("Serviço cadastrado com sucesso");
                if(result == ContentDialogResult.Primary)
                {
                    Frame? frame = this.Parent as Frame;
                    frame?.Navigate(typeof(Pdv));
                }
            }
            catch (Exception ex)
            {
                await msg.Error(ex.InnerException?.Message!);
            }
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        this.msg = new(this.XamlRoot);
    }
}
