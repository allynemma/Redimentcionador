using System;
using System.Drawing;
using System.Threading;

namespace Redimensionador
{
    internal class Program
    {
        static void Main ()
        {
            Console.WriteLine("Olá, bem vindo ao redimencionador!");
            Console.WriteLine("Iniciando redimencionador...");

            Thread thread = new Thread(Redimencionador);
            thread.Start();
            thread.Join();
            Console.WriteLine("Fim do redimencionador.");
        }


        static void Redimencionador()
        {

            #region Diretorios
            string dir_entrada = "Arquivos_Entrada";
            string dir_redimen = "Aquivos_Redimencionados";

            if (Directory.Exists(dir_entrada) == false) {
                Directory.CreateDirectory(dir_entrada);
            }
            if (Directory.Exists(dir_redimen) == false)
            {
                Directory.CreateDirectory(dir_redimen);
            }
            #endregion Diretorios
            while (true) {
                
                //verificar e validar se tem arquivo na pasta
                var arqsEntrada = Directory.EnumerateFiles(dir_entrada);
                var arqsRedimen = Directory.EnumerateFiles(dir_redimen);

                foreach (var arq in arqsEntrada)
                {
                    //lendo o arquivo:
                    FileStream FsEnt = new FileStream(
                            arq, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite
                        );
                    FileInfo FiEnt = new FileInfo( arq );

                    string caminhoCompletoArq = Environment.CurrentDirectory + @"\" + dir_redimen + @"\" + FiEnt.Name;

                    int tamAltura = 200; //pixels

                    foreach (var i in arqsRedimen)
                    {
                        FileStream FsRed = new FileStream(
                            i, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite
                        );
                        FileInfo FiRed = new FileInfo(i);
                        if (FiEnt.Name != FiRed.Name)
                        {
                            Redimensionar(Image.FromStream(FsEnt), tamAltura, caminhoCompletoArq);
                        }
                        else
                        {
                            Console.WriteLine(@"Arquivo " +FiRed.Name + " já foi redimensionado");
                        }
                    }
                    
                    
                    FsEnt.Close();

                    //Fi.MoveTo(dir_final);
                }
                break;
            }
        }

        static void Redimensionar (Image image, int alturaASerRedimen, string caminhoRedimen)
        {
            double ration = (double)alturaASerRedimen / image.Height;
            int novaLargura = (int)(image.Width*ration);
            int novaAltura = (int)(image.Height*ration);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImagem))
            {
                g.DrawImage(image, 0, 0, novaLargura, novaAltura);
            }
            novaImagem.Save(caminhoRedimen);
            image.Dispose();
        }
    }
}
