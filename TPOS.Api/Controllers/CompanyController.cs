using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces;
using TPOS.Core.Utilities;

namespace TPOS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CompanyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Use Company entity as parmeters or return types in API methods

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _unitOfWork.CompanyRepository.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet("Active")]
        public async Task<IActionResult> GetActiveCompanies()
        {
            var companies = await _unitOfWork.CompanyRepository.FindAsync(x => x.Active);
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company)
        {
            try
            {
                company.CreatedOn = company.UpdatedOn = DateTime.UtcNow;
                company.CreatedBy = company.UpdatedBy = ClaimsAccessor.UserID;
                company.Active = true;
                await _unitOfWork.CompanyRepository.AddAsync(company);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetCompanyById), new { id = company.CompanyID }, company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, Company company)
        {
            if(id != company.CompanyID )
            {
                return BadRequest();
            }
            try
            {
                var exisitingCompany = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
                if(exisitingCompany == null)
                {
                    return NotFound();
                }

                exisitingCompany.CompanyAddress = company.CompanyAddress;
                exisitingCompany.CompanyPhone = company.CompanyPhone;
                exisitingCompany.CompanyEmail = company.CompanyEmail;
                exisitingCompany.Active = company.Active;

                exisitingCompany.UpdatedOn = DateTime.UtcNow;
                exisitingCompany.UpdatedBy = ClaimsAccessor.UserID;
                _unitOfWork.CompanyRepository.Update(exisitingCompany);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    return NotFound();
                }
                company.Active = false;
                company.UpdatedOn = DateTime.UtcNow;
                company.UpdatedBy = ClaimsAccessor.UserID;

                //await _unitOfWork.CompanyRepository.DeleteAsync(id);
                _unitOfWork.CompanyRepository.Update(company);
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}
