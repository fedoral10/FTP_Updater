using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace FTP_Updater
{
    static class xml
    {
        public static string usuario = "";
        public static string password = "";
        public static string servidor = "";
        public static List<string> lista_archivos = new List<string>();
        public static List<string> lista_folder = new List<string>();
        public static bool debug;
        public static int tiempo_debug;
        public static string inicializa_parametros()
        {
            XmlTextReader reader = new XmlTextReader("configuracion.xml");
            string valor = "";
            while (reader.Read())
            {
             switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        /*Console.Write("<" + reader.Name);
                        Console.WriteLine(">");*/
                        try
                        {
                            if (reader.Name.ToUpper() == "USUARIO")
                            {
                                usuario = reader.ReadElementContentAsString();
                            }
                            if (reader.Name.ToUpper() == "CONTRASENIA")
                            {
                                password = reader.ReadElementContentAsString();
                            }
                            if (reader.Name.ToUpper() == "SERVIDOR")
                            {
                                servidor = reader.ReadElementContentAsString();
                            }
                            if (reader.Name.ToUpper() == "DEBUG")
                            {
                                string val = reader.ReadElementContentAsString();
                                if (val == "S")
                                {
                                    debug = true;
                                }
                                else
                                {
                                    debug = false;
                                }

                            }
                            if (reader.Name.ToUpper() == "LISTA_ARCHIVOS")
                            {
                                string archivos = reader.ReadElementContentAsString();
                                string palabra="";
                                int c=0;
                                foreach(char x in archivos.ToCharArray())
                                {
                                    palabra += x;
                                    if (x == '\n')
                                    {
                                        
                                        if (!string.IsNullOrEmpty(palabra.Trim()))
                                        {
                                            lista_archivos.Add(palabra.Trim());
                                            palabra = "";
                                            c++;
                                        }
                                    }
                                }
                            }
                            if (reader.Name.ToUpper() == "LISTA_FOLDER")
                            {
                                string archivos = reader.ReadElementContentAsString();
                                string palabra = "";
                                int c = 0;
                                foreach (char x in archivos.ToCharArray())
                                {
                                    palabra += x;
                                    if (x == '\n')
                                    {

                                        if (!string.IsNullOrEmpty(palabra.Trim()))
                                        {
                                            lista_folder.Add(palabra.Trim());
                                            palabra = "";
                                            c++;
                                        }
                                    }
                                }
                            }
                            if (reader.Name.ToUpper() == "TIEMPO")
                            {
                                tiempo_debug = reader.ReadElementContentAsInt();
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        //Console.Write(reader.Value);
                        break;
                    /*case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;*/
                    /*case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;*/
                }
            }
            return valor;
        }
    }
}
