using Microsoft.Extensions.Logging;
using TestASP.Core.IRepository;
using TestASP.Data;
using TestASP.Domain.Contexts;
using TestASP.Domain.Repository;

namespace TestASP.Domain.Repository;

public class ImageFileRepository : BaseRepository<ImageFile>, IImageFileRepository
{
    public ImageFileRepository(TestDbContext dbContext, ILogger<ImageFileRepository> logger) : base(dbContext, logger)
    {
    }
}
