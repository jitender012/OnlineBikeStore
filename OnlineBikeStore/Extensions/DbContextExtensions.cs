using System.Linq;

namespace OnlineBikeStore.Extensions
{
    public static class DbContextExtensions
    {
        public static int GetUserId(this BikeStoreEntities context, string userEmail)
        {
            return context.users
                .Where(x => x.email == userEmail)
                .Select(z => z.user_id)
                .FirstOrDefault();
        }
    }
}