using System.Collections;

namespace GoFDesignPatterns.Behavioral;

public record ApiUser(int Id, string Name, string Email);

public record PagedResponse<T>(IReadOnlyList<T> Items, int CurrentPage, int TotalPages, bool HasNext);

/// <summary>
/// Simulates a paginated 3rd-party REST API.
/// </summary>
public class RemoteUserDirectory
{
    private readonly List<ApiUser> _allUsers =
    [
        new(1, "Alice Morgan", "alice@enterprise.com"),
        new(2, "Bob Vance", "bob@enterprise.com"),
        new(3, "Charlie Day", "charlie@enterprise.com"),
        new(4, "Dana Scully", "dana@enterprise.com"),
        new(5, "Evan Wright", "evan@enterprise.com"),
        new(6, "Fiona Gallagher", "fiona@enterprise.com")
    ];

    public PagedResponse<ApiUser> FetchPage(int pageNumber, int pageSize = 2)
    {
        var totalPages = (int)Math.Ceiling(_allUsers.Count / (double)pageSize);
        var items = _allUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedResponse<ApiUser>(items, pageNumber, totalPages, pageNumber < totalPages);
    }
}

/// <summary>
/// Classic GoF Iterator implementing IEnumerable&lt;ApiUser&gt; over a paginated remote source.
/// Consumers iterate seamlessly with foreach without managing page tokens or offset cursors.
/// </summary>
public class PaginatedUserCollection(RemoteUserDirectory directory, int pageSize = 2) : IEnumerable<ApiUser>
{
    public IEnumerator<ApiUser> GetEnumerator()
    {
        var currentPage = 1;
        bool hasNext;

        do
        {
            var page = directory.FetchPage(currentPage, pageSize);
            foreach (var user in page.Items)
            {
                yield return user;
            }

            hasNext = page.HasNext;
            currentPage++;
        } while (hasNext);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Modern C# 12 Async Iterator using IAsyncEnumerable&lt;T&gt; with streaming cancellation support.
    /// </summary>
    public async IAsyncEnumerable<ApiUser> StreamUsersAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var currentPage = 1;
        bool hasNext;

        do
        {
            if (cancellationToken.IsCancellationRequested) yield break;

            // Simulate network latency per page request
            await Task.Delay(20, cancellationToken);
            var page = directory.FetchPage(currentPage, pageSize);

            foreach (var user in page.Items)
            {
                yield return user;
            }

            hasNext = page.HasNext;
            currentPage++;
        } while (hasNext);
    }
}
