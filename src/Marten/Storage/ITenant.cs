using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Marten.Schema;
using Marten.Schema.Identity;
using Marten.Schema.Identity.Sequences;
using Marten.Services;
using Marten.Transforms;
using Marten.V4Internals;
using Npgsql;

namespace Marten.Storage
{
    public interface ITenantStorage
    {
        /// <summary>
        /// Directs Marten to disregard any previous schema checks. Useful
        /// if you change the underlying schema without shutting down the document store
        /// </summary>
        void ResetSchemaExistenceChecks();

        void MarkAllFeaturesAsChecked();

        /// <summary>
        /// Ensures that the IDocumentStorage object for a document type is ready
        /// and also attempts to update the database schema for any detected changes
        /// </summary>
        /// <param name="documentType"></param>
        void EnsureStorageExists(Type documentType);
    }

    [Obsolete("This gets dramatically thinned down in v4. See the version in the V4 internals")]
    public interface ITenant : ITenantStorage
    {
        string TenantId { get; }

        /// <summary>
        ///     Query against the actual Postgresql database schema objects
        /// </summary>
        [Obsolete("Make this a method? Put on Schema?")]
        IDbObjects DbObjects { get; }

        /// <summary>
        /// Retrieves or generates the active IDocumentStorage object
        /// for the given document type
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        IDocumentStorage StorageFor(Type documentType);

        /// <summary>
        /// Finds or creates the IDocumentMapping for a document type
        /// that governs how that document type is persisted and queried
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        [Obsolete("Goes away in v4")]
        IDocumentMapping MappingFor(Type documentType);

        /// <summary>
        /// Used to create new Hilo sequences
        /// </summary>
        [Obsolete("Goes away in v4")]
        ISequences Sequences { get; }

        [Obsolete("Goes away in v4")]
        IdAssignment<T> IdAssignmentFor<T>();

        TransformFunction TransformFor(string name);


        /// <summary>
        ///     Directly open a managed connection to the underlying Postgresql database
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="isolationLevel"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IManagedConnection OpenConnection(CommandRunnerMode mode = CommandRunnerMode.AutoCommit, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int? timeout = null);

        /// <summary>
        ///     Set the minimum sequence number for a Hilo sequence for a specific document type
        ///     to the specified floor. Useful for migrating data between databases
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="floor"></param>
        void ResetHiloSequenceFloor<T>(long floor);

        /// <summary>
        ///     Fetch the entity version and last modified time from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DocumentMetadata MetadataFor<T>(T entity);

        /// <summary>
        ///     Fetch the entity version and last modified time from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<DocumentMetadata> MetadataForAsync<T>(T entity,
            CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Fetch a connection to the tenant database
        /// </summary>
        /// <returns></returns>
        NpgsqlConnection CreateConnection();



        IProviderGraph Providers { get; }
    }
}
