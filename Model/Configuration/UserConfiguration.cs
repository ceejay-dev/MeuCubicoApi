using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasData
                (
                    new User
                    {
                        Name = "Cândido Bernardo",
                        BI = "004545006060",
                        Photo = "foto.png",
                        Position = "admin",
                        Email = "20200054@isptec.co.ao",
                        UserName = "tester",
                        PasswordHash = "12345678",
                        PhoneNumber = "934818736"
                    }
                );
            }
    }
}
