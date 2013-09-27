/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		Web.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	26/07/2010
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
using System.IO;
using System.Net;

namespace Laedit.Net
{
    /// <summary>
    /// Class use to download or upload a file. you are not obliged any more to use the webclient class
    /// </summary>
    public static class Web
    {

        /// <summary>
        /// File name
        /// </summary>
        private static String FileName;

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="filePath">file to be download</param>
        /// <param name="outputFile">path of the downloaded file on the computer</param>
        public static void DownloadFile(String filePath, String outputFile)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(filePath, outputFile);
            wc.Dispose();

            Web.FileName = outputFile;
        } // end procedure DownloadFile

        
        /// <summary>
        /// Delete the last file downloaded by DownloadFile method
        /// </summary>
        public static void DeleteFile()
        {
            File.Delete(Web.FileName);
        } // end procedure DeleteFile


        /// <summary>
        /// Upload a file on a ftp
        /// </summary>
        /// <param name="username">username for the ftp connection</param>
        /// <param name="password">password for the ftp connection</param>
        /// <param name="fileToUpload">path of the uploaded file</param>
        /// <param name="fileFromUpload">path of the file to be upload</param>
        public static void UploadFile(String username, String password, String fileToUpload, String fileFromUpload)
        {
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential(username, password);
            wc.UploadFile(fileToUpload, WebRequestMethods.Ftp.UploadFile, fileFromUpload);
            wc.Dispose();
        } // end procedure UploadFile


    }
}