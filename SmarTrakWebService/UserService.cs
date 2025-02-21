using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserModel>> GetAll() => await _repository.GetAll();

        public async Task<UserModel> GetById(int id) => await _repository.GetById(id);

        public async Task Add(UserModel user) => await _repository.Add(user);

        public async Task Update(UserModel user) => await _repository.Update(user);

        public async Task Delete(int id) => await _repository.Delete(id);
    }
}
