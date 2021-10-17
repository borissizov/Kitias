﻿using Kitias.Persistence;
using Kitias.Persistence.Models;
using Kitias.Repository.Implementations.Base;
using Kitias.Repository.Interfaces;

namespace Kitias.Repository.Implementations
{
	public class RoleRepository : Repository<Role>, IRoleRepository
	{
		public RoleRepository(DataContext context) : base(context) { }
	}
}
