using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NEventStore.Persistence.AzureStorage
{
    /// <summary>
    /// Holds the header infomation for a stream blob.
    /// </summary>
    [Serializable]
    internal class StreamBlobHeader
    {
        private readonly IList<PageBlobCommitDefinition> _pageBlobCommitDefinitions =
            new List<PageBlobCommitDefinition>();

        /// <summary>
        /// List of commits indices ( in _pageBlobCommitDefinitions ) that are undispatched. 
        /// </summary>
        public int UndispatchedCommitCount { get; set; }

        /// <summary>
        /// Get the last commit sequence
        /// </summary>
        public int LastCommitSequence { get; set; }

        /// <summary>
        /// A read only collection of page blob commit information.
        /// </summary>
        public ReadOnlyCollection<PageBlobCommitDefinition> PageBlobCommitDefinitions => new ReadOnlyCollection<PageBlobCommitDefinition>(_pageBlobCommitDefinitions);

        /// <summary>
        /// Adds a PageBlobCommitDefinition to the end of the list.
        /// </summary>
        /// <param name="definition">The definition to be added</param>
        public void AppendPageBlobCommitDefinition(PageBlobCommitDefinition definition)
        {
            _pageBlobCommitDefinitions.Add(definition);
        }
    }
}