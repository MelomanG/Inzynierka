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

            builder
                .HasOne(address => address.Pub)
                .WithOne(pub => pub.Address)
                .HasForeignKey<Address>(address => address.PubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}