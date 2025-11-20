using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class CountryService : Service<Country>, IService<CountryRequest, CountryResponse>
    {
        public CountryService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Country> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(c => c.Cities).OrderBy(c => c.CountryName);
        }

        public CommandResponse Create(CountryRequest request)
        {
            if (Query().Any(c => c.CountryName == request.CountryName.Trim()))
                return Error("Country with the same name exists!");
            var entity = new Country
            {
                CountryName = request.CountryName?.Trim()
            };
            Create(entity);
            return Success("Country created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return Error("Country not found!");
            if (entity.Cities.Any())
                return Error("Country can't be deleted because it has relational cities!");
            Delete(entity);
            return Success("Country deleted successfully.", entity.Id);
        }

        public CountryRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return null;
            return new CountryRequest
            {
                Id = entity.Id,
                CountryName = entity.CountryName
            };
        }

        public CountryResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return null;
            return new CountryResponse
            {
                Id = entity.Id,
                CountryName = entity.CountryName,
                Cities = entity.Cities.Select(city => new CityResponse
                {
                    Id = city.Id,
                    CityName = city.CityName
                }).ToList()
            };
        }

        public List<CountryResponse> List()
        {
            return Query().Select(c => new CountryResponse
            {
                Id = c.Id,
                CountryName = c.CountryName,
                Cities = c.Cities.Select(city => new CityResponse
                {
                    Id = city.Id,
                    CityName = city.CityName
                }).ToList()
            }).ToList();
        }

        public CommandResponse Update(CountryRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.CountryName == request.CountryName.Trim()))
                return Error("Country with the same name exists!");
            var entity = Query(false).SingleOrDefault(c => c.Id == request.Id);
            if (entity is null)
                return Error("Country not found!");
            entity.CountryName = request.CountryName?.Trim();
            Update(entity);
            return Success("Country updated successfully.", entity.Id);
        }
    }
}

