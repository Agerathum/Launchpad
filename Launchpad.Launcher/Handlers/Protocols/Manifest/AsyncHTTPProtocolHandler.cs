﻿//
//  HTTPProtocolHandler.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See themanifest
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Launchpad.Common;
using Launchpad.Common.Enums;

namespace Launchpad.Launcher.Handlers.Protocols.Manifest
{
	/// <summary>
	/// HTTP protocol handler. Patches the launcher and game using the
	/// HTTP/HTTPS protocol.
	/// </summary>
	internal sealed class AsyncHTTPProtocolHandler : AsyncManifestBasedProtocolHandler
	{
		/// <summary>
		/// Logger instance for this class.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(HTTPProtocolHandler));

		/// <summary>
		/// Determines whether this instance can provide patches. Checks for an active connection to the
		/// patch provider (file server, distributed hash tables, hyperspace compression waves etc.)
		/// </summary>
		/// <returns><c>true</c> if this instance can provide patches; otherwise, <c>false</c>.</returns>
		public override async Task<bool> CanPatchAsync(CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();
			Log.Info("Pinging remote patching server to determine if we can connect to it.");

			bool bCanConnectToServer = false;

			try
			{
				HttpWebRequest plainRequest = CreateHttpWebRequest(this.Config.GetBaseHTTPUrl(), this.Config.GetRemoteUsername(), this.Config.GetRemotePassword());

				if (plainRequest == null)
				{
					return false;
				}

				plainRequest.Method = WebRequestMethods.Http.Head;
				plainRequest.Timeout = 4000;

				try
				{
					ct.ThrowIfCancellationRequested();
					using (HttpWebResponse response = (HttpWebResponse) await plainRequest.GetResponseAsync())
					{
						if (response.StatusCode == HttpStatusCode.OK)
						{
							bCanConnectToServer = true;
						}
					}
				}
				catch (WebException wex)
				{
					Log.Warn("Unable to connect to remote patch server (WebException): " + wex.Message);
					bCanConnectToServer = false;
				}
			}
			catch (WebException wex)
			{
				Log.Warn("Unable to connect due a malformed url in the configuration (WebException): " + wex.Message);
				bCanConnectToServer = false;
			}

			return bCanConnectToServer;
		}

		/// <summary>
		/// Determines whether the protocol can provide patches and updates for the provided platform.
		/// </summary>
		/// <returns><c>true</c> if the platform is available; otherwise, <c>false</c>.</returns>
		public override async Task<bool> IsPlatformAvailableAsync(CancellationToken ct, ESystemTarget platform)
		{
			string remote = $"{this.Config.GetBaseHTTPUrl()}/game/{platform}/.provides";

			return await DoesRemoteDirectoryOrFileExistAsync(remote);
		}

		/// <summary>
		/// Determines whether this protocol can provide access to a changelog.
		/// </summary>
		/// <returns><c>true</c> if this protocol can provide a changelog; otherwise, <c>false</c>.</returns>
		public override async Task<bool> CanProvideChangelogAsync(CancellationToken ct)
		{
			return await Task.FromResult(false);
		}

		/// <summary>
		/// Gets the changelog.
		/// </summary>
		/// <returns>The changelog.</returns>
		public override async Task<string> GetChangelogSourceAsync(CancellationToken ct)
		{
			return await Task.FromResult(string.Empty);
		}

		/// <summary>
		/// Determines whether this protocol can provide access to a banner for the game.
		/// </summary>
		/// <returns><c>true</c> if this instance can provide banner; otherwise, <c>false</c>.</returns>
		public override async Task<bool> CanProvideBannerAsync(CancellationToken ct)
		{
			string bannerURL = $"{this.Config.GetBaseHTTPUrl()}/launcher/banner.png";

			return await DoesRemoteDirectoryOrFileExistAsync(bannerURL);
		}

