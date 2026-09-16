using CrmVentas.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmVentas.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.Telefono).HasMaxLength(30);
        builder.Property(c => c.Direccion).HasMaxLength(250);
    }
}
