﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Tests.RepositoryTests
{
    public class LibraryListsFromDynamicRepositoryTest
    {
        private const string PathString = @"..\..\..\Solvberget.Service\bin\App_Data\librarylists\dynamic";
        private readonly string _imageCache = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Solvberget.Service\Content\cacheImages\");

        private IListRepository _listRepository;

        [TestFixtureSetUp]
        public void Init()
        {
            var path = Path.Combine(Environment.CurrentDirectory, PathString);
            var aleph = new AlephRepository(_imageCache);
            _listRepository = new LibraryListDynamicRepository(aleph, new ImageRepository(aleph, _imageCache), path);
        }

        [Test]
        public void TestCorrectFileCount()
        {
            var result = _listRepository.GetLists();
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void TestListContent()
        {
            var result = _listRepository.GetLists();
            Assert.AreEqual("Nyheter den siste uken: Skjønnlitteratur for voksne", result.ElementAt(0).Name);
            Assert.AreEqual("Nyheter den siste uken: Musikk-cder", result.ElementAt(1).Name);
            Assert.IsFalse(result.ElementAt(0).IsRanked);
            Assert.IsFalse(result.ElementAt(1).IsRanked);
        }

    }
}
