﻿using Dotnet.Homeworks.MainProject.Dto;
using Dotnet.Homeworks.Shared.MessagingContracts.Email;

namespace Dotnet.Homeworks.MainProject.Services;

public class RegistrationService : IRegistrationService
{
    private readonly ICommunicationService _communicationService;

    public RegistrationService(ICommunicationService communicationService) => _communicationService = communicationService;

    public async Task RegisterAsync(RegisterUserDto userDto)
    {
        await Task.Delay(100);

        await _communicationService.SendEmailAsync(new SendEmail("", "", "", ""));
    }
}