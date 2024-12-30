﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Systran.Actions;
using Apps.Systran.Models.Request;
using SystranTests.Base;

namespace Tests.Systran
{
    [TestClass]
    public class CorpusTests : TestBase
    {
        [TestMethod]
        public async Task ImportCorpus_ValidFile_ReturnsResponse()
        {
            // Arrange
            var fileReference = await FileManager.UploadTestFileAsync("test.tmx", "application/x-tmx+xml");


            var parameters = new ImportCorpusParameters
            {
                Name = "Test Corpus111",
                InputFile = fileReference,
                Tag = new[] { "tag1", "tag2" }
            };

            var actions = new CorpusActions(InvocationContext, FileManager);


            // Act
            var result = await actions.ImportCorpus(parameters);

            // Assert
            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.Corpus, "Corpus data is null.");
            Assert.IsFalse(string.IsNullOrEmpty(result.Corpus.Id), "Corpus ID is missing.");

        }

        [TestMethod]
        public async Task ExportCorpus_ValidCorpusId_ReturnsFileReference()
        {
            var corpusId = "677142345f6f31e36a0192a3";
            var expectedContentType = "application/x-tmx+xml";

            var parameters = new ExportCorpusParameters
            {
                CorpusId = corpusId
            };

            var actions = new CorpusActions(InvocationContext, FileManager);

            var result = await actions.ExportCorpus(parameters);

            Assert.IsNotNull(result, "Response is null.");
            Assert.IsNotNull(result.FileResponse, "FileResponse is null.");
        }
    }
}
