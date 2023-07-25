using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPackageDataStore : IPackageDataStore
    {
        private List<Package> _packages = new List<Package>();

        public Package Create(Package package)
        {
            package.Id = _packages.Count == 0
                ? 1
                : _packages[_packages.Count - 1].Id + 1;

            _packages.Add(package);

            return package;
        }

        public void Delete(int id)
        {
            _packages.RemoveAll(p => p.Id == id);
        }

        public List<Package> GetAll()
        {
            return _packages;
        }

        public Package GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _packages.Where(p => p.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Update(Package package)
        {
            var existing = _packages.Where(p => p.Id == package.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = package;
            }
        }
    }
}