		/// <summary>
		/// Gets the banner.
		/// </summary>
		/// <returns>The banner.</returns>
		public override async Task<Bitmap> GetBannerAsync(CancellationToken ct)
		{
			string bannerURL = $"{this.Config.GetBaseHTTPUrl()}/launcher/banner.png";
			string localBannerPath = $"{Path.GetTempPath()}/banner.png";

			await DownloadRemoteFileAsync(ct, bannerURL, localBannerPath);
			return new Bitmap(localBannerPath);
		}

		/// <summary>
		/// Downloads a remote file to a local file path.
		/// </summary>
		/// <param name="url">The remote url of the file..</param>
		/// <param name="localPath">Local path where the file is to be stored.</param>
		/// <param name="totalSize">Total size of the file as stated in the manifest.</param>
		/// <param name="contentOffset">Content offset. If nonzero, appends data to an existing file.</param>
		/// <param name="useAnonymousLogin">If set to <c>true</c> use anonymous login.</param>
		protected override async Task DownloadRemoteFileAsync(CancellationToken ct, string url, string localPath, long totalSize = 0, long contentOffset = 0, bool useAnonymousLogin = false)
		{
			// Early bail out
			ct.ThrowIfCancellationRequested();

			//clean the url string
			string remoteURL = url.Replace(Path.DirectorySeparatorChar, '/');

			string username;
			string password;
			if (useAnonymousLogin)
			{
				username = "anonymous";
				password = "anonymous";
			}
			else
			{
				username = this.Config.GetRemoteUsername();
				password = this.Config.GetRemotePassword();
			}

			try
			{
				ct.ThrowIfCancellationRequested();
				HttpWebRequest request = CreateHttpWebRequest(remoteURL, username, password);

				request.Method = WebRequestMethods.Http.Get;
				request.AddRange(contentOffset);

				using (Stream contentStream = (await request.GetResponseAsync()).GetResponseStream())
				{
					if (contentStream == null)
					{
						Log.Error($"Failed to download the remote file at \"{remoteURL}\" (NullReferenceException from the content stream). " +
								  "Check your internet connection.");

						return;
					}

					using (FileStream fileStream = contentOffset > 0 ? new FileStream(localPath, FileMode.Append) :
																		new FileStream(localPath, FileMode.Create))
					{
						fileStream.Position = contentOffset;
						long totalBytesDownloaded = contentOffset;

						long totalFileSize;
						if (contentStream.CanSeek)
						{
							totalFileSize = contentOffset + contentStream.Length;
						}
						else
						{
							totalFileSize = totalSize;
						}

						int bufferSize = this.Config.GetDownloadBufferSize();
						byte[] buffer = new byte[bufferSize];

						while (true)
						{
							ct.ThrowIfCancellationRequested();
							int bytesRead = contentStream.Read(buffer, 0, buffer.Length);

							if (bytesRead == 0)
							{
								break;
							}

							fileStream.Write(buffer, 0, bytesRead);

							totalBytesDownloaded += bytesRead;

							// Report download progress
							this.ModuleDownloadProgressArgs.ProgressBarMessage = GetDownloadProgressBarMessage(Path.GetFileName(remoteURL),
								totalBytesDownloaded, totalFileSize);
							this.ModuleDownloadProgressArgs.ProgressFraction = totalBytesDownloaded / (double)totalFileSize;
							OnModuleDownloadProgressChanged();
						}

						fileStream.Flush();
					}
				}
			}
			catch (WebException wex)
			{
				Log.Error($"Failed to download the remote file at \"{remoteURL}\" (WebException): {wex.Message}");
			}
			catch (IOException ioex)
			{
				Log.Error($"Failed to download the remote file at \"{remoteURL}\" (IOException): {ioex.Message}");
			}
		}

