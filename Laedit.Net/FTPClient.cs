/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		FTPClient.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	31/07/2010
 *  ----------------------------------------------------------------------------
 *  Copyright 2010 Jérémie Bertrand
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at 
 *  
 *      http://www.apache.org/licenses/LICENSE-2.0 
 *  
 *  Unless required by applicable law or agreed to in writing, software 
 *  distributed under the License is distributed on an "AS IS" BASIS, 
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 *  See the License for the specific language governing permissions and 
 *  limitations under the License.
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Laedit.Net
{
    /// <summary>
    /// Represent an FTP client
    /// </summary>
    public partial class FTPClient
    {
        #region Fields

        /// <summary>
        /// FTP Web Request
        /// </summary>
        private FtpWebRequest _ftpRequest;

        /// <summary>
        /// Files in asynchronous download Guid => ID of the download, String => Folder, List(String) => files in folder to download
        /// </summary>
        private Dictionary<Guid, Dictionary<String, List<String>>> _filesInDowload;

        #endregion Fields


        #region Properties

        /// <summary>
        /// IP or name
        /// </summary>
        public String Server { get; set; }

        /// <summary>
        /// user id for connection
        /// </summary>
        public String UserID { get; set; }

        /// <summary>
        /// password for connection
        /// </summary>
        public String Password { get; set; }

        #endregion Properties


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FTPServer">IP or name of the server</param>
        /// <param name="FTPUserID">user id for connection</param>
        /// <param name="FTPPassword">password for connection</param>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public FTPClient(String FTPServer, String FTPUserID, String FTPPassword)
        {
            if (FTPServer == null)
            {
                throw new ArgumentNullException("FTPServer");
            }
            if (FTPUserID == null)
            {
                throw new ArgumentNullException("FTPUserID");
            }
            if (FTPPassword == null)
            {
                throw new ArgumentNullException("FTPPassword");
            }

            this.Server = FTPServer;
            this.UserID = FTPUserID;
            this.Password = FTPPassword;

            this._ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.Server + "/"));

            this._filesInDowload = new Dictionary<Guid, Dictionary<String, List<String>>>();

        } // end constructor

        #endregion Constructor


        #region Private methods

        /// <summary>
        /// Determines whether [is detail file folder] [the specified detail file].
        /// </summary>
        /// <param name="detailFile">The detail file.</param>
        /// <returns>
        /// 	<c>true</c> if [is detail file folder] [the specified detail file]; otherwise, <c>false</c>.
        /// </returns>
        private Boolean IsDetailFileFolder(String[] detailFile)
        {
            Boolean flag = false;

            if (detailFile[0][0] == 'd' || detailFile[1].ToLower() == "<dir>")
            {
                flag = true;
            }

            return flag;
        } // end function IsDetailFileFolder

        /// <summary>
        /// initialize the FTPClient with File or Folder
        /// </summary>
        /// <param name="fileOrFolder">File or Folder on FTP</param>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void InitializeFTPRequest(String fileOrFolder)
        {
            if (fileOrFolder == null)
            {
                throw new ArgumentNullException("fileOrFolder");
            }

            this._ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.Server + "/" + fileOrFolder));
            this._ftpRequest.UseBinary = true;
            this._ftpRequest.Credentials = new NetworkCredential(this.UserID, this.Password);
        } // end procedure InitializeFTPRequest

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="pathOut">The path out.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void DownloadFiles(String folder, String pathOut)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            if (pathOut == null)
            {
                throw new ArgumentNullException("pathOut");
            }

            List<String[]> detailFiles = this.GetFilesDetailList(folder);

            if (folder != String.Empty)
            {
                folder += "/";
            }

            if (detailFiles != null)
            {
                for (int i = 0; i < detailFiles.Count; i++)
                {
                    int index = ((String[])detailFiles[i]).Length - 1;
                    if (((String[])detailFiles[i])[index] != "." && ((String[])detailFiles[i])[index] != "..")
                    {
                        if (!this.IsDetailFileFolder(detailFiles[i]))
                        {
                            this.OnDownloadFilesOrCreateDirectory(folder + ((String[])detailFiles[i])[index], FTPFileType.File);

                            this.Download(folder + ((String[])detailFiles[i])[index], pathOut + Path.GetFileName(((String[])detailFiles[i])[index]));
                        }
                        else
                        {
                            this.OnDownloadFilesOrCreateDirectory(folder + ((String[])detailFiles[i])[index], FTPFileType.Folder);

                            Directory.CreateDirectory(pathOut + ((String[])detailFiles[i])[index]);
                            this.DownloadFilesRecurs(folder, ((String[])detailFiles[i])[index], pathOut);
                        }
                    }
                }
            }
            else
            {
                if (folder == String.Empty)
                {
                    folder = "root";
                }

                throw new FTPException("The files of the " + folder + " folder could not be listed");
            }
        } // end procedure DownloadFiles

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="pathOut">The path out.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void DownloadFilesRecurs(String rootFolder, String folder, String pathOut)
        {
            if (rootFolder == null)
            {
                throw new ArgumentNullException("rootFolder");
            }

            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            if (pathOut == null)
            {
                throw new ArgumentNullException("pathOut");
            }

            List<String[]> detailFiles = this.GetFilesDetailList(rootFolder + folder);

            if (folder != String.Empty)
            {
                folder += "/";
            }

            if (detailFiles != null)
            {
                for (int i = 0; i < detailFiles.Count; i++)
                {
                    int index = ((String[])detailFiles[i]).Length - 1;
                    if (((String[])detailFiles[i])[index] != "." && ((String[])detailFiles[i])[index] != "..")
                    {
                        if (!this.IsDetailFileFolder(detailFiles[i]))
                        {
                            this.OnDownloadFilesOrCreateDirectory(rootFolder + folder + ((String[])detailFiles[i])[index], FTPFileType.File);

                            this.Download(rootFolder + folder + ((String[])detailFiles[i])[index], pathOut + folder + Path.GetFileName(((String[])detailFiles[i])[index]));
                        }
                        else
                        {
                            this.OnDownloadFilesOrCreateDirectory(rootFolder + folder + ((String[])detailFiles[i])[index], FTPFileType.Folder);

                            Directory.CreateDirectory(pathOut + folder + ((String[])detailFiles[i])[index]);
                            this.DownloadFilesRecurs(rootFolder, folder + ((String[])detailFiles[i])[index], pathOut);
                        }
                    }
                }
            }
            else
            {
                if (folder == String.Empty)
                {
                    folder = "root";
                }

                throw new FTPException("The files of the " + folder + " folder could not be listed");
            }
        } // end procedure DownloadFiles

        #endregion Private methods


        #region Public methods

        /// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// </summary>
        /// <param name="filename">file full name to be uploaded</param>
        /// <param name="FTPFileName">file full name on the ftp</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void Upload(String filename, String FTPFileName)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            if (FTPFileName == null)
            {
                throw new ArgumentNullException("FTPFileName");
            }

            FileInfo fileInf = new FileInfo(filename);
            //string uri = "ftp://" + this.Server + "/" + fileInf.Name;

            this.InitializeFTPRequest(FTPFileName);//fileInf.Name

            // By default KeepAlive is true, where the control connection is not closed
            // after a command is executed.
            this._ftpRequest.KeepAlive = false;

            // Specify the command to be executed.
            this._ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Notify the server about the size of the uploaded file
            this._ftpRequest.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb
            Int32 bufferLength = 2048;
            Byte[] buffer = new byte[bufferLength];
            Int32 contentLength;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            using (FileStream fs = fileInf.OpenRead())
            {
                try
                {
                    // Stream to which the file to be upload is written
                    using (Stream strm = this._ftpRequest.GetRequestStream())
                    {
                        // Read from the file stream 2kb at a time
                        contentLength = fs.Read(buffer, 0, bufferLength);

                        // Till Stream content ends
                        while (contentLength != 0)
                        {
                            // Write Content from the file stream to the FTP Upload Stream
                            strm.Write(buffer, 0, contentLength);
                            contentLength = fs.Read(buffer, 0, bufferLength);
                        }

                        // Close the file stream and the Request Stream
                        strm.Close();
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new FTPException("Upload Error", ex);
                }
            }
        } // end procedure Upload
        
        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="fileName">file will be deleted</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void Delete(String fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            try
            {
                //string uri = "ftp://" + this.Server + "/" + fileName;
                this.InitializeFTPRequest(fileName);

                this._ftpRequest.KeepAlive = false;
                this._ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                String result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();

                Int64 size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                        datastream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("Delete Error", ex);
            }
        } // end function Delete
        /// <summary>
        /// Method to delete a directory
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DeleteDir(String directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException("directoryPath");
            }

            try
            {
                this.InitializeFTPRequest(directoryPath);

                this._ftpRequest.KeepAlive = false;
                this._ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                
                String result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();

                Int64 size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                        datastream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("Delete Error", ex);
            }
        } // end function DeleteDir

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <returns>null if exception, else ArrayList of file description</returns>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        public List<String[]> GetFilesDetailList()
        {
            return this.GetFilesDetailList(String.Empty);
        } // end function GetFilesDetailList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="folder">folder on the ftp. String.empty for the root</param>
        /// <returns>null if exception, else ArrayList of file description</returns>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        public List<String[]> GetFilesDetailList(String folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            List<String[]> downloadFiles = new List<String[]>();
            try
            {
                this.InitializeFTPRequest(folder);
                //UseBinary false
                this._ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = this._ftpRequest.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    String line = String.Empty;

                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] entry = line.Split(new String[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

                        String[] trueEntry = new String[entry.Length];

                        for (int i = 0; i < entry.Length; i++)
                        {
                            trueEntry[i] = entry[i].Trim();
                        }

                        String[] temp = entry[entry.Length - 1].Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        trueEntry[trueEntry.Length - 1] = temp[temp.Length - 1];
                        trueEntry[trueEntry.Length - 2] = trueEntry[trueEntry.Length - 2].Replace(" " + trueEntry[trueEntry.Length - 1], "");

                        downloadFiles.Add(trueEntry);
                    }

                    reader.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                throw new FTPException(ex.Message);
            }

            return downloadFiles;
        } // end function GetFilesDetailList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <returns>The names of the files</returns>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public String[] GetFilesList()
        {
            return this.GetFilesList(String.Empty);
        } // end function GetFilesList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>The names of the files</returns>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public String[] GetFilesList(String folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            String[] downloadFiles;
            StringBuilder result = new StringBuilder();

            try
            {
                this.InitializeFTPRequest(folder);

                this._ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = this._ftpRequest.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = reader.ReadLine();
                    }
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    reader.Close();
                    response.Close();
                }

                return result.ToString().Split('\n');
            }
            catch (Exception)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        } // end functionGetFilesList

        /// <summary>
        /// Download a file to the local drive
        /// </summary>
        /// <param name="FTPFileName">Path of the file on FTP</param>
        /// <param name="fileName">The full path where the file is to be created</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void Download(String FTPFileName, String fileName)
        {
            if (FTPFileName == null)
            {
                throw new ArgumentNullException("FTPFileName");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            try
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    this.InitializeFTPRequest(FTPFileName);

                    this._ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    FtpWebResponse response = (FtpWebResponse)this._ftpRequest.GetResponse();

                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        Int32 bufferSize = 2048;
                        Int32 readCount;
                        Byte[] buffer = new Byte[bufferSize];

                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            outputStream.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                        }

                        ftpStream.Close();
                        outputStream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("Download Error", ex);
            }
        } // end procedure Download

        /// <summary>
        /// Download a file content in a String
        /// </summary>
        /// <param name="FTPFileName">Path of the file on FTP</param>
        /// <returns>File content in a string</returns>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public String DownloadAsString(String FTPFileName)
        {
            if (FTPFileName == null)
            {
                throw new ArgumentNullException("FTPFileName");
            }

            String file = null;

            try
            {

                this.InitializeFTPRequest(FTPFileName);

                this._ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)this._ftpRequest.GetResponse();

                using (Stream ftpStream = response.GetResponseStream())
                {
                    using (StreamReader outputStream = new StreamReader(ftpStream, Encoding.Default))
                    {
                        file = outputStream.ReadToEnd();

                        ftpStream.Close();
                        outputStream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("Download Error", ex);
            }

            return file;
        } // end procedure DownloadAsString

        /// <summary>
        /// Method to get the size of the file
        /// </summary>
        /// <param name="filename">the file</param>
        /// <returns>the size</returns>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public Int64 GetFileSize(String filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            Int64 fileSize = 0;
            try
            {
                this.InitializeFTPRequest(filename);
                this._ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)this._ftpRequest.GetResponse();

                using (Stream ftpStream = response.GetResponseStream())
                {
                    fileSize = response.ContentLength;

                    ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("File Size Error", ex);
            }
            return fileSize;
        } // end function GetFileSize

        /// <summary>
        /// Method to rename a file
        /// </summary>
        /// <param name="currentFilename">file will be renamed</param>
        /// <param name="newFilename">new filename</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void Rename(String currentFilename, String newFilename)
        {
            if (currentFilename == null)
            {
                throw new ArgumentNullException("currentFilename");
            }

            if (newFilename == null)
            {
                throw new ArgumentNullException("newFilename");
            }

            try
            {
                this.InitializeFTPRequest(currentFilename);
                this._ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                this._ftpRequest.RenameTo = newFilename;

                FtpWebResponse response = (FtpWebResponse)this._ftpRequest.GetResponse();
                using (Stream ftpStream = response.GetResponseStream())
                {
                    ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("Rename Error", ex);
            }
        } // end procedure Rename

        /// <summary>
        /// Method to create a directory
        /// </summary>
        /// <param name="dirName">name of the new directory</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void MakeDir(String dirName)
        {
            if (dirName == null)
            {
                throw new ArgumentNullException("dirName");
            }

            try
            {
                this.InitializeFTPRequest(dirName);
                this._ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                
                FtpWebResponse response = (FtpWebResponse)this._ftpRequest.GetResponse();
                using (Stream ftpStream = response.GetResponseStream())
                {
                    // ftpStream == String.Empty if it's ok
                    ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new FTPException("MakeDir Error", ex);
            }
        } // end procedure MakeDir

        /// <summary>
        /// Download all files and folders of the indicated folder
        /// </summary>
        /// <param name="pathIn">the folder on the FTP server</param>
        /// <param name="pathOut">the out folder, on your computer</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DownloadAllFiles(String pathIn, String pathOut)
        {
            if (pathIn == null)
            {
                throw new ArgumentNullException("pathIn");
            }

            if (pathOut == null)
            {
                throw new ArgumentNullException("pathOut");
            }

            if (!Directory.Exists(pathOut))
            {
                Directory.CreateDirectory(pathOut);
            }

            if (!pathOut.EndsWith("/") && !pathOut.EndsWith(@"\"))
            {
                pathOut += "/";
            }

            this.DownloadFiles(pathIn, pathOut);
        } // end procedure DownloadAllFiles

        #endregion Public methods

    }

}