using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting;

public static class MongoDbResourceBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="MongoDbResource"/> to the given
    /// <paramref name="builder"/> instance. Uses the "2.0.2" tag.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="mongoDbPort">The MongoDb port.</param>
    /// <returns>
    /// An <see cref="IResourceBuilder{MongoDbResource}"/> instance that
    /// represents the added MongoDbResource resource.
    /// </returns>
    public static IResourceBuilder<MongoDbResource> AddMongoDb(
        this IDistributedApplicationBuilder builder,
        string name,
        int? mongoDbPort = null)
    {
        // The AddResource method is a core API within .NET Aspire and is
        // used by resource developers to wrap a custom resource in an
        // IResourceBuilder<T> instance. Extension methods to customize
        // the resource (if any exist) target the builder interface.
        var resource = new MongoDbResource(name);

        return builder.AddResource(resource)
                      .WithImage(MongoDbContainerImageTags.Image)
                      .WithImageRegistry(MongoDbContainerImageTags.Registry)
                      .WithImageTag(MongoDbContainerImageTags.Tag)
                      .WithEndpoint(
                          targetPort: 27017,
                          port: mongoDbPort,
                          name: MongoDbResource.MongoDbEndpointName);
    }
}

// This class just contains constant strings that can be updated periodically
// when new versions of the underlying container are released.
internal static class MongoDbContainerImageTags
{
    internal const string Registry = "docker.io";

    internal const string Image = "mongodb/mongodb-community-server";

    internal const string Tag = "5.0.21-ubuntu2004";
}