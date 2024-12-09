using System;
using System.Diagnostics;
using System.IO;

namespace InventarioComputador
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Escolha uma opção de inventário:");
                Console.WriteLine("1. Hardware");
                Console.WriteLine("2. Software");
                Console.WriteLine("3. Hardware e Software");
                Console.WriteLine("4. Sair");
                Console.Write("Digite o número da opção desejada: ");
                
                string opcao = Console.ReadLine();
                
                switch (opcao)
                {
                    case "1":
                        SalvarInventario("Hardware");
                        break;
                    case "2":
                        SalvarInventario("Software");
                        break;
                    case "3":
                        SalvarInventario("HardwareSoftware");
                        break;
                    case "4":
                        Console.WriteLine("Saindo...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
                
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
        }

        static void SalvarInventario(string tipo)
        {
            string arquivo = "InventarioComputador.dat";
            string conteudoInventario = "";

            if (tipo == "Hardware" || tipo == "HardwareSoftware")
            {
                conteudoInventario += "=== Inventário de Hardware ===\n";
                conteudoInventario += ExecutarComandoPowerShell("Get-ComputerInfo");
                conteudoInventario += "\n";
            }

            if (tipo == "Software" || tipo == "HardwareSoftware")
            {
                conteudoInventario += "=== Inventário de Software ===\n";
                conteudoInventario += ExecutarComandoPowerShell("Get-WmiObject -Class Win32_Product | Select-Object -Property Name,Version");
                conteudoInventario += "\n";
            }

            File.WriteAllText(arquivo, conteudoInventario);
            Console.WriteLine($"Inventário salvo em: {arquivo}");
        }

        static string ExecutarComandoPowerShell(string comando)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-Command \"{comando}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
