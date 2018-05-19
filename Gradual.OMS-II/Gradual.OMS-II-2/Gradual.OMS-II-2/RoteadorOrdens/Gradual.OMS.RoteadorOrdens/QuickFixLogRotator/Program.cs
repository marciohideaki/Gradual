using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QuickFixLogRotator
{
    public class Program
    {
        public string dirOrigem { get; set; }
        public string dirDestino { get; set; }
        public bool verbose { get; set; }
        public bool testMode { get; set; }

        public static void Main(string[] args)
        {
            int i = 0;

            Program program = new Program();

            if (args.Length < 4)
            {
                Console.WriteLine("Usage: QuickFixLogRotator -s <source path> -d <destination path> [-v] where:");
                Console.WriteLine("-s <source path>: directory of the files to be rotated (quickfix FileLogPath)");
                Console.WriteLine("-d <destination path>: directory for file backup");
                Console.WriteLine("-v: optional, toogles verbose mode");
                Console.WriteLine("-t: optional, test mode enabled (no files will be moved)");
                Environment.Exit(0);
            }

            while (i < args.Length)
            {
                switch(args[i])
                {
                    case "-s":
                        program.dirOrigem = args[i + 1];
                        i++;
                        break;
                    case "-d":
                        program.dirDestino = args[i + 1];
                        i ++;
                        break;
                    case "-v":
                        program.verbose = true;
                        break;
                    case "-t":
                        program.testMode = true;
                        break;
                }
                i++;
            }

            program.doBackup();
        }

        private List<string> obterListaArquivos()
        {
            List<string> retorno = new List<string>();

            if ( verbose )
                Console.WriteLine("Getting file list of: " + dirOrigem);

            try
            {
                if (Directory.Exists(this.dirOrigem))
                {
                    string[] arquivos = Directory.GetFiles(this.dirOrigem, "*.messages.current.log", SearchOption.TopDirectoryOnly);

                    if (arquivos != null && arquivos.Length > 0)
                    {
                        retorno.AddRange(arquivos);
                    }

                    if (verbose)
                        Console.WriteLine(retorno.Count + " files found");
                }
                else
                {
                    Console.WriteLine("Directory [" + this.dirOrigem + "] is invalid or does not contains *.messages.log files");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }


            return retorno;
        }

        private void doBackup()
        {
            try
            {
                List<string> arquivosOrigem = this.obterListaArquivos();

                string diretorioDestino = String.Format("{0}\\{1}", this.dirDestino, DateTime.Now.ToString("yyyy-MM-dd"));

                if (verbose)
                    Console.WriteLine("Creating directory: " + diretorioDestino);

                if ( !testMode )
                    Directory.CreateDirectory(diretorioDestino);

                if (testMode || Directory.Exists(diretorioDestino))
                {
                    foreach(string arquivo in arquivosOrigem)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(arquivo);

                            string destFile = String.Format("{0}\\{1}", diretorioDestino, fileName);

                            if (verbose)
                                Console.WriteLine("Moving: " + arquivo);

                            if ( !testMode )
                                Directory.Move(arquivo, destFile);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error when moving [" + arquivo + "]: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
