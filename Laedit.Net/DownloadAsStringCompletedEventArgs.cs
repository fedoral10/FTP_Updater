/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		DownloadAsStringCompletedEventArgs.cs
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
    /// String event args
    /// </summary>
    public class DownloadAsStringCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the file as string.
        /// </summary>
        /// <value>The file as string.</value>
        public String FileAsString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAsStringCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="fileAsString">The file as string.</param>
        /// <param name="error">The error.</param>
        public DownloadAsStringCompletedEventArgs(String fileAsString, Exception error)
            : base(error)
        {
            this.FileAsString = fileAsString;
        } // end constructor
    }

}