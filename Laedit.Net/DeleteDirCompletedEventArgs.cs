/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		DeleteDirCompletedEventArgs.cs
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
    /// DeleteDirCompletedEventArgs
    /// </summary>
    public class DeleteDirCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        /// <value>The directory path.</value>
        public String DirectoryPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDirCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="error">The error.</param>
        public DeleteDirCompletedEventArgs(String directoryPath, Exception error)
            : base(error)
        {
            this.DirectoryPath = directoryPath;
        }
    }
}