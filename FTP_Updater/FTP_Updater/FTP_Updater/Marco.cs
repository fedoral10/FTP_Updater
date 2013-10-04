using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace FTP_Updater
{
    public partial class Marco : Form
    {
        ftp ftpClient;
        public Marco()
        {
            InitializeComponent();
            xml.inicializa_parametros();   
            if (xml.debug == true)
            {
                ViewConsole(xml.debug);
                tempo.Interval = xml.tiempo_debug;
            }
            Console.WriteLine("usuario:"+xml.usuario);
            Console.WriteLine("pass:" + xml.password);
            Console.WriteLine("servidor:" + xml.servidor);
            Console.WriteLine("debug:" + xml.debug);
            Console.WriteLine("intervalo thread debug: " + xml.tiempo_debug);
            Console.WriteLine("cantidad_archivos:" + xml.lista_archivos.Count);
            Console.WriteLine("cantidad_folders:" + xml.lista_folder.Count);
        //"ftp://" 
            ftpClient = new ftp(xml.servidor, xml.usuario, encrypter.Desencriptar(xml.password));
            //actualiza_archivo("archivo1.txt");
            this.Visible = false;
        }

        public void cargar_marco(object sender, EventArgs e)
        {    
            //Console.WriteLine("123456 encriptado:"+encrypter.Encriptar("123456"));
            //Console.WriteLine("123456 desencriptado:"+encrypter.Desencriptar(encrypter.Encriptar("123456")));
        }
        private void evento_thread(object sender, EventArgs e)
        {
            actualiza_lista_archivos(xml.lista_archivos);
            actualiza_lista_folder(xml.lista_folder);
        }

        private void actualiza_archivo(string archivo)
        {
            if (!File.Exists(archivo))
            {
                ftpClient.download(archivo, archivo);
                return;
            }

            DateTime archivo_pc= File.GetLastWriteTime(archivo);
            DateTime archivo_ftp = ftpClient.getFileCreatedDateTime(archivo);
            if (archivo_pc < archivo_ftp)
            {
                File.Delete(archivo);
                ftpClient.download(archivo, archivo);
            }
        }

        private void actualiza_lista_archivos(List<string> archivos)
        {
            foreach (string arch in archivos)
            {
                actualiza_archivo(arch);
            }
        }

        private void actualiza_folder(string folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);

            if (directory.Exists == false)
            {
                try
                {
                    ftpClient.ftpClient.DownloadAllFiles(folder, folder);
                    Console.WriteLine("Folder '" + folder + "' descargado exitosamente");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("En folder: '"+ folder + "' "+ex.Message);
                }
           }

            DateTime folder_pc = File.GetLastWriteTime(folder);
            DateTime folder_ftp = ftpClient.getFileCreatedDateTime(folder);

            
            //FileInfo[] files = directory.GetFiles("*.*");
            //DirectoryInfo[] directories = directory.GetDirectories();
            
            if (folder_pc < folder_ftp)
            {
                directory.Delete(true);
                try
                {
                    ftpClient.ftpClient.DownloadAllFiles(folder, folder);
                    Console.WriteLine("Folder '" + folder + "' descargado exitosamente");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("En folder: '"+ folder + "' "+ex.Message);
                }
            }

        }

        private void actualiza_lista_folder(List<string> folder)
        {
            foreach (string fold in folder)
            {
                actualiza_folder(fold);
            }
        }

        private void ViewConsole(bool valor)
        {
            if (valor)
                Win32.AllocConsole();
            else
                Win32.FreeConsole();
         }

        private void tempo_Tick(object sender, EventArgs e)
        {
            evento_thread(sender, e);
        }
    }

    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
    }
}
 
 
    
