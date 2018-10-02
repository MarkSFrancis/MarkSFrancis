﻿using Microsoft.EntityFrameworkCore;

namespace Phnx.Data.EFCore.Tests.Fakes
{
    public class FakeDbContext : DbContext
    {
        public FakeDbContext(DbContextOptions opts) : base(opts)
        {
        }

        public DbSet<DataModel> Fakes { get; set; }
    }
}
