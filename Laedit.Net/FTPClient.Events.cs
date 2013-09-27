/*  ----------------------------------------------------------------------------
 *  Laedit.Net : Laedit.Net
 *  ----------------------------------------------------------------------------
 *  File:       		FTPClient.Events.cs
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
using System.Threading;

namespace Laedit.Net
{
	/// <summary>
	/// Represent an FTP client
	/// </summary>
	public partial class FTPClient
	{

		#region Events

		/// <summary>
		/// Occurs the FPTCllient downloads files or creates directory.
		/// </summary>
		public event EventHandler<DownloadFileOrCreateDirectoryEventArgs> DownloadFilesOrCreateDirectory;

		/// <summary>
		/// Occurs when [download as string completed].
		/// </summary>
		public event EventHandler<DownloadAsStringCompletedEventArgs> DownloadAsStringCompleted;
		
		/// <summary>
		/// Occurs when [delete completed].
		/// </summary>
		public event EventHandler<DeleteCompletedEventArgs> DeleteCompleted;
		
		/// <summary>
		/// Occurs when [delete dir completed].
		/// </summary>
		public event EventHandler<DeleteDirCompletedEventArgs> DeleteDirCompleted;
		
		/// <summary>
		/// Occurs when [download completed].
		/// </summary>
		public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

		/// <summary>
		/// Occurs when [get files detail list completed].
		/// </summary>
		public event EventHandler<GetFilesDetailListCompletedEventArgs> GetFilesDetailListCompleted;

		/// <summary>
		/// Occurs when [get files list completed].
		/// </summary>
		public event EventHandler<GetFilesListCompletedEventArgs> GetFilesListCompleted;

		/// <summary>
		/// Occurs when [get file size completed].
		/// </summary>
		public event EventHandler<GetFileSizeCompletedEventArgs> GetFileSizeCompleted;

		/// <summary>
		/// Occurs when [make dir completed].
		/// </summary>
		public event EventHandler<MakeDirCompletedEventArgs> MakeDirCompleted;
		
		/// <summary>
		/// Occurs when [rename completed].
		/// </summary>
		public event EventHandler<RenameCompletedEventArgs> RenameCompleted;
		
		/// <summary>
		/// Occurs when [upload completed].
		/// </summary>
		public event EventHandler<UploadCompletedEventArgs> UploadCompleted;
		
		/// <summary>
		/// Occurs when [download all files completed].
		/// </summary>
		public event EventHandler<DownloadAllFilesCompletedEventArgs> DownloadAllFilesCompleted;

		/// <summary>
		/// Occurs when [download async completed].
		/// </summary>
		private event EventHandler<DownloadAsyncCompletedEventArgs> DownloadAsyncCompleted;

		/// <summary>
		/// Occurs when [get files detail list async completed for download files async].
		/// </summary>
		private event EventHandler<GetFilesDetailListCompletedForDownloadFilesEventArgs> GetFilesDetailListAsyncCompletedForDownloadFilesAsync;

		/// <summary>
		/// Occurs when [get files detail list async completed for download files async recurs].
		/// </summary>
		private event EventHandler<GetFilesDetailListCompletedForDownloadFilesEventArgs> GetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs;

		#endregion Events


		#region Methods

		/// <summary>
		/// Called when [download as string completed].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="error">The error.</param>
		private void OnDownloadAsStringCompleted(String value, Exception error)
		{
			if (this.DownloadAsStringCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DownloadAsStringCompleted(this, new DownloadAsStringCompletedEventArgs(value, error));
				}));
				t.Start();
			}
		} // end procedure OnDownloadAsStringCompleted

		/// <summary>
		/// Called when [delete completed].
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="error">The error.</param>
		private void OnDeleteCompleted(String fileName, Exception error)
		{
			if (this.DeleteCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DeleteCompleted(this, new DeleteCompletedEventArgs(fileName, error));
				}));
				t.Start();
			}
		} // end procedure OnDeleteCompleted

		/// <summary>
		/// Called when [delete dir completed].
		/// </summary>
		/// <param name="directoryPath">The directory path.</param>
		/// <param name="error">The error.</param>
		private void OnDeleteDirCompleted(String directoryPath, Exception error)
		{
			if (this.DeleteDirCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DeleteDirCompleted(this, new DeleteDirCompletedEventArgs(directoryPath, error));
				}));
				t.Start();
			}
		} // end procedure OnDeleteCompleted

		/// <summary>
		/// Called when [download completed].
		/// </summary>
		/// <param name="ftpFileName">Name of the FTP file.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="error">The error.</param>
		private void OnDownloadCompleted(String ftpFileName, String fileName, Exception error)
		{
			if (this.DownloadCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DownloadCompleted(this, new DownloadCompletedEventArgs(ftpFileName, fileName, error));
				}));
				t.Start();
			}
		} // end procedure OnDownloadCompleted

		/// <summary>
		/// Called when [get files detail list completed].
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="folder">The folder.</param>
		/// <param name="error">The error.</param>
		private void OnGetFilesDetailListCompleted(List<String[]> values, String folder, Exception error)
		{
			if (this.GetFilesDetailListCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.GetFilesDetailListCompleted(this, new GetFilesDetailListCompletedEventArgs(values, folder, error));
				}));
				t.Start();
			}
		} // end procedure OnGetFilesDetailListCompleted

		/// <summary>
		/// Called when [get files list completed].
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="folder">The folder.</param>
		/// <param name="error">The error.</param>
		private void OnGetFilesListCompleted(String[] values, String folder, Exception error)
		{
			if (this.GetFilesListCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.GetFilesListCompleted(this, new GetFilesListCompletedEventArgs(values, folder, error));
				}));
				t.Start();
			}
		} // end procedure OnGetFilesListCompleted

		/// <summary>
		/// Called when [get file size completed].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="file">The file.</param>
		/// <param name="error">The error.</param>
		private void OnGetFileSizeCompleted(Int64 value, String file, Exception error)
		{
			if (this.GetFileSizeCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.GetFileSizeCompleted(this, new GetFileSizeCompletedEventArgs(value, file, error));
				}));
				t.Start();
			}
		} // end procedure OnGetFileSizeCompleted

		/// <summary>
		/// Called when [make dir completed].
		/// </summary>
		/// <param name="isCreated">if set to <c>true</c> [is created].</param>
		/// <param name="directoryPath">The directory path.</param>
		/// <param name="error">The error.</param>
		private void OnMakeDirCompleted(Boolean isCreated, String directoryPath, Exception error)
		{
			if (this.MakeDirCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.MakeDirCompleted(this, new MakeDirCompletedEventArgs(isCreated, directoryPath, error));
				}));
				t.Start();
			}
		} // end procedure OnMakeDirCompleted

		/// <summary>
		/// Called when [rename completed].
		/// </summary>
		/// <param name="oldName">The old name.</param>
		/// <param name="newName">The new name.</param>
		/// <param name="error">The error.</param>
		private void OnRenameCompleted(String oldName, String newName, Exception error)
		{
			if (this.RenameCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.RenameCompleted(this, new RenameCompletedEventArgs(oldName, newName, error));
				}));
				t.Start();
			}
		} // end procedure OnRenameCompleted

		/// <summary>
		/// Called when [completed].
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="ftpFileName">Name of the FTP file.</param>
		/// <param name="error">The error.</param>
		private void OnUploadCompleted(String fileName, String ftpFileName, Exception error)
		{
			if (this.UploadCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.UploadCompleted(this, new UploadCompletedEventArgs(fileName, ftpFileName, error));
				}));
				t.Start();
			}
		} // end procedure OnUploadCompleted

		/// <summary>
		/// Called when [download all files completed].
		/// </summary>
		/// <param name="ftpFolder">The FTP folder.</param>
		/// <param name="folder">The folder.</param>
		private void OnDownloadAllFilesCompleted(String ftpFolder, String folder)
		{
			if (this.DownloadAllFilesCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DownloadAllFilesCompleted(this, new DownloadAllFilesCompletedEventArgs(ftpFolder, folder));
				}));
				t.Start();
			}
		} // end procedure OnDownloadAllFilesCompleted

		/// <summary>
		/// Called when FTPClient downloads files or creates directory.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="fileType">Type of the file.</param>
		/// <exception cref="ArgumentNullException">A parameter is null</exception>
		private void OnDownloadFilesOrCreateDirectory(String file, FTPFileType fileType)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}

			if (this.DownloadFilesOrCreateDirectory != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DownloadFilesOrCreateDirectory(this, new DownloadFileOrCreateDirectoryEventArgs(file, fileType));
				}));
				t.Start();
			}
		} // end procedure OnDownloadFilesOrCreateDirectory

		/// <summary>
		/// Called when [download async completed].
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="folder">The folder.</param>
		/// <param name="guid">The GUID.</param>
		private void OnDownloadAsyncCompleted(String fileName, String folder, Guid guid)
		{
			if (this.DownloadAsyncCompleted != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.DownloadAsyncCompleted(this, new DownloadAsyncCompletedEventArgs(fileName, folder, guid));
				}));
				t.Start();
			}
		} // end procedure OnDownloadAsyncCompleted

		/// <summary>
		/// Called when [get files detail list completed].
		/// </summary>
		private void OnGetFilesDetailListAsyncCompletedForDownloadFilesAsync(Guid guid, String folder, List<String[]> values)
		{
			if (this.GetFilesDetailListAsyncCompletedForDownloadFilesAsync != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.GetFilesDetailListAsyncCompletedForDownloadFilesAsync(this, new GetFilesDetailListCompletedForDownloadFilesEventArgs(guid, folder, values));
				}));
				t.Start();
			}
		} // end procedure OnGetFilesDetailListAsyncCompletedForDownloadFilesAsync

		/// <summary>
		/// Called when [get files detail list completed].
		/// </summary>
		private void OnGetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs(Guid guid, String rootFolder, String folder, List<String[]> values)
		{
			if (this.GetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs != null)
			{
				Thread t = new Thread(new ThreadStart(() =>
				{
					this.GetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs(this, new GetFilesDetailListCompletedForDownloadFilesEventArgs(guid, rootFolder, folder, values));
				}));
				t.Start();
			}
		} // end procedure OnGetFilesDetailListAsyncCompletedForDownloadFilesAsyncRecurs

		#endregion Methods
	}
}