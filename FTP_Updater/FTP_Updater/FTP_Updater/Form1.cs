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
    public partial class Form1 : Form
    {
        ftp ftpClient;
        public Form1()
        {
            InitializeComponent();
            xml.inicializa_parametros();   
            if (xml.debug == true)
            {
                ViewConsole(xml.debug);
            }
            Console.WriteLine("usuario:"+xml.usuario);
            Console.WriteLine("pass:" + xml.password);
            Console.WriteLine("servidor:" + xml.servidor);
            Console.WriteLine("debug:" + xml.debug);
            Console.WriteLine("cantidad_archivos:" + xml.lista_archivos.Count);
            ftpClient = new ftp(@"ftp://" + xml.servidor, xml.usuario, encrypter.Desencriptar(xml.password));
            //actualiza_archivo("archivo1.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //actualiza_lista_archivos(xml.lista_archivos);
            //Console.WriteLine("123456 encriptado:"+encrypter.Encriptar("123456"));
            //Console.WriteLine("123456 desencriptado:"+encrypter.Desencriptar(encrypter.Encriptar("123456")));
        }

        private void actualiza_archivo(string archivo)
        {
            DateTime d= File.GetLastWriteTime(archivo);
            DateTime xl = ftpClient.getFileCreatedDateTime(archivo);
            if (d < xl)
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

        private void ViewConsole(bool valor)
        {
            if (valor)
                Win32.AllocConsole();
            else
                Win32.FreeConsole();
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
 
 
    
