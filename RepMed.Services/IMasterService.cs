using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Localize;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Services
{
    public interface IMasterService : IDisposable
    {
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetCounrties(Func<Country, bool> filter = null);
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetStates(Func<State, bool> filter = null);
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetCities(Func<City, bool> filter = null);
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetRoles(Func<Role, bool> filter = null);
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetProducts(Func<Product, bool> filter = null);
        Task<APIsResponse<IEnumerable<SelectListItem>>> GetPharmacyRoles(Func<Role, bool> filter = null);

    }

    public class MasterService : BaseService, IMasterService
    {

        public MasterService(IDbConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {

        }

        public  Task<APIsResponse<IEnumerable<SelectListItem>>> GetCounrties(Func<Country,bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(EntityCountriesDto.Id)} , {nameof(EntityCountriesDto.Name)} from {DbTables.tblCountry}";
                var mQuery = _idbConnection.Query<Country>(sql, transaction: _idbTransaction).Where(r=> 
                filter.Invoke(r)).Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }

        public Task<APIsResponse<IEnumerable<SelectListItem>>> GetStates(Func<State, bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(State.Id)}, {nameof(State.Name)},{nameof(State.CountryId)}  from {DbTables.tblStates}";
                var mQuery = _idbConnection.Query<State>(sql, transaction: _idbTransaction).Where(r =>
                filter.Invoke(r)).Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {

                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }

        public Task<APIsResponse<IEnumerable<SelectListItem>>> GetCities(Func<City, bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(City.Id)},{nameof(City.Name)},{nameof(City.StateId)} from {DbTables.tblCity}";
                var mQuery = _idbConnection.Query<City>(sql, transaction: _idbTransaction)
                    .Where(r => filter.Invoke(r))
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Id.ToString()
                    });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {

                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }

        public Task<APIsResponse<IEnumerable<SelectListItem>>> GetRoles(Func<Role, bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(EntityRoleDto.Id)}, {nameof(EntityRoleDto.RoleName)} from {DbTables.tblRole} where {nameof(EntityRoleDto.IsActive)}=true and { nameof(EntityRoleDto.RoleName)} !='admin' and {nameof(EntityRoleDto.IsAdminRole)} !=false";
                var mQuery = _idbConnection.Query<Role>(sql, transaction: _idbTransaction).Where(r =>
                filter.Invoke(r)).Select(r => new SelectListItem
                {
                    Text = r.RoleName,
                    Value = r.Id.ToString()
                });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }
        public Task<APIsResponse<IEnumerable<SelectListItem>>> GetPharmacyRoles(Func<Role, bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(EntityRoleDto.Id)}, {nameof(EntityRoleDto.RoleName)} from {DbTables.tblRole} where {nameof(EntityRoleDto.IsActive)}=true and {nameof(EntityRoleDto.RoleName)} !='admin' and {nameof(EntityRoleDto.IsAdminRole)} =false";
                var mQuery = _idbConnection.Query<Role>(sql, transaction: _idbTransaction).Where(r =>
                filter.Invoke(r)).Select(r => new SelectListItem
                {
                    Text = r.RoleName,
                    Value = r.Id.ToString()
                });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }
        public Task<APIsResponse<IEnumerable<SelectListItem>>> GetProducts(Func<Product, bool> filter = null)
        {
            APIsResponse<IEnumerable<SelectListItem>> response;
            try
            {
                if (filter == null)
                    filter = (d) => true;

                string sql = $@"select {nameof(Product.Id)}, {nameof(Product.Name)} from {DbTables.tblProduct}";
                var mQuery = _idbConnection.Query<Product>(sql, transaction: _idbTransaction).Where(r =>
                filter.Invoke(r)).Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                });

                response = new APIsSuccsss<IEnumerable<SelectListItem>>("Success", mQuery);
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                return Task.FromResult(new APIsError<IEnumerable<SelectListItem>>(ex.GetActualError()) as APIsResponse<IEnumerable<SelectListItem>>);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
