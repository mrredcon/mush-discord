﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FalloutRPG.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RpgContext>
    {
        public RpgContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RpgContext>();

            builder.UseLazyLoadingProxies();
            builder.UseSqlite("Filename=MUSHDB.db", b => b.MigrationsAssembly("FalloutRPG.Data"));

            return new RpgContext(builder.Options);
        }
    }
}
