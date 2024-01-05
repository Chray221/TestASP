using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TestASP.Web;

public class CachedDataService {
    internal readonly ProtectedLocalStorage _localStorage;
    public CachedDataService (
        ProtectedLocalStorage localStorage) {
        _localStorage = localStorage;
    }
}