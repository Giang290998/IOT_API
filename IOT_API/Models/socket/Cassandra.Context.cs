using Cassandra;
using ISession = Cassandra.ISession;

namespace IOT_API.Models;

public class CassandraContext
{
    private readonly Cluster _cluster;
    private readonly ISession _session;

    public CassandraContext(string contactPoint, string username, string password, string keySpace)
    {
        _cluster = Cluster.Builder()
            .AddContactPoints(contactPoint)
            .WithCredentials(username, password)
            .Build();

        _session = _cluster.Connect(keySpace);
    }

    public ISession GetSession()
    {
        return _session;
    }

    public void Dispose()
    {
        _session.Dispose();
        _cluster.Dispose();
    }
}
