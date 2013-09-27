/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		GetFileSizeCompletedEventArgs.cs
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
    /// GetFileSizeCompleted Event Args
    /// </summary>
    public class GetFileSizeCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>The size of the file.</value>
        public Int64 FileSize { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public String File { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileSizeCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fileSice">The file sice.</param>
        /// <param name="file">The file.</param>
        /// <param name="error">The error.</param>
        public GetFileSizeCompletedEventArgs(Int64 fileSice, String file, Exception error)
            : base(error)
        {
            this.FileSize = fileSice;
            this.File = file;
        }
    }
}