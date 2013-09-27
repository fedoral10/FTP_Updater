/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		GetFilesDetailListCompletedEventArgs.cs
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

namespace Laedit.Net
{
    /// <summary>
    /// GetFilesDetailListCompleted Event Args
    /// </summary>
    public class GetFilesDetailListCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the files detail list.
        /// </summary>
        /// <value>The files detail list.</value>
        public List<String[]> FilesDetailList { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public String Folder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesDetailListCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="filesDetailList">The files detail list.</param>
        /// <param name="error">The error.</param>
        public GetFilesDetailListCompletedEventArgs(List<String[]> filesDetailList, String folder, Exception error)
            : base(error)
        {
            this.FilesDetailList = filesDetailList;
            this.Folder = folder;
        }
    }
}