/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		RenameCompletedEventArgs.cs
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
    /// RenameCompletedEventArgs
    /// </summary>
    public class RenameCompletedEventArgs : FTPEventArgs
    {
        /// <summary>
        /// Gets or sets the old name.
        /// </summary>
        /// <value>The old name.</value>
        public String OldName { get; set; }

        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        /// <value>The new name.</value>
        public String NewName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="error">The error.</param>
        public RenameCompletedEventArgs(String oldName, String newName, Exception error)
            : base(error)
        {
            this.OldName = oldName;
            this.NewName = newName;
        }
    }
}