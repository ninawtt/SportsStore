using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "John";
        private const string adminPassword = "Secret123$";

        private const string managerUser = "Paul";
        private const string managerPassword = "Secret456$";

        private const string adminRoleName = "Admin";
        private const string managerRoleName = "Manager";

        // ensure the database is populated with data
        // the parameter will be sent from the startup (we call this method in startup)
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            // create the role
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            // create the admin role; to find if the adminRoleName exists or not
            IdentityRole adminRole = await roleManager.FindByNameAsync(adminRoleName);

            // if the adminRole is not exist
            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                await roleManager.CreateAsync(adminRole); // save the adminRole into the database
            }

            // create the user
            UserManager<IdentityUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();

            // create the admin user; to find if the adminUser exists or not
            IdentityUser user = await userManager.FindByIdAsync(adminUser); 

            // create user, update user, delete user, find user are all async 
            // when we use async, we need to use await keyword
            // if we use async inside a method, the method should also be async

            // if the user is not exist
            if (user == null)
            {
                user = new IdentityUser(adminUser);
                await userManager.CreateAsync(user, adminPassword); // save the user into the database
                await userManager.AddToRoleAsync(user, adminRoleName); // assign the user to the adminRoleName
            }
            else
            {
                // if the user already exists, check if the user is already the adminRoleName? if not, assign it 
                if (!(await userManager.IsInRoleAsync(user, adminRoleName)))
                {
                    await userManager.AddToRoleAsync(user, adminRoleName); // assign the user to the adminRoleName
                }
            }

            // create the manager role; to find if the managerRileName exists or not
            IdentityRole managerRole = await roleManager.FindByNameAsync(managerRoleName);

            // if the managerRole is not exist
            if (managerRole == null)
            {
                managerRole = new IdentityRole(managerRoleName);
                await roleManager.CreateAsync(managerRole); // save the managerRole into the database
            }

            // create the manager user; to find if the adminUser exists or not
            IdentityUser mgrUser = await userManager.FindByIdAsync(managerUser);

            // create user, update user, delete user, find user are all async 
            // when we use async, we need to use await keyword
            // if we use async inside a method, the method should also be async

            // if the user is not exist
            if (mgrUser == null)
            {
                mgrUser = new IdentityUser(managerUser);
                await userManager.CreateAsync(mgrUser, managerPassword); // save the user into the database
                await userManager.AddToRoleAsync(mgrUser, managerRoleName); // assign the user to the adminRoleName
            }
            else
            {
                // if the user already exists, check if the user is already the adminRoleName? if not, assign it 
                if (!(await userManager.IsInRoleAsync(mgrUser, managerRoleName)))
                {
                    await userManager.AddToRoleAsync(mgrUser, managerRoleName); // assign the user to the adminRoleName
                }
            }
        }
    }
}
