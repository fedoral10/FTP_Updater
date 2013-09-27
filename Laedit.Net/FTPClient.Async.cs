/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		FTPClient.Async.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	05/09/2010
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
        #region Private methods

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="pathOut">The path out.</param>
        /// <param name="guid">The GUID.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void DownloadFilesAsync(String folder, String pathOut, Guid guid)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            if (pathOut == null)
            {
                throw new ArgumentNullException("pathOut");
            }

            if (guid == null)
            {
                throw new ArgumentNullException("guid");
            }

            this._filesInDowload.Add(guid, new Dictionary<String, List<String>>());

            this.DownloadAsyncCompleted += new EventHandler<DownloadAsyncCompletedEventArgs>(new Action<Object, DownloadAsyncCompletedEventArgs>((s, dace) =>
            {
                this._filesInDowload[dace.Guid][dace.Folder].Remove(dace.FileName);

                if (this._filesInDowload[dace.Guid][dace.Folder].Count == 0)
                {
                    this._filesInDowload[dace.Guid].Remove(dace.Folder);

                    if (this._filesInDowload[dace.Guid].Count == 0)
                    {
                        this.OnDownloadAllFilesCompleted(folder, pathOut);
                    }
                }
            }));

            if (GetFilesDetailListAsyncCompletedForDownloadFilesAsync == null)
            {
                this.GetFilesDetailListAsyncCompletedForDownloadFilesAsync += new EventHandler<GetFilesDetailListCompletedForDownloadFilesEventArgs>(new Action<Object, GetFilesDetailListCompletedForDownloadFilesEventArgs>((sender, e) =>
                {
                    if (folder != String.Empty && (!folder.EndsWith("/") && !folder.EndsWith(@"\")))
                    {
                        folder += "/";
                    }

                    this._filesInDowload[guid].Add(folder, new List<String>());

                    List<String[]> detailFiles = e.FilesDetail;
                    if (detailFiles.Count == 0)
                    {
                        this._filesInDowload[e.Guid].Remove(e.Folder);

                        if (this._filesInDowload[e.Guid].Count == 0)
                        {
                            this.OnDownloadAllFilesCompleted(folder, pathOut);
                        }
                    }

                    foreach (String[] fileDetails in detailFiles)
                    {
                        if (this.IsDetailFileFolder(fileDetails))
                        {
                            if (fileDetails[fileDetails.Length - 1] != String.Empty && (!fileDetails[fileDetails.Length - 1].EndsWith("/") && !fileDetails[fileDetails.Length - 1].EndsWith(@"\")))
                            {
                                fileDetails[fileDetails.Length - 1] += "/";
                            }

                            this._filesInDowload[guid].Add(folder + fileDetails[fileDetails.Length - 1], new List<String>());
                        }

                        this._filesInDowload[guid][folder].Add(folder + fileDetails[fileDetails.Length - 1]);

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

                                    this.DownloadAsync(folder + ((String[])detailFiles[i])[index], pathOut + Path.GetFileName(((String[])detailFiles[i])[index]), folder, guid);
                                }
                                else
                                {
                                    this.OnDownloadFilesOrCreateDirectory(folder + ((String[])detailFiles[i])[index], FTPFileType.Folder);

                                    Directory.CreateDirectory(pathOut + ((String[])detailFiles[i])[index]);
                                    this._filesInDowload[guid][folder].Remove(folder + ((String[])detailFiles[i])[index]);
                                    this.DownloadFilesAsyncRecurs(folder, ((String[])detailFiles[i])[index], pathOut, guid);
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
                }));
            }
            this.GetFilesDetailListAsync(folder, guid);
        } // end procedure DownloadFiles

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="pathOut">The path out.</param>
        /// <param name="guid">The GUID.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void DownloadFilesAsyncRecurs(String rootFolder, String folder, String pathOut, Guid guid)
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

            if (guid == null)
            {
                throw new ArgumentNullException("guid");
            }

            if (this.GetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs == null)
            {
                this.GetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs += new EventHandler<GetFilesDetailListCompletedForDownloadFilesEventArgs>(new Action<Object, GetFilesDetailListCompletedForDownloadFilesEventArgs>((sender, e) =>
                {
                    List<String[]> detailFiles = e.FilesDetail;
                    if (detailFiles.Count == 0)
                    {
                        this._filesInDowload[e.Guid].Remove(e.RootFolder + e.Folder);

                        if (this._filesInDowload[e.Guid].Count == 0)
                        {
                            this.OnDownloadAllFilesCompleted(e.RootFolder, pathOut);
                        }
                    }

                    if (folder != String.Empty && (!folder.EndsWith("/") && !folder.EndsWith(@"\")))
                    {
                        folder += "/";
                    }

                    // Add files in download
                    foreach (String[] fileDetails in detailFiles)
                    {
                        if (this.IsDetailFileFolder(fileDetails))
                        {
                            if (fileDetails[fileDetails.Length - 1] != String.Empty && (!fileDetails[fileDetails.Length - 1].EndsWith("/") && !fileDetails[fileDetails.Length - 1].EndsWith(@"\")))
                            {
                                fileDetails[fileDetails.Length - 1] += "/";
                            }

                            this._filesInDowload[guid].Add(rootFolder + folder + fileDetails[fileDetails.Length - 1], new List<String>());
                        }

                        this._filesInDowload[guid][rootFolder + folder].Add(rootFolder + folder + fileDetails[fileDetails.Length - 1]);

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

                                    this.DownloadAsync(rootFolder + folder + ((String[])detailFiles[i])[index], pathOut + folder + Path.GetFileName(((String[])detailFiles[i])[index]), rootFolder + folder, guid);
                                }
                                else
                                {
                                    this.OnDownloadFilesOrCreateDirectory(rootFolder + folder + ((String[])detailFiles[i])[index], FTPFileType.Folder);
                                    // Remove folder from filesInDownload
                                    Directory.CreateDirectory(pathOut + folder + ((String[])detailFiles[i])[index]);
                                    this._filesInDowload[guid][rootFolder + folder].Remove(rootFolder + folder + ((String[])detailFiles[i])[index]);
                                    this.DownloadFilesAsyncRecurs(rootFolder, folder + ((String[])detailFiles[i])[index], pathOut, guid);
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
                }));
            }
            this.GetFilesDetailListAsync(rootFolder, folder, guid);
        } // end procedure DownloadFilesAsyncRecurs

        /// <summary>
        /// Download a file to the local drive asynchronously
        /// </summary>
        /// <param name="FTPFileName">Path of the file on FTP</param>
        /// <param name="fileName">The full path where the file is to be created</param>
        /// <param name="folder">The folder.</param>
        /// <param name="guid">The GUID.</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        private void DownloadAsync(String FTPFileName, String fileName, String folder, Guid guid)
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
                this.InitializeFTPRequest(FTPFileName);

                this._ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    try
                    {
                        WebRequest request = (WebRequest)s.AsyncState;

                        FtpWebResponse response = null;
                        response = (FtpWebResponse)request.EndGetResponse(s);

                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            Int32 bufferSize = 2048;
                            Int32 readCount;
                            Byte[] buffer = new Byte[bufferSize];

                            readCount = ftpStream.Read(buffer, 0, bufferSize);

                            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                            {
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

                        this.OnDownloadAsyncCompleted(FTPFileName, folder, guid);
                    }
                    catch (Exception ex)
                    {
                        throw new FTPException("Download Error", ex);
                    }

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("Download Error", ex);
            }
        } // end procedure DownloadAsync

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="folder">folder on the ftp. String.Empty for the root</param>
        /// <param name="guid">The GUID.</param>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        private void GetFilesDetailListAsync(String folder, Guid guid)
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

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    response = (FtpWebResponse)request.EndGetResponse(s);

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

                    this.OnGetFilesDetailListAsyncCompletedForDownloadFilesAsync(guid, folder, downloadFiles);

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                downloadFiles = null;
                throw new FTPException(ex.Message);
            }

        } // end function GetFilesDetailList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="folder">folder on the ftp. String.Empty for the root</param>
        /// <param name="guid">The GUID.</param>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        private void GetFilesDetailListAsync(String rootFolder, String folder, Guid guid)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            List<String[]> downloadFiles = new List<String[]>();
            try
            {
                this.InitializeFTPRequest(rootFolder + folder);
                //UseBinary false
                this._ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    response = (FtpWebResponse)request.EndGetResponse(s);

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

                    this.OnGetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs(guid, rootFolder, folder, downloadFiles);

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                downloadFiles = null;
                throw new FTPException(ex.Message);
            }

        } // end function GetFilesDetailList

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// </summary>
        /// <param name="filename">file full name to be uploaded</param>
        /// <param name="FTPFileName">file full name on the ftp</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void UploadAsync(String filename, String FTPFileName)
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

            try
            {
                // Stream to which the file to be upload is written
                IAsyncResult asyncResult = this._ftpRequest.BeginGetRequestStream(new AsyncCallback(s =>
                {
                    FtpWebRequest request = (FtpWebRequest)s.AsyncState;
                    Exception error = null;

                    try
                    {
                        using (Stream strm = request.EndGetRequestStream(s))
                        {
                            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                            using (FileStream fs = fileInf.OpenRead())
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
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }

                    this.OnUploadCompleted(filename, FTPFileName, error);

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("Upload Error", ex);
            }
        } // end procedure UploadAsync
        
        /// <summary>
        /// Method to delete a file
        /// </summary>
        /// <param name="fileName">file will be deleted</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DeleteAsync(String fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            try
            {
                this.InitializeFTPRequest(fileName);

                this._ftpRequest.KeepAlive = false;
                this._ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                IAsyncResult asyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;

                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);

                        String result = String.Empty;

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
                        error = ex;
                    }

                    this.OnDeleteCompleted(fileName, error);
                }), this._ftpRequest);
            }
            catch (Exception ex)
            {
                throw new FTPException("Delete Error", ex);
            }
        } // end function Delete
        /// <summary>
        /// Method to delete a directory
        /// </summary>
        /// <param name="directoryPath">directory will be deleted</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DeleteDirAsync(String directoryPath)
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
                
                IAsyncResult asyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;
                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);

                        String result = String.Empty;

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
                        error = ex;
                    }

                    this.OnDeleteDirCompleted(directoryPath, error);
                }), this._ftpRequest);
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
        public void GetFilesDetailListAsync()
        {
            this.GetFilesDetailListAsync(String.Empty);
        } // end function GetFilesDetailList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="folder">folder on the ftp. String.empty for the root</param>
        /// <returns>null if exception, else ArrayList of file description</returns>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        public void GetFilesDetailListAsync(String folder)
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

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;

                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);

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
                        error = ex;
                    }

                    this.OnGetFilesDetailListCompleted(downloadFiles, folder, error);

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                downloadFiles = null;
                throw new FTPException(ex.Message);
            }

        } // end function GetFilesDetailList

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void GetFilesListAsync()
        {
            this.GetFilesListAsync(String.Empty);
        } // end procedure GetFilesListAsync

        /// <summary>
        /// If exception return null
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void GetFilesListAsync(String folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            StringBuilder result = new StringBuilder();

            try
            {
                this.InitializeFTPRequest(folder);

                this._ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;

                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);


                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            String line = reader.ReadLine();
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
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }

                    this.OnGetFilesListCompleted(result.ToString().Split('\n'), folder, error);
                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException(ex.Message);
            }
        } // end functionGetFilesListAsync

        /// <summary>
        /// Download a file to the local drive asynchronously
        /// </summary>
        /// <param name="FTPFileName">Path of the file on FTP</param>
        /// <param name="fileName">The full path where the file is to be created</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DownloadAsync(String FTPFileName, String fileName)
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
                this.InitializeFTPRequest(FTPFileName);

                this._ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    try
                    {
                        WebRequest request = (WebRequest)s.AsyncState;

                        FtpWebResponse response = null;
                        Exception error = null;

                        try
                        {
                            response = (FtpWebResponse)request.EndGetResponse(s);

                            using (Stream ftpStream = response.GetResponseStream())
                            {
                                Int32 bufferSize = 2048;
                                Int32 readCount;
                                Byte[] buffer = new Byte[bufferSize];

                                readCount = ftpStream.Read(buffer, 0, bufferSize);

                                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                                {
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
                            error = ex;
                        }

                        this.OnDownloadCompleted(FTPFileName, fileName, error);
                    }
                    catch (Exception ex)
                    {
                        throw new FTPException("Download error", ex);
                    }

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("Download Error", ex);
            }
        } // end procedure DownloadAsync

        /// <summary>
        /// Download a file content in a String asynchronously
        /// </summary>
        /// <param name="FTPFileName">Path of the file on FTP</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DownloadAsStringAsync(String FTPFileName)
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

                IAsyncResult result = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;
                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);

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
                        error = ex;
                    }

                    this.OnDownloadAsStringCompleted(file, error);

                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("Download Error", ex);
            }

        } // end procedure DownloadAsStringAsync

        /// <summary>
        /// Method to get the size of the file
        /// </summary>
        /// <param name="filename">the file</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void GetFileSizeAsync(String filename)
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

                IAsyncResult aSyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    WebRequest request = (WebRequest)s.AsyncState;

                    FtpWebResponse response = null;
                    Exception error = null;

                    try
                    {
                        response = (FtpWebResponse)request.EndGetResponse(s);

                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            fileSize = response.ContentLength;

                            ftpStream.Close();
                            response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }

                    this.OnGetFileSizeCompleted(fileSize, filename, error);
                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("File Size Error", ex);
            }
        } // end function GetFileSizeAsync

        /// <summary>
        /// Method to rename a file
        /// </summary>
        /// <param name="currentFilename">file will be renamed</param>
        /// <param name="newFilename">new filename</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void RenameAsync(String currentFilename, String newFilename)
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

                IAsyncResult asyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    FtpWebRequest request = (FtpWebRequest)s.AsyncState;
                    Exception error = null;

                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.EndGetResponse(s);
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            ftpStream.Close();
                            response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }

                    this.OnRenameCompleted(currentFilename, newFilename, error);
                }), this._ftpRequest);

            }
            catch (Exception ex)
            {
                throw new FTPException("Rename Error", ex);
            }
        } // end procedure RenameAsync

        /// <summary>
        /// Method to create a directory Asynchronously
        /// </summary>
        /// <param name="dirName">name of the new directory</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void MakeDirAsync(String dirName)
        {
            if (dirName == null)
            {
                throw new ArgumentNullException("dirName");
            }

            try
            {
                this.InitializeFTPRequest(dirName);
                this._ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                IAsyncResult asyncResult = this._ftpRequest.BeginGetResponse(new AsyncCallback(s =>
                {
                    FtpWebRequest request = (FtpWebRequest)s.AsyncState;

                    Boolean isCreated = false;
                    Exception error = null;

                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.EndGetResponse(s);

                        String result = String.Empty;
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(ftpStream))
                            {
                                result = reader.ReadToEnd();

                                reader.Close();
                                ftpStream.Close();
                                response.Close();
                            }
                        }

                        if (result == String.Empty)
                        {
                            isCreated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        isCreated = false;
                        error = ex;
                    }

                    this.OnMakeDirCompleted(isCreated, dirName, error);

                }), this._ftpRequest);
            }
            catch (Exception ex)
            {
                throw new FTPException("MakeDir Error", ex);
            }
        } // end procedure MakeDirAsync

        /// <summary>
        /// Download all files and folders of the indicated folder
        /// </summary>
        /// <param name="pathIn">the folder on the FTP server</param>
        /// <param name="pathOut">the out folder, on your computer</param>
        /// <exception cref="FTPException">The communication with the FTP server was wrong</exception>
        /// <exception cref="ArgumentNullException">A parameter is null</exception>
        public void DownloadAllFilesAsync(String pathIn, String pathOut)
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

            this.DownloadFilesAsync(pathIn, pathOut, Guid.NewGuid());
        } // end procedure DownloadAllFilesAsync

        #endregion Public methods
    }
}