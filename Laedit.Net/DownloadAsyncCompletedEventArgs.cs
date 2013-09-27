/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		DownloadAsyncCompletedEventArgs.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	28/07/2010
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
    /// DownloadAscynCompletedEventArgs
    /// </summary>
    internal class DownloadAsyncCompletedEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public String FileName { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public String Folder { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Guid Guid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAsyncCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="guid">The GUID.</param>
        public DownloadAsyncCompletedEventArgs(String fileName, String folder, Guid guid)
        {
            this.FileName = fileName;
            this.Folder = folder;
            this.Guid = guid;
        }
    }
}