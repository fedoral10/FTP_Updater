/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		GetFilesListCompletedEventArgs.cs
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
    /// GetFilesListCompletedEventArgs
    /// </summary>
    public class GetFilesListCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the files list.
        /// </summary>
        /// <value>The files list.</value>
        public String[] FilesList { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public String Folder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesListCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="filesList">The files list.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="error">The error.</param>
        public GetFilesListCompletedEventArgs(String[] filesList, String folder, Exception error)
            : base(error)
        {
            this.FilesList = filesList;
            this.Folder = folder;
        }
    }
}