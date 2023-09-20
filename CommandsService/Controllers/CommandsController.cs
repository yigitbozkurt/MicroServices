using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform (int platformID)
        {
            System.Console.WriteLine($"Hit CommandsForPlatform: {platformID}");

            if(!_repository.PlatformExist(platformID))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformID);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
            
        }


        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformID, int commandId)
        {

            System.Console.WriteLine($"Hit CommandsForPlatform: {platformID} / {commandId}");

            if(!_repository.PlatformExist(platformID))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(platformID,commandId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId , CommandCreateDto commandDto)
        {
            System.Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if(!_repository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId,command);
            _repository.SaveChanges();

            var commandreadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
               new {platformId = platformId, commandId = commandreadDto.Id},commandreadDto);
            
        }
    }
}