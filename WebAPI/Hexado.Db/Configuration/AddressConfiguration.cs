using System;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexado.Db.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasIndex(address => new { address.Street, address.BuildingNumber, address.LocalNumber, address.PostalCode, address.City })
                .IsUnique();
        }
    }
}