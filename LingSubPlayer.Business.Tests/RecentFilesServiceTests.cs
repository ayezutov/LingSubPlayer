using System;
using System.Collections.Generic;
using LingSubPlayer.Common.Data;
using LingSubPlayer.DataAccess;
using NSubstitute;
using NUnit.Framework;

namespace LingSubPlayer.Business.Tests
{
    [TestFixture]
    public class RecentFilesServiceTests
    {
        [TestCaseSource("AddToListData")]
        public void AddToList(IList<SessionData> initial, SessionData current, IList<SessionData> expected)
        {
            ValidateWhatIsSavedToRepo(initial, expected, service => service.AddToRecentSessions(current));
        }

        [TestCaseSource("RemoveFromListData")]
        public void RemoveFromList(IList<SessionData> initial, SessionData current, IList<SessionData> expected)
        {
            ValidateWhatIsSavedToRepo(initial, expected, service => service.RemoveFromRecentSessions(current));
        }

        private static void ValidateWhatIsSavedToRepo(IList<SessionData> initial, IList<SessionData> expected, Action<RecentFilesService> serviceAction)
        {
            var repo = Substitute.For<IRecentFilesRepository>();

            IList<SessionData> actual = null;

            repo.When(repository => repository
                .SaveRecentSessions(Arg.Any<IList<SessionData>>()))
                .Do(info => actual = info.Arg<IList<SessionData>>());

            repo.GetRecentSessions().Returns(initial);

            var service = new RecentFilesService(repo);
            serviceAction(service);

            Assert.That(actual, Is.EqualTo(expected));
        }

        public IEnumerable<TestCaseData> AddToListData
        {
            get
            {
                return new List<TestCaseData>
                {
                    new TestCaseData(
                        null, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"}
                        })
                        .SetName("List is created and element is added"),
                    new TestCaseData(
                        new List<SessionData> {}, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"}
                        })
                        .SetName("Element is added to empty list"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"}
                        }, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"}
                        })
                        .SetName("Element is not duplicated"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "2"}
                        }, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"}
                        })
                        .SetName("Element is added to the beginning"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "4"},
                        }, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "4"}
                        })
                        .SetName("Element is added to the beginning in bigger collection"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "1"}
                        }, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "2"}
                        })
                        .SetName("Element is moved to the beginning"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "4"},
                            new SessionData(){VideoFileName = "5"},
                        }, 
                        new SessionData{VideoFileName = "0"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "0"},
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "4"},
                        })
                        .SetName("List is truncated up to max"),
                    
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "4"},
                            new SessionData(){VideoFileName = "5"},
                        }, 
                        new SessionData{VideoFileName = "3"}, 
                        new List<SessionData>
                        {
                            new SessionData(){VideoFileName = "3"},
                            new SessionData(){VideoFileName = "1"},
                            new SessionData(){VideoFileName = "2"},
                            new SessionData(){VideoFileName = "4"},
                            new SessionData(){VideoFileName = "5"},
                        })
                        .SetName("List is not truncated when item is replaced"),
                    
                };
            }
        }

        public IEnumerable<TestCaseData> RemoveFromListData
        {
            get
            {
                return new List<TestCaseData>
                {
                    new TestCaseData(
                        null, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>{})
                        .SetName("List is created if not existent"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData{VideoFileName = "1"}
                        }, 
                        new SessionData{VideoFileName = "1"}, 
                        new List<SessionData>{})
                        .SetName("Element is removed"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData{VideoFileName = "1"},
                            new SessionData{VideoFileName = "2"},
                            new SessionData{VideoFileName = "3"},
                            new SessionData{VideoFileName = "4"},
                        }, 
                        new SessionData{VideoFileName = "3"}, 
                        new List<SessionData>
                        {
                            new SessionData{VideoFileName = "1"},
                            new SessionData{VideoFileName = "2"},
                            new SessionData{VideoFileName = "4"},
                        })
                        .SetName("Element is removed and order is preserved"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData()
                        }, 
                        new SessionData(), 
                        new List<SessionData>{})
                        .SetName("Empty instance is removed"),
                    new TestCaseData(
                        new List<SessionData>
                        {
                            new SessionData()
                        }, 
                        new SessionData()
                        {
                            VideoFileName = ""
                        }, 
                        new List<SessionData>{})
                        .SetName("Empty instance is removed when some fields are empty strings"),
                    
                };
            }
        }
    }
}
