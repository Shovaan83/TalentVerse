using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentVerse.WebAPI.Common;
using TalentVerse.WebAPI.DTO.Skills;
using TalentVerse.WebAPI.Interfaces;



namespace TalentVerse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepository _skillRepo;

        public SkillsController(ISkillRepository skillRepo)
        {
            _skillRepo = skillRepo;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> AddSkill(AddSkillDto skillDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _skillRepo.AddSkillToUserAsync(userId, skillDto);

            if (success)
                return Ok(ServiceResponse<bool>.SuccessResponse(true, "Skill added successfully."));

            return BadRequest(ServiceResponse<bool>.FailureResponse("Failed to add skill."));
        }

        [HttpGet("my-skills")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SkillDto>>>> GetMySkills()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var skills = await _skillRepo.GetUserSkillsAsync(userId);

            return Ok(ServiceResponse<IEnumerable<SkillDto>>.SuccessResponse(skills));
        }

        [HttpDelete("{userSkillId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteSkill(int userSkillId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _skillRepo.DeleteUserSkillAsync(userId, userSkillId);
            if (success)
                return Ok(ServiceResponse<bool>.SuccessResponse(true, "Skill deleted successfully."));

            return BadRequest(ServiceResponse<bool>.FailureResponse("Skill not found or could not be deleted"));
        }
    }
}
