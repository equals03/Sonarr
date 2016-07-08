using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using NzbDrone.Common.Disk.Abstractions;
using NzbDrone.Core.Download.Clients.RssGenerator;
using NzbDrone.Core.Test.Framework;
using FluentAssertions;

namespace NzbDrone.Core.Test.Download.DownloadClientTests.RssGenerator {

    [TestFixture]
    public class TorrentRssGeneratorCachedEpisodeMatcherFixture : CoreTest<TorrentRssGeneratorCachedEpisodeMatcher> {

        protected Mock<IFileSystemInfo> GivenTheFollowingFileSystemInfo(string fullname, string name, string logicalName) {
            var mock = new Mock<IFileSystemInfo>();

            mock.Setup(c => c.FullName)
                .Returns(fullname);
            mock.Setup(c => c.Name)
                .Returns(name);
            mock.Setup(c => c.LogicalName)
                .Returns(logicalName);
            mock.Setup(c => c.Exists)
                .Returns(true);

            return mock;
        }
        protected TorrentRssGeneratorCachedEpisode GivenTheFollowingCachedEpisode(string json) {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TorrentRssGeneratorCachedEpisode>(json);
        }

        [Test]
        public void Should_match_given_normal_release() {
            var fileinfo = this.GivenTheFollowingFileSystemInfo(
                "C:\\foo\\bar\\MasterChef.Au.S08E16.Webrip.x264-MFO.mp4", 
                "MasterChef.Au.S08E16.Webrip.x264-MFO.mp4", 
                "MasterChef.Au.S08E16.Webrip.x264-MFO").Object;

            var episode = this.GivenTheFollowingCachedEpisode("{Title:\"MasterChef Au S08E16 Webrip x264-MFO mp4\"}");

            this.Subject.Matches(episode, fileinfo).Should().BeTrue();
        }

        [Test]
        public void Should_match_given_repack_release() {
            var fileinfo = this.GivenTheFollowingFileSystemInfo(
                "C:\\foo\\bar\\masterchef.australia.s08e22.repack.hdtv.x264-fqm[ettv].mkv",
                "masterchef.australia.s08e22.repack.hdtv.x264-fqm[ettv].mkv",
                "masterchef.australia.s08e22.repack.hdtv.x264-fqm[ettv]").Object;

            var episode = this.GivenTheFollowingCachedEpisode("{\r\n  \"Guid\": \"guid\",\r\n  \"Title\": \"MasterChef Australia S08E22 REPACK HDTV x264-FQM[ettv]\" \r\n}");

            this.Subject.Matches(episode, fileinfo).Should().BeTrue();
        }
    }
}
