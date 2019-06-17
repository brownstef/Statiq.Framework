﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Statiq.Common.Documents;
using Statiq.Core.Documents;
using Statiq.Core.Meta;
using Statiq.Testing;
using Statiq.Testing.Execution;

namespace Statiq.Core.Tests.Documents
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self | ParallelScope.Children)]
    public class CustomDocumentFactoryFixture : BaseFixture
    {
        public class GetDocumentTests : CustomDocumentFactoryFixture
        {
            [Test]
            public void GetsInitialDocumentWithInitialMetadata()
            {
                // Given
                MetadataDictionary initialMetadata = new MetadataDictionary();
                initialMetadata.Add("Foo", "Bar");
                DocumentFactory documentFactory = new DocumentFactory(initialMetadata);
                CustomDocumentFactory<TestDocument> customDocumentFactory = new CustomDocumentFactory<TestDocument>(documentFactory);
                TestExecutionContext context = new TestExecutionContext();

                // When
                IDocument resultDocument = customDocumentFactory.GetDocument(context, null, null, null, null, null);

                // Then
                Assert.IsInstanceOf<TestDocument>(resultDocument);
                CollectionAssert.AreEqual(
                    new Dictionary<string, object>
                    {
                        { "Foo", "Bar" }
                    }, resultDocument);
            }

            [Test]
            public void ThrowsWhenCloneReturnsNullDocument()
            {
                // Given
                MetadataDictionary initialMetadata = new MetadataDictionary();
                DocumentFactory documentFactory = new DocumentFactory(initialMetadata);
                CustomDocumentFactory<TestDocument> customDocumentFactory = new CustomDocumentFactory<TestDocument>(documentFactory);
                TestExecutionContext context = new TestExecutionContext();
                CloneReturnsNullDocument document = new CloneReturnsNullDocument();

                // When, Then
                Assert.Throws<Exception>(() => customDocumentFactory.GetDocument(context, document, null, null, new Dictionary<string, object>(), null));
            }

            [Test]
            public void ThrowsWhenCloneReturnsSameDocument()
            {
                // Given
                MetadataDictionary initialMetadata = new MetadataDictionary();
                DocumentFactory documentFactory = new DocumentFactory(initialMetadata);
                CustomDocumentFactory<TestDocument> customDocumentFactory = new CustomDocumentFactory<TestDocument>(documentFactory);
                TestExecutionContext context = new TestExecutionContext();
                CloneReturnsSameDocument document = new CloneReturnsSameDocument();

                // When, Then
                Assert.Throws<Exception>(() => customDocumentFactory.GetDocument(context, document, null, null, new Dictionary<string, object>(), null));
            }

            [Test]
            public void CloneResultsInClonedDocument()
            {
                // Given
                MetadataDictionary initialMetadata = new MetadataDictionary();
                initialMetadata.Add("Foo", "Bar");
                DocumentFactory documentFactory = new DocumentFactory(initialMetadata);
                CustomDocumentFactory<TestDocument> customDocumentFactory = new CustomDocumentFactory<TestDocument>(documentFactory);
                TestExecutionContext context = new TestExecutionContext();
                CustomDocument sourceDocument = (CustomDocument)customDocumentFactory.GetDocument(context, null, null, null, null, null);

                // When
                IDocument resultDocument = customDocumentFactory.GetDocument(
                    context,
                    sourceDocument,
                    null,
                    null,
                    new Dictionary<string, object>
                    {
                        { "Baz", "Bat" }
                    },
                    null);

                // Then
                CollectionAssert.AreEquivalent(
                    new Dictionary<string, object>
                    {
                        { "Foo", "Bar" }
                    },
                    sourceDocument);
                CollectionAssert.AreEquivalent(
                    new Dictionary<string, object>
                    {
                        { "Foo", "Bar" },
                        { "Baz", "Bat" }
                    },
                    resultDocument);
            }
        }

        private class TestDocument : CustomDocument
        {
            public string Title { get; set; }

            protected internal override CustomDocument Clone()
            {
                return new TestDocument
                {
                    Title = Title
                };
            }
        }

        private class CloneReturnsNullDocument : CustomDocument
        {
            protected internal override CustomDocument Clone()
            {
                return null;
            }
        }

        private class CloneReturnsSameDocument : CustomDocument
        {
            protected internal override CustomDocument Clone()
            {
                return this;
            }
        }
    }
}