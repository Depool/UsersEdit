﻿using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using Infrastructure.Repository.Generic.Concrete.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRepository.Concrete.Entity
{
    public class RoleRepository : GenericRepository<UsersEntities, Role>, IRoleRepository
    {

        public Role GetById(int id)
        {
            return context.Role.Where(role => role.Id == id).FirstOrDefault();
        }
    }
}
