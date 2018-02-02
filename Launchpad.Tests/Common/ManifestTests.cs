﻿//
//  ManifestTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Launchpad.Common;
using Launchpad.Common.Handlers.Manifest;
using Xunit;

namespace Launchpad.Tests.Common
{
	public class ManifestTests
	{
		private readonly ManifestEntry ExpectedObject = new ManifestEntry
		{
			RelativePath = "/data/content.pak",
			Hash = "6A99C575AB87F8C7D1ED1E52E7E349CE",
			Size = 2000000000
		};

		private const string ValidInput = "/data/content.pak:6A99C575AB87F8C7D1ED1E52E7E349CE:2000000000";
		private const string InvalidInputNegativeSize = "/data/content.pak:6A99C575AB87F8C7D1ED1E52E7E349CE:-1";
		private const string InvalidInputHashTooShort = "/data/content.pak:6A99C575AB8:2000000000";
		private const string InvalidInputTooManyElements = "/data/content.pak:6A99C575AB87F8C7D1ED1E52E7E349CE:2000000000:ExtraData";
		private const string InvalidInputInvalidNumber = "/data/content.pak:6A99C575AB87F8C7D1ED1E52E7E349CE:deadbeef";
		private const string InvalidInputMissingHash = "/data/content.pak::2000000000";


		private readonly ManifestEntry ValidObject;

		private const string ExpectedOutputString = ValidInput;

		private const string SampleManifestWindowsStyle =
			"\\GameVersion.txt:7D23FF901039AEF6293954D33D23C066:5\r\n" +
			"\\MyGame.exe:D41D8CD98F00B204E9800998ECF8427E:0\r\n" +
			"\\MyGame.txt:170606695BC36CDC3455E29ADBEE0D40:28\r\n";

		private const string SampleManifestUnixStyle =
			"/GameVersion.txt:7D23FF901039AEF6293954D33D23C066:5\n" +
			"/MyGame.exe:D41D8CD98F00B204E9800998ECF8427E:0\n" +
			"/MyGame.txt:170606695BC36CDC3455E29ADBEE0D40:28\n";

		private readonly List<ManifestEntry> SampleManifestEntries;

		public ManifestTests()
		{
			this.ValidObject = new ManifestEntry
			{
				Hash = "6A99C575AB87F8C7D1ED1E52E7E349CE",
				RelativePath = PlatformHelpers.IsRunningOnUnix() ? "/data/content1.pak" : "\\data\\content1.pak",
				Size = 3000000000
			};

			this.SampleManifestEntries = new List<ManifestEntry>
			{
				new ManifestEntry
				{
					Hash = "7D23FF901039AEF6293954D33D23C066",
					RelativePath = PlatformHelpers.IsRunningOnUnix() ? "/GameVersion.txt" : "\\GameVersion.txt",
					Size = 5
				},
				new ManifestEntry
				{
					Hash = "D41D8CD98F00B204E9800998ECF8427E",
					RelativePath = PlatformHelpers.IsRunningOnUnix() ? "/MyGame.exe" : "\\MyGame.exe",
					Size = 0
				},
				new ManifestEntry
				{
					Hash = "170606695BC36CDC3455E29ADBEE0D40",
					RelativePath = PlatformHelpers.IsRunningOnUnix() ? "/MyGame.txt" : "\\MyGame.txt",
					Size = 28
				}
			};
		}


		[Fact]
		public void TestCreateFromValidInput()
		{
			bool parsingSucceded = ManifestEntry.TryParse(ValidInput, out var createdEntry);

			Assert.True(parsingSucceded);
			Assert.Equal(this.ExpectedObject, createdEntry);
		}

		[Fact]
		public void TestInvalidEmptyString()
		{
			bool parsingSucceeded = ManifestEntry.TryParse(string.Empty, out ManifestEntry _);
			Assert.False(parsingSucceeded);
		}

		[Fact]
		public void TestInvalidNullInput()
		{
			bool parsingSucceeded = ManifestEntry.TryParse(null, out ManifestEntry _);
			Assert.False(parsingSucceeded);
		}

		[Fact]
		public void TestInvalidNegativeSize()
		{
			bool parsingSucceded = ManifestEntry.TryParse(InvalidInputNegativeSize, out _);

			Assert.False(parsingSucceded);
		}

		[Fact]
		public void TestInvalidHashTooShort()
		{
			bool parsingSucceded = ManifestEntry.TryParse(InvalidInputHashTooShort, out _);

			Assert.False(parsingSucceded);
		}

		[Fact]
		public void TestInvalidTooManyElements()
		{
			bool parsingSucceded = ManifestEntry.TryParse(InvalidInputTooManyElements, out _);

			Assert.False(parsingSucceded);
		}

		[Fact]
		public void TestInvalidInvalidNumber()
		{
			bool parsingSucceded = ManifestEntry.TryParse(InvalidInputInvalidNumber, out _);

			Assert.False(parsingSucceded);
		}

		[Fact]
		public void TestInvalidMissingHash()
		{
			bool parsingSucceded = ManifestEntry.TryParse(InvalidInputMissingHash, out _);

			Assert.False(parsingSucceded);
		}

		[Fact]
		public void TestToString()
		{
			Assert.Equal(ExpectedOutputString, this.ExpectedObject.ToString());
		}

		[Fact]
		public void TestObjectsNotEqualNull()
		{
			Assert.False(this.ValidObject.Equals(null));
		}

		[Fact]
		public void TestObjectsNotEqualOtherType()
		{
			Assert.False(this.ValidObject.Equals(new object()));
		}

		[Fact]
		public void TestObjectsEqual()
		{
			Assert.True(this.ValidObject.Equals(this.ValidObject));
		}

		[Fact]
		public void TestLoadManifestWindowsStyle()
		{
			IReadOnlyList<ManifestEntry> loadedEntries;
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(SampleManifestWindowsStyle)))
			{
				loadedEntries = ManifestHandler.LoadManifest(ms);
			}

			Assert.Equal(loadedEntries, SampleManifestEntries);
		}

		[Fact]
		public void TestLoadManifestUnixStyle()
		{
			IReadOnlyList<ManifestEntry> loadedEntries;
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(SampleManifestUnixStyle)))
			{
				loadedEntries = ManifestHandler.LoadManifest(ms);
			}

			Assert.Equal(loadedEntries, SampleManifestEntries);
		}
	}
}