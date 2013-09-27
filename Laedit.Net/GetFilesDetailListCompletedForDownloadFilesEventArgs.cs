/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		GetFilesDetailListCompletedForDownloadFilesEventArgs.cs
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
using System.Collections.Generic;

namespace Laedit.Net
{
    /// <summary>
    /// GetFilesDetailListAsyncCompletedEventArgs
    /// </summary>
    internal class GetFilesDetailListCompletedForDownloadFilesEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        /// <value>The root folder.</value>
        public String RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public String Folder { get; set; }

        /// <summary>
        /// Gets or sets the files detail.
        /// </summary>
        /// <value>The files detail.</value>
        public List<String[]> FilesDetail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesDetailListCompletedForDownloadFilesEventArgs"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="filesDetail">The files detail.</param>
        public GetFilesDetailListCompletedForDownloadFilesEventArgs(Guid guid, String rootFolder, String folder, List<String[]> filesDetail)
        {
            this.Guid = guid;
            this.RootFolder = rootFolder;
            this.Folder = folder;
            this.FilesDetail = filesDetail;
        } // end constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesDetailListCompletedForDownloadFilesEventArgs"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="filesDetail">The files detail.</param>
        public GetFilesDetailListCompletedForDownloadFilesEventArgs(Guid guid, String folder, List<String[]> filesDetail)
        {
            this.Guid = guid;
            this.Folder = folder;
            this.FilesDetail = filesDetail;
        } // end constructor
    }
}