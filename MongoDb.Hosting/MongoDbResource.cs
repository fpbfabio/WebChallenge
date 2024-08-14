namespace Aspire.Hosting.ApplicationModel;

public sealed class MongoDbResource(string name) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string MongoDbEndpointName = "mongodb";

    private EndpointReference? _mongoDbReference;

    public EndpointReference MongoDbEndpoint =>
        _mongoDbReference ??= new(this, MongoDbEndpointName);

    // Required property on IResourceWithConnectionString. Represents a connection
    // string that applications can use to access the MailDev server. In this case
    // the connection string is composed of the SmtpEndpoint endpoint reference.
    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"mongodb://{MongoDbEndpoint.Property(EndpointProperty.Host)}:{MongoDbEndpoint.Property(EndpointProperty.Port)}"
        );
}