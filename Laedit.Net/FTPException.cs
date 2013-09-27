/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		FTPException.cs
 *  Author:     		Jérémie Bertrand
 *  Last modification:	21/07/2010
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
    /// Exception throw on the ftp
    /// </summary>
    public class FTPException : Exception
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public FTPException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">message of the exception</param>
        public FTPException(String message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">message of the exception</param>
        /// <param name="inner">another exception</param>
        public FTPException(String message, Exception inner) : base(message, inner) { }
    }
}