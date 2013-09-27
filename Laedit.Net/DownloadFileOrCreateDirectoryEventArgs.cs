/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		DownloadFileOrCreateDirectoryEventArgs.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	27/07/2010
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
    /// EventArgs for the event DownloadFileOrCreateDirectory
    /// </summary>
    public class DownloadFileOrCreateDirectoryEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public String File { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public FTPFileType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileOrCreateDirectoryEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        public DownloadFileOrCreateDirectoryEventArgs(String file, FTPFileType type)
        {
            this.File = file;
            this.Type = type;
        } // end constructor
    }
}