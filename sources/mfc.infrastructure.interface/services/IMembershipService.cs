namespace mfc.infrastructure.services {
    public interface IMembershipService {
        bool IsUserValid(string account, string password);
    }
}
