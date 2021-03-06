﻿namespace Template.Infrastructure.Tests.Integration
{
    using Xunit;
    using Template.Infrastructure.Tests.Integration.Testcontainers;
    using Template.Infrastructure.Tests.Integration.DockerClient;

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DockerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
