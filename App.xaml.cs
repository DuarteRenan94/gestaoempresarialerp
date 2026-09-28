using GestaoEmpresarialERP.Entities;
using GestaoEmpresarialERP.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GestaoEmpresarialERP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }

        /// <summary>
        /// Classe que conecta a aplicação com o banco de dados
        /// </summary>
        /// 
        public class AppDbContext : DbContext
        {
            public DbSet<Servico> Servicos { get; set; }

            public DbSet<Venda> Vendas { get; set; }

            public DbSet<ServicoVenda> ServicosRealizados { get; set; }

            public DbSet<FormaPagamento> FormasPagamento { get; set; }

            public DbSet<Caixa> Movimentacoes { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Replace with your real PostgreSQL connection details
                string connectionString = Environment.GetEnvironmentVariable("GESTAOEMPRESARIALERP_DB_CONNECTION", EnvironmentVariableTarget.Machine)!;
                //string connectionString = Environment.GetEnvironmentVariable("GESTAOEMPRESARIALERP_DB_CONNECTION_DESENV", EnvironmentVariableTarget.Machine)!;

                optionsBuilder.UseNpgsql(connectionString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Define the composite primary key
                modelBuilder.Entity<ServicoVenda>()
                    .HasKey(sv => new { sv.ServicoId, sv.VendaId});

                // Link Book side
                modelBuilder.Entity<ServicoVenda>()
                    .HasOne(sv => sv.Servico)
                    .WithMany(b => b.Vendas)
                    .HasForeignKey(bc => bc.ServicoId);

                // Link Category side
                modelBuilder.Entity<ServicoVenda>()
                    .HasOne(bc => bc.Venda)
                    .WithMany(c => c.Vendas)
                    .HasForeignKey(bc => bc.VendaId);
            }
        }
    }
}
