/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		UploadCompletedEventArgs.cs
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
    /// UploadCompleted Event Args
    /// </summary>
    public class UploadCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public String FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the FTP file.
        /// </summary>
        /// <value>The name of the FTP file.</value>
        public String FTPFileName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="ftpFileName">Name of the FTP file.</param>
        /// <param name="error">The error.</param>
        public UploadCompletedEventArgs(String fileName, String ftpFileName, Exception error)
            : base(error)
        {
            this.FileName = fileName;
            this.FTPFileName = fileName;
        }
    }
}