		/// <summary>
		/// Reads the string content of a remote file. The output is scrubbed
		/// of all \r, \n and \0 characters before it is returned.
		/// </summary>
		/// <returns>The contents of the remote file.</returns>
		/// <param name="url">The remote url of the file.</param>
		/// <param name="useAnonymousLogin">If set to <c>true</c> use anonymous login.</param>
		protected override async Task<string> ReadRemoteFileAsync(CancellationToken ct, string url, bool useAnonymousLogin = false)
		{
			// Early bail out
			ct.ThrowIfCancellationRequested();
			
			string remoteURL = url.Replace(Path.DirectorySeparatorChar, '/');

			string username;
			string password;
			if (useAnonymousLogin)
			{
				username = "anonymous";
				password = "anonymous";

			}
			else
			{
				username = this.Config.GetRemoteUsername();
				password = this.Config.GetRemotePassword();
			}

			try
			{
				ct.ThrowIfCancellationRequested();
				HttpWebRequest request = CreateHttpWebRequest(remoteURL, username, password);

				request.Method = WebRequestMethods.Http.Get;

				string data = "";
				using (Stream remoteStream = (await request.GetResponseAsync()).GetResponseStream())
				{
					// Drop out early if the stream wasn't present
					if (remoteStream == null)
					{
						Log.Error($"Failed to read the contents of remote file \"{remoteURL}\": " +
						          "Remote stream was null. This could be due to a network interruption " +
						          "or issues with the remote file.");

						return string.Empty;
					}

					int bufferSize = this.Config.GetDownloadBufferSize();
					byte[] buffer = new byte[bufferSize];

					while (true)
					{
						ct.ThrowIfCancellationRequested();
						int bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

						if (bytesRead == 0)
						{
							break;
						}

						data += Encoding.UTF8.GetString(buffer, 0, bytesRead);
					}
				}

				return data.RemoveLineSeparatorsAndNulls();
			}
			catch (WebException wex)
			{
				Log.Error($"Failed to read the contents of remote file \"{remoteURL}\" (WebException): {wex.Message}");
				return string.Empty;
			}
			catch (NullReferenceException nex)
			{
				Log.Error("Failed to establish a network connection, or the connection was interrupted during the download (NullReferenceException): " + nex.Message);
				return string.Empty;
			}
		}

		/// <summary>
		/// Creates a HTTP web request.
		/// </summary>
		/// <returns>The HTTP web request.</returns>
		/// <param name="url">url of the desired remote object.</param>
		/// <param name="username">The username used for authentication.</param>
		/// <param name="password">The password used for authentication.</param>
		private static HttpWebRequest CreateHttpWebRequest(string url, string username, string password)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
				request.Proxy = null;
				request.Credentials = new NetworkCredential(username, password);

				return request;
			}
			catch (WebException wex)
			{
				Log.Warn("Unable to create a WebRequest for the specified file (WebException): " + wex.Message);
				return null;
			}
			catch (ArgumentException aex)
			{
				Log.Warn("Unable to create a WebRequest for the specified file (ArgumentException): " + aex.Message);
				return null;
			}
			catch (UriFormatException uex)
			{
				Log.Warn("Unable to create a WebRequest for the specified file (UriFormatException): " + uex.Message + "\n" +
					"You may need to add \"http://\" before the url in the config.");
				return null;
			}
		}

		/// <summary>
		/// Checks if the provided path points to a valid directory or file.
		/// </summary>
		/// <returns><c>true</c>, if the directory or file exists, <c>false</c> otherwise.</returns>
		/// <param name="url">The remote url of the directory or file.</param>
		private async Task<bool> DoesRemoteDirectoryOrFileExistAsync(string url)
		{
			string cleanURL = url.Replace(Path.DirectorySeparatorChar, '/');
			HttpWebRequest request = CreateHttpWebRequest(cleanURL, this.Config.GetRemoteUsername(), this.Config.GetRemotePassword());

			request.Method = WebRequestMethods.Http.Head;
			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse) await request.GetResponseAsync();
				if (response.StatusCode != HttpStatusCode.OK)
				{
					return false;
				}
			}
			catch (WebException wex)
			{
				response = (HttpWebResponse)wex.Response;
				if (response.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
			}
			finally
			{
				response?.Dispose();
			}

			return true;
		}
	}
}

