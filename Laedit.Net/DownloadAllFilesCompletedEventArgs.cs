/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		DownloadAllFilesCompletedEventArgs.cs
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

namespace Laedit.Net
{
    /// <summary>
    /// DownloadAllFilesCompleted Event Args
    /// </summary>
    public class DownloadAllFilesCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the FTP folder.
        /// </summary>
        /// <value>The FTP folder.</value>
        public String FTPFolder { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public String Folder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAllFilesCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="ftpFolder">The FTP folder.</param>
        /// <param name="folder">The folder.</param>
        public DownloadAllFilesCompletedEventArgs(String ftpFolder, String folder)
        {
            this.FTPFolder = ftpFolder;
            this.Folder = folder;
        }
    }
}