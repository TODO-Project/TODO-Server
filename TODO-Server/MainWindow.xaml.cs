using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TODO_Server.Console;
using TODO_Server.Server;

namespace TODO_Server
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeConsole();
            GameServer.Initialize();
        }

        private void TextBoxConsoleInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!ServerConsole.HandleCommands(TextBoxConsoleInput.Text))
                {
                    ServerConsole.Print("Command \"" + TextBoxConsoleInput.Text + "\" unknown. ", ConsoleFlags.Alert);
                }
                TextBoxConsoleInput.Text = "";
                if (ScrollViewerConsole.IsEnabled)
                {
                    ScrollViewerConsole.ScrollToBottom();
                }
            }
        }

        private void InitializeConsole()
        {
            ServerConsole.Console = TextBlockConsole;
            ServerConsole.ConsoleWindow = this;
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerConsole.Print("Stopping the server...");
            GameServer.ShouldRun = false;
            GameServer.ServerThread.Join();
        }
    }
}
